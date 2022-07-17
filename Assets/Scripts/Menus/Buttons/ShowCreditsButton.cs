using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCreditsButton : BattleButton
{
    public GameObject credits;
    public override void OnClick()
    {
        base.OnClick();

        credits.SetActive(!credits.activeInHierarchy);
    }
}
