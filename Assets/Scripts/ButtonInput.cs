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
    private List<ButtonType> button;
    private List<UserButton> userButtons = new List<UserButton>();

    private void OnPunch(InputValue pValue)
    {
        if (pValue.isPressed)
        {
            button.Add(ButtonType.PUNCH);
        }
        else
        {
            button.Remove(ButtonType.PUNCH);
        }
    }

    private void OnKick()
    {
        //button.Add(ButtonType.KICK);
    }

    private void OnMedium()
    {
        //button.Add(ButtonType.MEDIUM);
    }

    private void OnHeavy()
    {
        //button.Add(ButtonType.HEAVY);
    }

    private void OnWipe()
    {
        //button.Add(ButtonType.WIPE);
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (userButtons.Count == 0 || userButtons[^1].button != button)
        {
            userButtons.Add(new UserButton(button, MotionInput.currentFrame));
        }

        print(userButtons[^1]);
    }
}

public class UserButton
{
    public List<ButtonType> button { get; }
    public int startFrame { get; }

    public UserButton(List<ButtonType> button, int startFrame)
    {
        this.button = button;
        this.startFrame = startFrame;
    }

    public override string ToString()
    {
        return $"{button}; frame {startFrame}";
    }
}