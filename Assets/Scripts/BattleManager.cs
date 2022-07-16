using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class BattleManager : MonoBehaviour
{
    public enum BattleState : byte { PreBattle = 0, PlayerRoll = 1, PlayerHighLow = 2, PlayerBet = 3, PlayerFinish = 4, EnemyRoll = 5, BattleOutcome = 6 }

    [Header("Timings")]
    public float preBattleDelay = 0.5f;

    [Header("Battle States")]
    public DefaultBattleState[] allBattleStates;

    [Header("Character Prefab")]
    public GameObject playerPrefab;
    public GameObject[] enemyPrefabs;
    public Transform playerStartTransform;
    public Transform enemyStartTransform;

    private IBattleState[] stateInterfaces;
    private int currentBattleStateIndex = 0;

    //Characters
    private Character player;
    private Character currentEnemy;

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
        SpawnCharacters();

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

    #region BattleStates
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
    #endregion

    #region Characters
    private void SpawnCharacters()
    {
        //initialize characters
        GameObject playerObj = Instantiate(playerPrefab);
        playerObj.transform.position = playerStartTransform.position;
        playerObj.transform.rotation = playerStartTransform.rotation;

        player = playerObj.GetComponent<Character>();
        if (player == null) { Debug.LogError("No player in Battle Manager!"); return; }

        //spawn FIRST enemy
        if (enemyPrefabs.Length == 0) { Debug.LogError("Enemy Prefabs Array is empty"); return; }
        GameObject enemyObj = Instantiate(enemyPrefabs[0]);
        enemyObj.transform.position = enemyStartTransform.position;
        enemyObj.transform.rotation = enemyStartTransform.rotation;

        currentEnemy = enemyObj.GetComponent<Character>();
        if (currentEnemy == null) { Debug.LogError("No current enemy in Battle Manager!"); return; }
    }

    public Character GetPlayerCharacter()
    {
        return player;
    }

    public Character GetEnemyCharacter()
    {
        return currentEnemy;
    }
    #endregion
}
