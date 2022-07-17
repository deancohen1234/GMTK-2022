using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameButton : BattleButton
{
    public override void OnClick()
    {
        BattleManager.GetBattleManager().StartGame();
    }
}
