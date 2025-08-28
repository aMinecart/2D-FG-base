using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private ButtonType button;
    private List<UserButton> userButtons = new List<UserButton>();

    private void OnPunch()
    {

    }

    private void OnKick()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class UserButton
{
    public Button button { get; }
    public int startFrame { get; }

    public UserButton(Button button, int startFrame)
    {
        this.button = button;
        this.startFrame = startFrame;
    }

    public override string ToString()
    {
        return $"{button}; frame {startFrame}";
    }
}