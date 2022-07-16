using UnityEngine;

public class BattleMenu : MonoBehaviour
{
    [SerializeField]
    protected BattleButton[] buttons;
    protected IBattleState parentState;

    public virtual void Initialize(IBattleState _parentState)
    {
        parentState = _parentState;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].Initialize(this);
        }
    }

    public virtual void Close()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].Close();
        }

        gameObject.SetActive(false);
    }

    public IBattleState GetBattleState()
    {
        return parentState;
    }
}
