using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BattleState", menuName = "BattleStates/BattleState")]
public class DefaultBattleState : ScriptableObject, IBattleState
{
    public virtual IEnumerator<float> EnterState()
    {
        Debug.LogError("BAD NO ENTER");
        yield return 0f;
    }

    public virtual IEnumerator<float> ExitState()
    {
        Debug.LogError("BAD NO EXIT");
        yield return 0f;
    }

    public virtual bool UpdateState()
    {
        return false;
    }
}
