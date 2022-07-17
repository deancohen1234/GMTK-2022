using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : BattleMenu
{
    [Header("References")]
    public HealthBar playerHealthBar;
    public HealthBar enemyHealthBar;

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
    }

    //only update health if values have changed
    private void UpdateCharactersHealth()
    {
        playerHealthBar.UpdateHealthValue(BattleManager.GetBattleManager().GetPlayerCharacter().GetHealth());
        enemyHealthBar.UpdateHealthValue(BattleManager.GetBattleManager().GetEnemyCharacter().GetHealth());
    }

}
