using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Input Battle State", menuName = "BattleStates/InputState")]
public class InputBattleState : DefaultBattleState
{
    public float startDelay = 0.75f;
    public override IEnumerator<float> EnterState()
    {
        yield return Timing.WaitForSeconds(startDelay);

        Debug.Log("Entering BS: " + name);
    }

    public override IEnumerator<float> ExitState()
    {
        Debug.Log("Exiting BS: " + name);
        yield return Timing.WaitForSeconds(0);
    }

    public override bool UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }

        return false;
    }
}
