using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PermittedPos
{
    GROUNDED_ONLY,
    AIR_OK,
    AIR_ONLY
}

public class Attack : MonoBehaviour
{
    // position
    // simple direction
    // button

    public ButtonType button;
    public PermittedPos permittedPos;

    // public SimpleDirection simpleDir;  (for normal)
    // public Direction dir;  (for command normal)
    // public MotionType motionType; (special)
}

