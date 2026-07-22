using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionState
{
    NEUTRAL,
    STARTUP,
    ACTIVE,
    INACTIVE,
    RECOVERY,
    SPECIAL_RECOVERY
}

public enum EffectType
{
    NONE
}

public class PlayerAction
{
    public string code { get; }
    public PlayerState[] states { get; }

    public int duration
    {
        get
        {
            int sum = 0;
            foreach (PlayerState state in states)
            {
                sum += state.duration;
            }

            return sum;
        }
    }

    public PlayerAction(string code, PlayerState[] states)
    {
        this.code = code;
        this.states = states;
    }

    public bool IsValid()
    {
        foreach (PlayerState state in states)
        {
            if (!state.IsValid())
            {
                return false;
            }
        }

        return true;
    }

    public override string ToString()
    {
        /*
        string asString = $"Player action {code}";

        foreach (PlayerState state in states)
        {
            asString += state.ToString() + "; ";
        }
        */

        return $"Player action {code}";
    }
}

public class PlayerState
{
    public ActionState type { get; }
    public int duration { get; }

    public TimedArea[] hurtboxes { get; }
    public TimedArea[] hitboxes { get; }
    public TimedEffect[] effects { get; }
    public AttackInfo attackInfo { get; }

    public PlayerState(ActionState type,
                       int duration,
                       TimedArea[] hurtboxes,
                       TimedArea[] hitboxes,
                       TimedEffect[] effects,
                       AttackInfo attackInfo)
    {
        this.type = type;
        this.duration = duration;
        this.hurtboxes = hurtboxes;
        this.hitboxes = hitboxes;
        this.effects = effects;
        this.attackInfo = attackInfo;
    }

    public bool IsValid()
    {
        if (hurtboxes != null)
        {
            int hurtboxLength = 0;

            foreach (TimedArea area in hurtboxes)
            {
                hurtboxLength += area.duration;
            }

            if (hurtboxLength != duration)
            {
                return false;
            }
        }

        if (hitboxes != null)
        {
            int hitboxLength = 0;

            foreach (TimedArea area in hitboxes)
            {
                hitboxLength += area.duration;
            }

            if (hitboxLength != duration)
            {
                return false;
            }
        }

        return true;
    }
    
    public override string ToString()
    {
        return $"{type} state lasting {duration} frames";
    }
}

/*
public class PlayerStage
{
    public int duration { get; }
    public BoxInfo[] hurtbox { get; }
    public BoxInfo[] hitbox { get; }
    public TimedEffect[] effects { get; }
    public AttackInfo attackInfo { get; }

    public PlayerStage(int duration, BoxInfo[] hurtbox, BoxInfo[] hitbox, TimedEffect[] effects, AttackInfo attackInfo)
    {
        this.duration = duration;
        this.hurtbox = hurtbox;
        this.hitbox = hitbox;
        this.effects = effects;
        this.attackInfo = attackInfo;
    }
}*/

public struct TimedEffect
{
    public EffectType effect;
    public int duration;

    public TimedEffect(EffectType effect, int duration)
    {
        this.effect = effect;
        this.duration = duration;
    }

    public override string ToString()
    {
        return $"{effect} effect with duration {duration}";
    }
}

public struct TimedArea
{
    public BoxInfo[] subboxes;
    public int duration;

    public TimedArea(BoxInfo[] subboxes, int duration)
    {
        this.subboxes = subboxes;
        this.duration = duration;
    }

    public override string ToString()
    {
        return $"'Composite' area with duration {duration}";
    }
}

public struct AttackInfo
{
    int damage;
    Vector2 knockback;

    public AttackInfo(int damage, Vector2 knockback)
    {
        this.damage = damage;
        this.knockback = knockback;
    }
}

public struct ProjectileInfo
{

}

/*
 * PlayerActionOld -> string id
 *                 PlayerState[] stateList
 * 
 * PlayerState -> int length (maybe?)
 *                ActionState type
 *                PlayerStage[] stages
 * 
 * PlayerStage -> int length
 *                BoxInfo[] hurtbox
 *                BoxInfo[] hitbox
 *                EffectInfo[] selfEffects
 *                AttackInfo attackInfo
 * 
 * AttackInfo -> int damage
 *               Vector2 knockback
 *               int RISC
 *               etc...
 * 
 */