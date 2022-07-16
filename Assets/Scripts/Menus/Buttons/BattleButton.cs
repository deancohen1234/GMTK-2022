using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleButton : MonoBehaviour
{
    protected BattleMenu parentMenu;

    public virtual void Initialize(BattleMenu _parentMenu)
    {
        parentMenu = _parentMenu;

        Button button = gameObject.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
    }

    public virtual void Close()
    {
        Button button = gameObject.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveListener(OnClick);
        }
    }

    public virtual void OnClick()
    {
        
    }
}
