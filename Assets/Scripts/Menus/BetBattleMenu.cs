using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BetBattleMenu : BattleMenu
{
    public TextMeshProUGUI betAmountText;

    public Color betDefaultColor = Color.black;
    public Color betActiveColor = Color.yellow;

    public override void UpdateMenu()
    {
        base.UpdateMenu();

        UpdateBetValue();
    }
    private void UpdateBetValue()
    {
        //update the current bet
        int currentBet = BattleManager.GetBattleManager().GetBet();
        if (currentBet <= 0)
        {
            betAmountText.color = betDefaultColor;
        }
        else
        {
            betAmountText.color = betActiveColor;
        }
        betAmountText.text = "+" + currentBet.ToString();
    }
}
