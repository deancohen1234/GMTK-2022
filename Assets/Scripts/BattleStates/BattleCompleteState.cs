using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CompleteState", menuName = "BattleStates/CompleteState")]
public class BattleCompleteState : DefaultBattleState
{
    public override IEnumerator<float> EnterState()
    {
        yield return Timing.WaitForSeconds(startDelay);

        //clear bets
        BattleManager.GetBattleManager().SetBet(0);

        BattleManager.GetBattleManager().UpdateHUDValues();

        BattleManager.GetBattleManager().AdvanceBattleState();

    }
}
