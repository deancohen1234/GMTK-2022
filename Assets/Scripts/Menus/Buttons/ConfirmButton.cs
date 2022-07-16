using MEC;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmButton : BattleButton
{
    public override void OnClick()
    {
        base.OnClick();

        BattleManager.GetBattleManager().AdvanceBattleState();
    }
}
