using MEC;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyRollMenu : BattleMenu
{
    public TextMeshProUGUI enemyRollText;
    public float enemyPreRollDelay = 0.75f;
    public float enemyPostRollDelay = 2.5f;
    public override void Initialize(IBattleState _parentState)
    {
        base.Initialize(_parentState);

        Timing.RunCoroutine(_DoEnemyRoll());
    }

    private IEnumerator<float> _DoEnemyRoll()
    {
        enemyRollText.text = "";

        yield return Timing.WaitForSeconds(enemyPreRollDelay);

        int enemyRoll = BattleManager.GetBattleManager().GetEnemyCharacter().Roll();

        enemyRollText.text = enemyRoll.ToString();

        yield return Timing.WaitForSeconds(enemyPostRollDelay);

        BattleManager.GetBattleManager().AdvanceBattleState();
    }
}
