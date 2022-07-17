using MEC;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    public DiceProbablity diceProbablity;

    public GameObject dicePrefab;
    public Transform diceSpawnTransform;
    public float diceMaxLaunchForce = 50f;
    public float diceMaxLaunchTorque = 50f;
    public int desiredDiceRoll = 2;

    public float characterMassOverride = 0;
    public int testCount = 500;

    private DiceProbablity instantiatedProbability;
    private GameObject launchedDice;

    private List<Vector3> dicePositions;
    private List<Quaternion> diceRotations;

    private const int DICEMAXSIMULATIONS = 1000;

    private void Start()
    {
        dicePositions = new List<Vector3>();
        diceRotations = new List<Quaternion>();

        instantiatedProbability = Instantiate(diceProbablity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ThrowDice();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TestProbability();
        }
    }

    private void TestProbability()
    {
        List<int> results = new List<int>();
        for (int i = 0; i < testCount; i++)
        {
            int diceResult = instantiatedProbability.PickWeightedDiceRoll(characterMassOverride);
            results.Add(diceResult);
        }

        float onePercent = ((float)results.FindAll(x => x == 1).Count / testCount);
        float twoPercent = ((float)results.FindAll(x => x == 2).Count / testCount);
        float threePercent = ((float)results.FindAll(x => x == 3).Count / testCount);
        float fourPercent = ((float)results.FindAll(x => x == 4).Count / testCount);
        float fivePercent = ((float)results.FindAll(x => x == 5).Count / testCount);
        float sixPercent = ((float)results.FindAll(x => x == 6).Count / testCount);

        Debug.Log($"One: {onePercent}\nTwo: {twoPercent}\nThree: {threePercent}\nFour: {fourPercent}\nFive: {fivePercent}\nSix: {sixPercent}\n");
    }

    private void ThrowDice()
    {
        //kill old dice if it exists
        if (launchedDice != null)
        {
            Destroy(launchedDice);
            launchedDice = null;
        }

        launchedDice = Instantiate(dicePrefab);
        launchedDice.transform.position = diceSpawnTransform.position;

        Rigidbody diceRB = launchedDice.GetComponent<Rigidbody>();
        diceRB.MovePosition(diceSpawnTransform.position);

        diceRB.AddForce(new Vector3(Random.Range(-diceMaxLaunchForce, diceMaxLaunchForce),
            Random.Range(0, 0),
            Random.Range(-diceMaxLaunchForce, diceMaxLaunchForce)));

        //diceRB.AddRelativeTorque(new Vector3(Random.Range(-diceMaxLaunchTorque, diceMaxLaunchTorque),
        //    Random.Range(-diceMaxLaunchTorque, diceMaxLaunchTorque),
        //    Random.Range(-diceMaxLaunchTorque, diceMaxLaunchTorque)), ForceMode.VelocityChange);

        diceRB.maxAngularVelocity = diceMaxLaunchTorque * 2f;
        diceRB.angularVelocity = new Vector3(Random.Range(-diceMaxLaunchTorque, diceMaxLaunchTorque),
            Random.Range(-diceMaxLaunchTorque, diceMaxLaunchTorque),
            Random.Range(-diceMaxLaunchTorque, diceMaxLaunchTorque));

        SimulateDiceRoll(diceRB, desiredDiceRoll);

    }

    private void SimulateDiceRoll(Rigidbody diceRB, int desiredValue)
    {
        Physics.autoSimulation = false;

        dicePositions.Clear();
        diceRotations.Clear();

        int endingFaceUp = 0;
        for (int i = 0; i < DICEMAXSIMULATIONS; i++)
        {
            if (diceRB.IsSleeping())
            {
                //keep track of upright axis
                Dice dice = diceRB.gameObject.GetComponent<Dice>();
                if (dice != null)
                {
                    //keep track of current face up
                    endingFaceUp = dice.GetFaceUpNumber();
                }

                break;
            }
            else
            {
                //keep keeping track of positions
                Physics.Simulate(Time.fixedDeltaTime);

                dicePositions.Add(diceRB.position);
                diceRotations.Add(diceRB.rotation);
            }
        }

        diceRB.position = dicePositions[0];
        diceRB.rotation = diceRotations[0];

        Physics.Simulate(Time.fixedDeltaTime);

        //turn physics on again incase anything needs it
        Physics.autoSimulation = true;


        //now we have fully simulated the collision
        //now run the simulation through Timing
        Timing.RunCoroutine(FakeSimulateDiceRoll(diceRB, desiredValue, endingFaceUp));
    }

    private IEnumerator<float> FakeSimulateDiceRoll(Rigidbody diceRB, int desiredUpValue, int endingFaceUp)
    {
        Dice dice = diceRB.GetComponent<Dice>();

        if (dice == null)
        {
            Debug.LogError("WTF it's all scuffed");
            yield break;
        }

        //don't let dice ACTUALLY simulate
        diceRB.isKinematic = true;

        //adjust starting rotation so desired face up is where ending faceup is
        //retroactively go through ALL rotations and apply adjustment
        Quaternion adjustmentRot = Quaternion.identity * Quaternion.FromToRotation(dice.GetDiceAxis(endingFaceUp).axis, dice.GetDiceAxis(desiredUpValue).axis);

        //doubleCheck adjustment is correct
        diceRB.MoveRotation(diceRB.rotation * adjustmentRot);
        if (dice.GetFaceUpNumber() != desiredUpValue)
        {
            //we got a fucky rotation, reverse it
            adjustmentRot = Quaternion.identity * Quaternion.FromToRotation(dice.GetDiceAxis(desiredUpValue).axis, dice.GetDiceAxis(endingFaceUp).axis);
        }
        //reset rotation
        diceRB.MoveRotation(diceRotations[0]);

        if (dice.diceMeshTransform != null)
        {
            dice.diceMeshTransform.rotation = dice.diceMeshTransform.rotation * adjustmentRot;
        }

        int simulationIndex = 0;
        while (simulationIndex < dicePositions.Count)
        {
            yield return Timing.WaitForSeconds(Time.fixedDeltaTime);
            diceRB.MovePosition(dicePositions[simulationIndex]);
            diceRB.MoveRotation(diceRotations[simulationIndex]);
            simulationIndex++;
        }

        diceRB.isKinematic = false;

    }
}
