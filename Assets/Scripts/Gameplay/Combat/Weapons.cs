using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dice;

public enum WeaponType
{
    MELEE,
    CQB,
    RIFLE,
    LAUNCHER,
    CANNON,
    NEXUS
}

public enum WeaponSize
{
    AUXILIARY,
    MAIN,
    HEAVY,
    SUPER_HEAVY
}

public enum DamageType
{
    KINETIC,
    EXPLOSIVE,
    ENERGY
}

[System.Serializable]
public struct Damage
{
    public int amount;
    public DamageType type;

    public Damage(int _amount, DamageType _type)
    {
        amount = _amount;
        type = _type;
    }
}

[System.Serializable]
public struct DamageData
{
    public DicePool dice;
    public int staticMod;
    public DamageType damageType;

    public Damage Roll()
    {
        return new Damage(staticMod + dice.Roll(), damageType);
    }
}

[System.Serializable]
public class Weapon
{
    public WeaponType type;
    public WeaponSize size;
    public DamageData damage;
    public int range;

    public void Attack(ActingEntity attacker, Entity target)
    {
        target.TakeDamage(attacker, damage.Roll());
    }
}