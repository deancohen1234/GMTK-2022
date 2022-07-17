using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : BattleMenu
{
    [Header("References")]
    public HealthBar playerHealthBar;
    public HealthBar enemyHealthBar;
    public BetGauge gambitGauge;

    public override void Initialize(IBattleState _parentState)
    {
        base.Initialize(_parentState);

        playerHealthBar.Initialize(BattleManager.GetBattleManager().GetPlayerCharacter().GetHealth());
        enemyHealthBar.Initialize(BattleManager.GetBattleManager().GetEnemyCharacter().GetHealth());
    }

    public override void UpdateMenu()
    {
        base.UpdateMenu();

        //recalcuate characters health
        UpdateCharactersHealth();

        //update the current bet
        gambitGauge.SetBetAmount(BattleManager.GetBattleManager().GetBet());
    }

    //only update health if values have changed
    private void UpdateCharactersHealth()
    {
        playerHealthBar.UpdateHealthValue(BattleManager.GetBattleManager().GetPlayerCharacter().GetHealth());
        enemyHealthBar.UpdateHealthValue(BattleManager.GetBattleManager().GetEnemyCharacter().GetHealth());
    }

}
