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

public class PlayerAction
{
    public string actionCode { get; private set; }
    public FrameData frameData { get; private set; }
    public BoxInfo[] hitboxes { get; private set; }

    public PlayerAction(string actionCode, FrameData frameData, BoxInfo[] hitboxes)
    {
        this.actionCode = actionCode;
        this.frameData = frameData;
        this.hitboxes = hitboxes;
    }

    // position
    // simple direction
    // button

    // public ButtonType button;
    // public PermittedPos permittedPos;

    // public SimpleDirection simpleDir;  (for normal)
    // public Direction dir;  (for command normal)
    // public MotionType motionType; (special)

    public override string ToString()
    {
        return $"Action with code {actionCode}, {frameData.startup} frame startup, {frameData.active} frames active, and {frameData.recovery} frame recovery";
    }
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

public readonly struct BoxInfo
{
    public readonly int push;
    public readonly int raise;

    public readonly int length;
    public readonly int width;

    public BoxInfo(int push, int raise, int length, int width)
    {
        this.push = push;
        this.raise = raise;
        this.length = length;
        this.width = width;
    }
}