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
    // Start is called before the first frame update


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
    }
}
