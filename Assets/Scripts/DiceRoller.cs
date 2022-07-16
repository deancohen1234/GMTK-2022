using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    public GameObject dicePrefab;
    public Transform diceSpawnTransform;
    public float diceMaxLaunchForce = 50f;
    public float diceMaxLaunchTorque = 50f;
    public int desiredDiceRoll = 2;

    private GameObject launchedDice;

    
    private List<Vector3> dicePositions;
    private List<Quaternion> diceRotations;

    private const int DICEMAXSIMULATIONS = 1000;

    private void Start()
    {
        dicePositions = new List<Vector3>();
        diceRotations = new List<Quaternion>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ThrowDice();
        }
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

        diceRB.AddForce(new Vector3(Random.Range(-diceMaxLaunchForce, diceMaxLaunchForce),
            Random.Range(0, 0),
            Random.Range(-diceMaxLaunchForce, diceMaxLaunchForce)));

        diceRB.AddTorque(new Vector3(Random.Range(-diceMaxLaunchTorque, diceMaxLaunchTorque),
            Random.Range(-diceMaxLaunchTorque, diceMaxLaunchTorque),
            Random.Range(-diceMaxLaunchTorque, diceMaxLaunchTorque)));

        SimulateDiceRoll(diceRB, desiredDiceRoll);

    }

    private void SimulateDiceRoll(Rigidbody diceRB, int desiredValue)
    {
        Physics.autoSimulation = false;

        dicePositions.Clear();
        diceRotations.Clear();

        //add starting positions
        dicePositions.Add(diceRB.position);
        diceRotations.Add(diceRB.rotation);

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

        //Debug.Log("desiredUpValue: " + endingFaceUp);

        int simulationIndex = 0;
        while (simulationIndex < dicePositions.Count)
        {
            yield return Timing.WaitForSeconds(Time.fixedDeltaTime);
            diceRB.MovePosition(dicePositions[simulationIndex]);
            diceRB.MoveRotation(diceRotations[simulationIndex]);
            simulationIndex++;
        }


    }
}
