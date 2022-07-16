using MEC;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Player Roll Battle State", menuName = "BattleStates/PlayerRoll")]
public class PlayerRollBattleState : DefaultBattleState
{
    public float startDelay = 0.75f;

    public override IEnumerator<float> EnterState()
    {
        yield return Timing.WaitForSeconds(startDelay);
        Debug.Log("IN player State");

        if (battleMenuPrefab != null)
        {
            Debug.Log("Battle Menu Spawning");
            //don't need to worry about position if it's overlay menu
            GameObject menuObj = Instantiate(battleMenuPrefab);

            battleMenu = menuObj.GetComponent<BattleMenu>();
            if (battleMenu != null)
            {
                battleMenu.Initialize(this);
            }
        }
        
    }

    public override IEnumerator<float> ExitState()
    {
        Debug.Log("Exiting BS: " + name);

        if (battleMenu != null)
        {
            battleMenu.Close();
        }

        yield return Timing.WaitForSeconds(0);
    }

    public override bool UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }

        return false;
    }
}
