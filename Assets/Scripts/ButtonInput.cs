using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ButtonType
{
    NONE,
    DASH,
    PUNCH,
    KICK,
    MEDIUM,
    HEAVY,
    WIPE,
    BURST,
    SPEC_CANCEL
}

public class ButtonInput : MonoBehaviour
{
    [SerializeField] private MotionInput motionTracker;
    private List<ButtonType> buttons = new List<ButtonType>();
    private List<UserButton> userButtons = new List<UserButton>();

    /*
    private bool IsButtonActive(ButtonType type)
    {
        foreach (ButtonType button in userButtons[^1].buttons)
        {
            if (button == type)
            {
                return true;
            }
        }

        return false;
    }
    */

    private bool IsButtonActive(ButtonType type, int tolerance = 0)
    {
        if (userButtons[^1].startFrame < motionTracker.currentFrame - tolerance)
        {
            //print($"{userButtons[^1].startFrame}, {motionTracker.currentFrame}");
            return false;
        }

        foreach (ButtonType button in userButtons[^1].buttons)
        {
            if (button == type)
            {
                return true;
            }
        }

        return false;
    }

    private void OnPunch(InputValue pValue)
    {
        if (pValue.isPressed)
        {
            buttons.Add(ButtonType.PUNCH);
        }
        else
        {
            buttons.Remove(ButtonType.PUNCH);
        }
    }

    private void OnKick(InputValue kValue)
    {
        if (kValue.isPressed)
        {
            buttons.Add(ButtonType.KICK);
        }
        else
        {
            buttons.Remove(ButtonType.KICK);
        }
    }

    private void OnMedium(InputValue mValue)
    {
        if (mValue.isPressed)
        {
            buttons.Add(ButtonType.MEDIUM);
        }
        else
        {
            buttons.Remove(ButtonType.MEDIUM);
        }
    }

    private void OnHeavy(InputValue hValue)
    {
        if (hValue.isPressed)
        {
            buttons.Add(ButtonType.HEAVY);
        }
        else
        {
            buttons.Remove(ButtonType.HEAVY);
        }
    }

    private void OnWipe(InputValue wValue)
    {
        if (wValue.isPressed)
        {
            buttons.Add(ButtonType.WIPE);
        }
        else
        {
            buttons.Remove(ButtonType.WIPE);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        motionTracker = GetComponent<MotionInput>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (userButtons.Count == 0 || userButtons[^1].buttons != buttons.ToArray())
        {
            userButtons.Add(new UserButton(buttons.ToArray(), motionTracker.currentFrame));
        }

        //print(userButtons.Count);
        //print(IsButtonActive(ButtonType.MEDIUM));
        print(userButtons[^1]);
    }
}

public class UserButton
{
    public ButtonType[] buttons { get; }
    public int startFrame { get; }

    public UserButton(ButtonType[] buttons, int startFrame)
    {
        this.buttons = buttons;
        this.startFrame = startFrame;
    }

    public override string ToString()
    {
        string types = "";
        foreach (ButtonType type in buttons)
        {
            types += type + " ";
        }

        return $"{types}; frame {startFrame}";
    }
}