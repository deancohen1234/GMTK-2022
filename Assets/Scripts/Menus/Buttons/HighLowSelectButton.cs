using MEC;
using System.Collections.Generic;
using UnityEngine;

public class HighLowSelectButton : BattleButton
{
    public bool isGuessingHigh = false;
    public override void OnClick()
    {
        base.OnClick();

        //Set guess in battle manager
        BattleManager.GetBattleManager().SetPlayerHighLowGuess(isGuessingHigh);
        
        //tell battle manager to advance to next state
        BattleManager.GetBattleManager().AdvanceBattleState();
    }
}
