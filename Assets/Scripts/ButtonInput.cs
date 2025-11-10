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
    private GameManager gameManager;

    // private List<ButtonRecord> userButtons = new List<UserButton>();

    private List<ButtonType> buttonAddLog = new List<ButtonType>();
    private List<ButtonType> buttonRemoveLog = new List<ButtonType>();

    public List<ButtonType> pressedButtons { get; private set; } = new List<ButtonType>();
    public List<ButtonType> releasedButtons { get; private set; } = new List<ButtonType>();
    public List<ButtonType> heldButtons { get; private set; } = new List<ButtonType>();

    private List<ButtonRecord> buttonRecords = new List<ButtonRecord>();

    // returns true if type is currently being held
    private bool IsButtonActive(ButtonType type)
    {
        return heldButtons.Contains(type);
    }

    // returns true when type was pressed less than tolerance frames ago
    private bool IsButtonRecent(ButtonType type, int tolerance)
    {
        int index = 1;
        while (gameManager.currentFrame - buttonRecords[^index].startFrame < tolerance)
        {
            if (buttonRecords[^index].button == type && buttonRecords[^index].wasPress)
            {
                return true;
            }

            index++;
        }

        return false;

        /*
        if (buttonRecords[^1].startFrame < gameManager.currentFrame - tolerance)
        {
            //print($"{buttonRecords[^1].startFrame}, {gameManager.currentFrame}");
            return false;
        }

        foreach (ButtonType button in buttonRecords[^1].buttons)
        {
            if (button == type)
            {
                return true;
            }
        }

        return false;
        */
    }

    private void printButtons(List<ButtonType> list)
    {
        string buttonTypeNames = "";
        foreach (ButtonType button in list)
        {
            buttonTypeNames += button.ToString() + " ";
        }

        print(buttonTypeNames);
    }

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
        // ActionManager = GetComponent<MotionInput>();
        gameManager = FindAnyObjectByType<GameManager>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        /*
        if (userButtons.Count == 0 || !(userButtons[^1].buttons.Count() == currentButtons.Count && userButtons[^1].buttons.All(currentButtons.Contains)))
        {
            userButtons.Add(new UserButton(currentButtons.ToArray(), gameManager.currentFrame));
        }
        */

        pressedButtons.Clear();
        releasedButtons.Clear();

        foreach (ButtonType add in buttonAddLog)
        {
            pressedButtons.Add(add);
            heldButtons.Add(add);
            buttonRecords.Add(new ButtonRecord(add, true, gameManager.currentFrame));
        }

        buttonAddLog.Clear();

        foreach (ButtonType remove in buttonRemoveLog)
        {
            releasedButtons.Add(remove);
            heldButtons.Remove(remove);
            buttonRecords.Add(new ButtonRecord(remove, false, gameManager.currentFrame));
        }

        buttonRemoveLog.Clear();

        // printButtons(heldButtons);

        // print(buttonRecords.Count);
        // print(IsButtonActive(ButtonType.MEDIUM));
        // print(buttonRecords[^1]);
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