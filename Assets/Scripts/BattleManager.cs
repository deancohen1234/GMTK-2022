using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class BattleManager : MonoBehaviour
{
    public enum BattleState : byte { PreBattle = 0, PlayerRoll = 1, PlayerHighLow = 2, PlayerBet = 3, PlayerFinish = 4, EnemyRoll = 5, BattleOutcome = 6 }

    [Header("Timings")]
    public float preBattleDelay = 0.5f;

    public DefaultBattleState[] allBattleStates;

    private IBattleState[] stateInterfaces;
    private int currentBattleStateIndex = 0;

    private static BattleManager singleton;

    public static BattleManager GetBattleManager()
    {
        if (singleton == null)
        {
            singleton = FindObjectOfType<BattleManager>();

            if (singleton == null)
            {
                Debug.LogError("Yike couldn't find Battle Manager :(");
            }
        }

        return singleton;
    }

    // Start is called before the first frame update
    void Awake()
    {
        stateInterfaces = new IBattleState[allBattleStates.Length];
        for (int i = 0; i < allBattleStates.Length; i++)
        {
            stateInterfaces[i] = allBattleStates[i];
        }

    }

    private void Start()
    {
        //start at first state and run
        currentBattleStateIndex = 0;
        Timing.RunCoroutine(GetCurrentBattleState().EnterState());
    }

    // Update is called once per frame
    void Update()
    {
        IBattleState currentBattleState = GetCurrentBattleState();
        if (currentBattleState != null)
        {
            bool completed = currentBattleState.UpdateState();

            if (completed)
            {
                AdvanceBattleState();
            }
        }
    }

    public void AdvanceBattleState()
    {
        Timing.RunCoroutine(GetCurrentBattleState().ExitState());

        //update current state and start next one
        currentBattleStateIndex = (int)Mathf.Repeat(currentBattleStateIndex + 1, stateInterfaces.Length);
        Timing.RunCoroutine(GetCurrentBattleState().EnterState());
    }

    private IBattleState GetCurrentBattleState()
    {
        if (currentBattleStateIndex < stateInterfaces.Length)
        {
            return stateInterfaces[currentBattleStateIndex];
        }
        else
        {
            return null;
        }
    }
}
