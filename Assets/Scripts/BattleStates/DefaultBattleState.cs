using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BattleState", menuName = "BattleStates/BattleState")]
public class DefaultBattleState : ScriptableObject, IBattleState
{
    [SerializeField]
    protected GameObject battleMenuPrefab;
    [SerializeField]
    protected float startDelay = 0.75f;

    protected BattleMenu battleMenu;

    public virtual IEnumerator<float> EnterState()
    {
        yield return Timing.WaitForSeconds(startDelay);

        if (battleMenuPrefab != null)
        {
            //don't need to worry about position if it's overlay menu
            GameObject menuObj = Instantiate(battleMenuPrefab);

            battleMenu = menuObj.GetComponent<BattleMenu>();
            if (battleMenu != null)
            {
                battleMenu.Initialize(this);
            }
        }

    }

    public virtual IEnumerator<float> ExitState()
    {
        if (battleMenu != null)
        {
            battleMenu.Close();
        }

        yield return Timing.WaitForSeconds(0);
    }

    public virtual bool UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }

        return false;
    }
}
