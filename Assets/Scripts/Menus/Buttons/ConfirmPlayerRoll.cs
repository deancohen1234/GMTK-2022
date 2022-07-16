using MEC;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmPlayerRoll : BattleButton
{
    public float preRollDisplayDelay = 0.75f;
    public float postRollDisplayDelay = 0.75f;
    public override void OnClick()
    {
        base.OnClick();

        //Tell player to roll
        int playerRoll = BattleManager.GetBattleManager().GetPlayerCharacter().Roll();

        //do animation shit with this roll
        Timing.RunCoroutine(_ShowPlayerRoll(playerRoll));
    }

    private IEnumerator<float> _ShowPlayerRoll(int playerRoll)
    {
        yield return Timing.WaitForSeconds(preRollDisplayDelay);

        //show the roll
        Debug.Log("Player Rolled a <color=green>" + playerRoll + "</color>");

        yield return Timing.WaitForSeconds(postRollDisplayDelay);

        //tell battle manager to advance to next state
        BattleManager.GetBattleManager().AdvanceBattleState();
    }

}
