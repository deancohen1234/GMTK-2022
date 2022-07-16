using MEC;
using System.Collections.Generic;
using UnityEngine;

public class BetSelectionButton : BattleButton
{
    //+1 for up -1 for down
    public int betDirection = 0;
    public override void OnClick()
    {
        base.OnClick();

        //Set guess in battle manager
        int currentBet = BattleManager.GetBattleManager().UpdateBet(betDirection);

        Debug.Log("Updated bet to <color=green>" + currentBet + "</color>");

        //tell battle manager to advance to next state
        //BattleManager.GetBattleManager().AdvanceBattleState();
    }
}
