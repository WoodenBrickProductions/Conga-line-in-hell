using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContextWheel : MonoBehaviour
{
    public enum WheelDirection
    {
        UP,
        RIGHT,
        DOWN,
        LEFT
    }

    public event Action<SelectionContextAction> OnPress;

    public TextMeshProUGUI centerText;
    public TextMeshProUGUI[] texts;
    public Button[] buttons;
    private SelectionContextAction[] boundActions = new SelectionContextAction[4];

    public void SetupContext(SelectionContext context)
    {
        if (context == null || context.actions == null || context.actions.Count == 0)
            return;

        centerText.text = context.name;

        var keys = context.actions.Keys.GetEnumerator();

        int i = 0;
        while(keys.MoveNext())
        {
            if (i >= 4)
            {
                Debug.LogError("Too many actions");
                break;
            }

            SelectionContextAction current = keys.Current;
            buttons[i].gameObject.SetActive(true);
            texts[i].text = Enum.GetName(typeof(SelectionContextAction), current);
            boundActions[i] = current;

            i++;
        }
    }

    public void ClearContext()
    {
        centerText.text = "No action available";
        foreach(var button in buttons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void Up()
    {
        OnPress?.Invoke(boundActions[0]);
    }

    public void Right()
    {
        OnPress?.Invoke(boundActions[1]);
    }

    public void Down()
    {
        OnPress?.Invoke(boundActions[2]);
    }

    public void Left()
    {
        OnPress?.Invoke(boundActions[3]);
    }
}
