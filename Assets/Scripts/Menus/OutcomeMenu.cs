using DG.Tweening;
using MEC;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OutcomeMenu : BattleMenu
{
    [Header("References")]
    public TextMeshProUGUI enemyRollText;
    public TextMeshProUGUI playerRollText;
    public Transform enemyTextEnd;
    public Transform playerTextEnd;

    [Header("Tweening")]
    public float tweenDuration = 0.9f;
    public Ease tweenEase = Ease.OutBack;

    [Header("Camera Shake")]
    public float duration = 0.75f;
    public float intensity = 4f;

    [Header("Timing")]
    public float outcomePreDelay = 0.75f;
    public float outcomePostDelay = 2.25f;

    private int enemyRoll, playerRoll = 0;

    public override void Initialize(IBattleState _parentState)
    {
        base.Initialize(_parentState);
        
        playerRoll = BattleManager.GetBattleManager().GetPlayerCharacter().GetLastRoll();
        enemyRoll = BattleManager.GetBattleManager().GetEnemyCharacter().GetLastRoll();

        //update text
        enemyRollText.text = enemyRoll.ToString();
        playerRollText.text = playerRoll.ToString();

        //play two tweens at the same time
        Sequence sequence = DOTween.Sequence();
        sequence.Append(enemyRollText.transform.DOMove(enemyTextEnd.position, tweenDuration).SetEase(tweenEase));
        sequence.Insert(0, playerRollText.transform.DOMove(playerTextEnd.position, tweenDuration).SetEase(tweenEase));
        sequence.OnComplete(CompareStartSequenceComplete);

    }

    private void CompareStartSequenceComplete()
    {
        bool isPlayerGuessingHigh = BattleManager.GetBattleManager().GetPlayerHighLowGuess();

        int rollDif = Mathf.Abs(playerRoll - enemyRoll);
        if (DidPlayerWin(playerRoll, enemyRoll, isPlayerGuessingHigh))
        {
            PlayerWin(rollDif);
        }
        else if (rollDif == 0)
        {
            //if player rolled 1 then player wins in tie,
            if (playerRoll == 1)
            {
                PlayerWin(1);
            }
            //if enemy rolled 6 then enemy wins in tie
            else if (enemyRoll == 6)
            {
                EnemyWin(1);
            }
            else
            {
                //it's a wash
                Timing.RunCoroutine(DelayEndOfState());
            }
        }
        else
        {
            //enemy full won
            EnemyWin(rollDif);
        }
    }

    private void PlayerWin(int rollDif)
    {
        BattleManager.GetBattleManager().GetPlayerCharacter().SetAnimationState(AnimationState.Attack);
        BattleManager.GetBattleManager().GetEnemyCharacter().SetAnimationState(AnimationState.Hurt);

        //shake camera
        CameraShakeUtility.GetCameraShake().ShakeCamera(duration, intensity + rollDif);

        int damage = Mathf.RoundToInt(rollDif * BattleManager.GetBattleManager().GetBetDamageMultiplier());
        int enemyHealthRemaining = BattleManager.GetBattleManager().GetEnemyCharacter().Hurt(damage);

        if (enemyHealthRemaining < 0)
        {
            //heal by the overkill damage you do to the enemy
            BattleManager.GetBattleManager().GetPlayerCharacter().Heal(Mathf.Abs(enemyHealthRemaining));
        }

        Timing.RunCoroutine(DelayEndOfState());
    }

    private void EnemyWin(int rollDif)
    {
        BattleManager.GetBattleManager().GetPlayerCharacter().SetAnimationState(AnimationState.Hurt);
        BattleManager.GetBattleManager().GetEnemyCharacter().SetAnimationState(AnimationState.Attack);

        //shake camera
        CameraShakeUtility.GetCameraShake().ShakeCamera(duration, intensity + rollDif);

        int damage = Mathf.RoundToInt(rollDif * BattleManager.GetBattleManager().GetBetDamageMultiplier());
        BattleManager.GetBattleManager().GetPlayerCharacter().Hurt(damage);

        Timing.RunCoroutine(DelayEndOfState());
    }

    private IEnumerator<float> DelayEndOfState()
    {
        yield return Timing.WaitForSeconds(outcomePostDelay);

        //reset animations
        BattleManager.GetBattleManager().GetPlayerCharacter().SetAnimationState(AnimationState.Idle);
        BattleManager.GetBattleManager().GetEnemyCharacter().SetAnimationState(AnimationState.Idle);

        BattleManager.GetBattleManager().AdvanceBattleState();
    }

    private bool DidPlayerWin(int playerRoll, int enemyRoll, bool playerHighLowGuess)
    {
        if (playerHighLowGuess)
        {
            if (playerRoll < enemyRoll)
            {
                return true;
            }
            //less than OR equal to
            else
            {
                return false;
            }
        }
        else
        {
            if (playerRoll > enemyRoll)
            {
                return true;
            }
            //less than OR equal to
            else
            {
                return false;
            }
        }
    }
}
