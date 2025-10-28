using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public enum PermittedPos
{
    GROUNDED_ONLY,
    AIR_OK,
    AIR_ONLY
}
*/

public class PlayerAction : ScriptableObject
{
    public string actionCode { get; private set; }
    public FrameData frameData { get; private set; }
    public GameObject hitbox { get; private set; }

    public PlayerAction(string actionCode, FrameData frameData, GameObject hitbox)
    {
        this.actionCode = actionCode;
        this.frameData = frameData;
        this.hitbox = hitbox;
    }

    // position
    // simple direction
    // button

    // public ButtonType button;
    // public PermittedPos permittedPos;

    // public SimpleDirection simpleDir;  (for normal)
    // public Direction dir;  (for command normal)
    // public MotionType motionType; (special)
}

public readonly struct FrameData
{
    public readonly int startup;
    public readonly int active;
    public readonly int recovery;

    public FrameData(int sta, int act, int rec)
    {
        this.startup = sta;
        this.active = act;
        this.recovery = rec;
    }
}