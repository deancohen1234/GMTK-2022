using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationState { Idle = 0, Attack = 1, Hurt = 2 }

public class Character : MonoBehaviour
{
    [Header("Sprites")]
    public string characterName;
    public SpriteRenderer spriteRenderer;
    public Sprite idleSprite;
    public Sprite attackSprite;
    public Sprite hurtSprite;

    public int startingHealth;
    [Range(-1f, 1f)]
    public float mass = 0;
    public bool isPlayer = false;

    protected DiceRoller diceRoller;

    protected int health;
    protected int currentRoll;

    private void Awake()
    {
        health = startingHealth;

        diceRoller = GetComponent<DiceRoller>();
    }

    private void Start()
    {
        if (diceRoller)
        {
            if (isPlayer)
            {
                diceRoller.SetSpawnTransform(BattleManager.GetBattleManager().playerDiceSpawnTransform);
            }
            else
            {
                diceRoller.SetSpawnTransform(BattleManager.GetBattleManager().enemyDiceSpawnTransform);
            }
        }
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
        //Debug.Log("Character: " + gameObject.name + " died");
    }

    public float GetPercentOfMaxHealth()
    {
        return (float)health / (float)startingHealth;
    }

    public int GetHealth()
    {
        return health;
    }

    public bool IsDead()
    {
        return health <= 0;
    }
    #endregion

    #region Rolling and Dice
    public int Roll()
    {
        //can take into account weight class later
        currentRoll = diceRoller.RollWeightedDice(mass);

        return currentRoll;
    }

    public int GetLastRoll()
    {
        return currentRoll;
    }

    public void ClearDice()
    {
        diceRoller.ClearDice();
    }
    #endregion

    #region Animation
    public void SetAnimationState(AnimationState state)
    {
        switch (state)
        {
            case AnimationState.Idle:
                spriteRenderer.sprite = idleSprite;
                break;
            case AnimationState.Attack:
                spriteRenderer.sprite = attackSprite;
                break;
            case AnimationState.Hurt:
                spriteRenderer.sprite = hurtSprite;
                break;

        }
    }
    #endregion

}
