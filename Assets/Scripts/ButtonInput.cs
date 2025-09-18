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

    private List<ButtonType> newButtons = new List<ButtonType>();
    private List<ButtonType> activeButtons = new List<ButtonType>();
    private List<ButtonRecord> buttonRecords = new List<ButtonRecord>();



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
    */

    private void OnPunch(InputValue pValue)
    {
        if (pValue.isPressed)
        {
            buttonAddLog.Add(ButtonType.PUNCH);
        }
        else
        {
            buttonRemoveLog.Add(ButtonType.PUNCH);
        }
    }

    private void OnKick(InputValue kValue)
    {
        if (kValue.isPressed)
        {
            buttonAddLog.Add(ButtonType.KICK);
        }
        else
        {
            buttonRemoveLog.Add(ButtonType.KICK);
        }
    }

    private void OnMedium(InputValue mValue)
    {
        if (mValue.isPressed)
        {
            buttonAddLog.Add(ButtonType.MEDIUM);
        }
        else
        {
            buttonRemoveLog.Add(ButtonType.MEDIUM);
        }
    }

    private void OnHeavy(InputValue hValue)
    {
        if (hValue.isPressed)
        {
            buttonAddLog.Add(ButtonType.HEAVY);
        }
        else
        {
            buttonRemoveLog.Add(ButtonType.HEAVY);
        }
    }

    private void OnWipe(InputValue wValue)
    {
        if (wValue.isPressed)
        {
            buttonAddLog.Add(ButtonType.WIPE);
        }
        else
        {
            buttonRemoveLog.Add(ButtonType.WIPE);
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
        /*
        if (userButtons.Count == 0 || !(userButtons[^1].buttons.Count() == currentButtons.Count && userButtons[^1].buttons.All(currentButtons.Contains)))
        {
            userButtons.Add(new ButtonRecord(currentButtons.ToArray(), motionTracker.currentFrame));
        }
        */

        newButtons.Clear();

        foreach (ButtonType add in buttonAddLog)
        {
            newButtons.Add(add);
            activeButtons.Add(add);
            buttonRecords.Add(new ButtonRecord(add, true, motionTracker.currentFrame));
        }

        buttonAddLog.Clear();

        foreach (ButtonType remove in buttonRemoveLog)
        {
            activeButtons.Remove(remove);
            buttonRecords.Add(new ButtonRecord(remove, false, motionTracker.currentFrame));
        }

        buttonRemoveLog.Clear();

        // print(userButtons.Count);
        // print(IsButtonActive(ButtonType.MEDIUM));
        // print(userButtons[^1]);
    }
}

public class ButtonRecord
{
    public ButtonType button { get; }
    public bool wasPress { get; }
    public int startFrame { get; }

    public ButtonRecord(ButtonType button, bool wasPress, int startFrame)
    {
        this.button = button;
        this.wasPress = wasPress;
        this.startFrame = startFrame;
    }

    public override string ToString()
    {
        return $"{button} was {(wasPress ? "pressed" : "released")}; frame {startFrame}";
    }
}