using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleState
{
    public IEnumerator<float> EnterState();
    //called every frame
    //if return true, then exit the state
    public bool UpdateState();
    public IEnumerator<float> ExitState();
}
