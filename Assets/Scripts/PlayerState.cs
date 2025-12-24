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
}

public class PlayerState
{
    public ActionState type { get; }
    public PlayerStage[] stages { get; }

    public int duration
    {
        get
        {
            int sum = 0;
            foreach (PlayerStage stage in stages)
            {
                sum += stage.duration;
            }

            return sum;
        }
    }

    public PlayerState(ActionState type, PlayerStage[] stages)
    {
        this.type = type;
        this.stages = stages;
    }
}

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
}

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