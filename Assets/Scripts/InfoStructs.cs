using UnityEngine;

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

    public override string ToString()
    {
        return $"X Offset: {push}, Y Offset: {raise}, Length: {length}, Width: {width}";
    }
}

public readonly struct Hurtbox
{
    public BoxInfo[] subBoxes { get; }

    // additional hurt info

    public Hurtbox(BoxInfo[] subBoxes)
    {
        this.subBoxes = subBoxes;
    }
}

public readonly struct Hitbox
{
    public BoxInfo[] subBoxes { get; }
    public int baseDamage { get; }
    public Vector2 baseKnockback { get; }

    // array of effect types and their duration on hit

    // other properties to be applied on hit (e.g. RISC)

    public Hitbox(BoxInfo[] subBoxes, int baseDamage, Vector2 baseKnockback)
    {
        this.subBoxes = subBoxes;
        this.baseDamage = baseDamage;
        this.baseKnockback = baseKnockback;
    }
}

public readonly struct MoveInfo
{
    public Vector2 directAccel { get; }

    // public float speedModifier { get; }

    // other properties to apply to player's position, speed, etc. when they use a move

    public MoveInfo(Vector2 directAccel)
    {
        this.directAccel = directAccel;
    }
}