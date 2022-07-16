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

        SimulateDiceRoll(diceRB);

    }

    private void SimulateDiceRoll(Rigidbody diceRB)
    {
        Physics.autoSimulation = false;

        dicePositions.Clear();
        diceRotations.Clear();

        //add starting positions
        dicePositions.Add(diceRB.position);
        diceRotations.Add(diceRB.rotation);

        int desiredUpValue = 0;
        for (int i = 0; i < DICEMAXSIMULATIONS; i++)
        {
            if (diceRB.IsSleeping())
            {
                //keep track of upright axis
                Dice dice = diceRB.gameObject.GetComponent<Dice>();
                if (dice != null)
                {
                    desiredUpValue = dice.GetFaceUpNumber();
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
        Timing.RunCoroutine(FakeSimulateDiceRoll(diceRB, desiredUpValue));
    }

    private IEnumerator<float> FakeSimulateDiceRoll(Rigidbody diceRB, int desiredUpValue)
    {
        //don't let dice ACTUALLY simulate
        diceRB.isKinematic = true;

        Debug.Log("desiredUpValue: " + desiredUpValue);

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
