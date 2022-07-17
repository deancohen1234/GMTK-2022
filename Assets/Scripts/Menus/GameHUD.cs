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
    public Image enemyWeightBar;
    public Transform curtainLeft;
    public Transform curtainRight;
    public TextMeshProUGUI enemyName;
    public Ease curtainEase = Ease.OutBack;
    public float curtainMoveDuration = 1.0f;

    public override void Initialize(IBattleState _parentState)
    {
        base.Initialize(_parentState);

        playerHealthBar.Initialize(BattleManager.GetBattleManager().GetPlayerCharacter().GetHealth());
        enemyHealthBar.Initialize(BattleManager.GetBattleManager().GetEnemyCharacter().GetHealth());

        //show weight in weight bar
        float fillPercent = (BattleManager.GetBattleManager().GetEnemyCharacter().mass + 1f) * 0.5f;
        enemyWeightBar.fillAmount = fillPercent;

        //update enemy name
        if (BattleManager.GetBattleManager().GetEnemyCharacter())
        {
            enemyName.text = BattleManager.GetBattleManager().GetEnemyCharacter().characterName;
        }

    }

    public override void UpdateMenu()
    {
        base.UpdateMenu();

        //recalcuate characters health
        UpdateCharactersHealth();

        //update enemy name
        if (BattleManager.GetBattleManager().GetEnemyCharacter())
        {
            enemyName.text = BattleManager.GetBattleManager().GetEnemyCharacter().characterName;
        }
    }

    public void CloseCurtains()
    {
        curtainLeft.GetComponent<RectTransform>().DOAnchorPosX(0f, curtainMoveDuration).SetEase(curtainEase);
        curtainRight .GetComponent<RectTransform>().DOAnchorPosX(0f, curtainMoveDuration).SetEase(curtainEase);
    }

    public void OpenCurtains()
    {
        curtainLeft.GetComponent<RectTransform>().DOAnchorPosX(-2000f, curtainMoveDuration).SetEase(curtainEase);
        curtainRight.GetComponent<RectTransform>().DOAnchorPosX(2000f, curtainMoveDuration).SetEase(curtainEase);
    }

    //only update health if values have changed
    private void UpdateCharactersHealth()
    {
        playerHealthBar.UpdateHealthValue(BattleManager.GetBattleManager().GetPlayerCharacter().GetHealth());
        enemyHealthBar.UpdateHealthValue(BattleManager.GetBattleManager().GetEnemyCharacter().GetHealth());

        //show weight in weight bar
        float fillPercent = (BattleManager.GetBattleManager().GetEnemyCharacter().mass + 1f) * 0.5f;
        enemyWeightBar.fillAmount = fillPercent;
    }

}
