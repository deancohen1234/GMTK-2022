using MEC;
using System.Collections.Generic;
using UnityEngine;

public class HighLowSelectButton : BattleButton
{
    public int playerGuess = 0;
    public override void OnClick()
    {
        base.OnClick();

        //Set guess in battle manager
        BattleManager.GetBattleManager().SetPlayerHighLowGuess(playerGuess);

        Debug.Log("Set Player Guess to <color=green>" + playerGuess + "</color>");
        
        //tell battle manager to advance to next state
        BattleManager.GetBattleManager().AdvanceBattleState();
    }
}
