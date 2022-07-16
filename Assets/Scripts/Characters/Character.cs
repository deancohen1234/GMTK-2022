using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeightClass { Light = 0, Medium = 1, Heavy = 2 }

public class Character : MonoBehaviour
{
    public WeightClass weightClass;
    public int startingHealth;

    protected int health;
    protected int currentRoll;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public int Roll()
    {
        //can take into account weight class later

        //get a random number between 1-6
        currentRoll = Random.Range(1, 7);
        return currentRoll;
    }

    public int GetHealth()
    {
        return health;
    }


}
