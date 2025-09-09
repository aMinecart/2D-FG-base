using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private List<ButtonRecord> userButtons = new List<ButtonRecord>();

    private List<ButtonType> buttonAddLog = new List<ButtonType>();
    private List<ButtonType> buttonRemoveLog = new List<ButtonType>();

    private List<ButtonType> activeButtons = new List<ButtonType>();
    private List<ButtonType> currentButtons = new List<ButtonType>();
    


    /*
    private bool IsButtonActive(ButtonType type)
    {
        foreach (ButtonType button in userButtons[^1].currentButtons)
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
            currentButtons.Add(ButtonType.PUNCH);
        }
        else
        {
            currentButtons.Remove(ButtonType.PUNCH);
        }
    }

    private void OnKick(InputValue kValue)
    {
        if (kValue.isPressed)
        {
            currentButtons.Add(ButtonType.KICK);
        }
        else
        {
            currentButtons.Remove(ButtonType.KICK);
        }
    }

    private void OnMedium(InputValue mValue)
    {
        if (mValue.isPressed)
        {
            currentButtons.Add(ButtonType.MEDIUM);
        }
        else
        {
            currentButtons.Remove(ButtonType.MEDIUM);
        }
    }

    private void OnHeavy(InputValue hValue)
    {
        if (hValue.isPressed)
        {
            currentButtons.Add(ButtonType.HEAVY);
        }
        else
        {
            currentButtons.Remove(ButtonType.HEAVY);
        }
    }

    private void OnWipe(InputValue wValue)
    {
        if (wValue.isPressed)
        {
            currentButtons.Add(ButtonType.WIPE);
        }
        else
        {
            currentButtons.Remove(ButtonType.WIPE);
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
        if (userButtons.Count == 0 || !(userButtons[^1].buttons.Count() == currentButtons.Count && userButtons[^1].buttons.All(currentButtons.Contains)))
        {
            userButtons.Add(new ButtonRecord(currentButtons.ToArray(), motionTracker.currentFrame));
        }

        //print(userButtons.Count);
        print(IsButtonActive(ButtonType.MEDIUM));
        //print(userButtons[^1]);
    }
}

public class ButtonRecord
{
    public ButtonType[] buttons { get; }
    public int startFrame { get; }

    public ButtonRecord(ButtonType[] buttons, int startFrame)
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