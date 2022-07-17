using MEC;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    public DiceProbablity diceProbablity;

    public GameObject dicePrefab;
    public float diceMaxLaunchForce = 50f;
    public Vector2 diceMaxLaunchTorqueMinMax = new Vector2(10f, 20f);
    public Transform spawnPosition;
    public AudioClip[] rollAudioClips;

    [Header("Debug")]
    public int desiredDiceRollOverride = 2;

    private DiceProbablity instantiatedProbability;
    private GameObject launchedDice;
    private Transform diceSpawnTransform;

    private List<Vector3> dicePositions;
    private List<Quaternion> diceRotations;

    private AudioSource source;

    private const int DICEMAXSIMULATIONS = 1000;

    private void Start()
    {
        dicePositions = new List<Vector3>();
        diceRotations = new List<Quaternion>();

        instantiatedProbability = Instantiate(diceProbablity);

        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (spawnPosition != null)
            {
                SetSpawnTransform(spawnPosition);
            }

            ThrowDice(desiredDiceRollOverride);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            CameraShakeUtility.GetCameraShake().ShakeCamera(0.75f, 8f);
        }
    }

    public void PlayRollSound()
    {
        AudioClip sound = rollAudioClips[Random.Range(0, rollAudioClips.Length)];
        
        if (source != null)
        {
            source.clip = sound;
            source.Play();
        }
    }

    //private void TestProbability()
    //{
    //    List<int> results = new List<int>();
    //    for (int i = 0; i < testCount; i++)
    //    {
    //        int diceResult = instantiatedProbability.PickWeightedDiceRoll(characterMassOverride);
    //        results.Add(diceResult);
    //    }

    //    float onePercent = ((float)results.FindAll(x => x == 1).Count / testCount);
    //    float twoPercent = ((float)results.FindAll(x => x == 2).Count / testCount);
    //    float threePercent = ((float)results.FindAll(x => x == 3).Count / testCount);
    //    float fourPercent = ((float)results.FindAll(x => x == 4).Count / testCount);
    //    float fivePercent = ((float)results.FindAll(x => x == 5).Count / testCount);
    //    float sixPercent = ((float)results.FindAll(x => x == 6).Count / testCount);

    //    Debug.Log($"One: {onePercent}\nTwo: {twoPercent}\nThree: {threePercent}\nFour: {fourPercent}\nFive: {fivePercent}\nSix: {sixPercent}\n");
    //}

    public void SetSpawnTransform(Transform spawnTransform)
    {
        diceSpawnTransform = spawnTransform;
    }

    public void ClearDice()
    {
        if (launchedDice != null)
        {
            Destroy(launchedDice);
            launchedDice = null;
        }
    }

    public int RollWeightedDice(float characterWeight)
    {
        //get weighted result from character
        int diceResult = instantiatedProbability.PickWeightedDiceRoll(characterWeight);

        //roll physical dice to match result
        ThrowDice(diceResult);

        return diceResult;
    }

    private void ThrowDice(int desiredValue)
    {
        PlayRollSound();

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

        diceRB.maxAngularVelocity = diceMaxLaunchTorqueMinMax.y * 2f;
        diceRB.angularVelocity = new Vector3(Random.Range(diceMaxLaunchTorqueMinMax.x, diceMaxLaunchTorqueMinMax.y),
            Random.Range(diceMaxLaunchTorqueMinMax.x, diceMaxLaunchTorqueMinMax.y),
            Random.Range(diceMaxLaunchTorqueMinMax.x, diceMaxLaunchTorqueMinMax.y));

        if (desiredDiceRollOverride > 0)
        {
            desiredValue = desiredDiceRollOverride;
        }

        SimulateDiceRoll(diceRB, desiredValue);

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
        //have to set transforms since GetFaceUpNumber
        dice.transform.rotation = diceRotations[diceRotations.Count - 1];
        Physics.SyncTransforms();
        if (dice.GetFaceUpNumber() != desiredUpValue)
        {
            //we got a fucky rotation, reverse it
            adjustmentRot = Quaternion.identity * Quaternion.FromToRotation(dice.GetDiceAxis(desiredUpValue).axis, dice.GetDiceAxis(endingFaceUp).axis);
            //Debug.Log("Got a fucky ROT");
        }

        //reset rotation
        //diceRB.MoveRotation(diceRotations[0]);
        diceRB.rotation = diceRotations[0];

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
