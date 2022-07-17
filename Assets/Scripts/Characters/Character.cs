using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeightClass { Light = 0, Medium = 1, Heavy = 2 }

public class Character : MonoBehaviour
{
    public WeightClass weightClass;
    public int startingHealth;

    protected DiceRoller diceRoller;

    protected int health;
    protected int currentRoll;

    private void Awake()
    {
        health = startingHealth;

        diceRoller = GetComponent<DiceRoller>();
    }

    #region Health
    public int Hurt(int damageAmount)
    {
        health -= damageAmount;

        //have UI reflect health changes BEFORE death
        BattleManager.GetBattleManager().UpdateHUDValues();

        if (health <= 0)
        {
            Die();
        }

        return health;
    } 

    public int Heal(int healAmount)
    {
        health = Mathf.Min(health + healAmount, startingHealth);

        //have UI reflect health changes BEFORE death
        BattleManager.GetBattleManager().UpdateHUDValues();

        return health;
    }

    public void Die()
    {
        Debug.Log("Character: " + gameObject.name + " died");
    }

    public float GetPercentOfMaxHealth()
    {
        return (float)health / (float)startingHealth;
    }
    #endregion

    public int Roll()
    {
        //can take into account weight class later
        //return diceRoller.RollWeightedDice();

        //get a random number between 1-6
        currentRoll = Random.Range(1, 7);
        return currentRoll;
    }

    public int GetLastRoll()
    {
        return currentRoll;
    }

    public int GetHealth()
    {
        return health;
    }


}
