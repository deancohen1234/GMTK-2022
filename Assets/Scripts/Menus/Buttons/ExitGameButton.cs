using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGameButton : BattleButton
{
    public override void OnClick()
    {
        Application.Quit();
    }
}
