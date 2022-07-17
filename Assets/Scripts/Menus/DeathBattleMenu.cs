using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathBattleMenu : BattleMenu
{
    public TextMeshProUGUI endText;
    public string numKillsPrefix = "Enemies Out-Diced: <color=green>";
    public override void Initialize(IBattleState _parentState)
    {
        base.Initialize(_parentState);

        //display num kills
        endText.text = numKillsPrefix + BattleManager.GetBattleManager().GetNumKills() + "</color>";
    }
}
