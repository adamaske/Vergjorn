using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class CombatLevel : ScriptableObject
{
    public string levelName;

    public float baseAttackDamage;

    public float baseAttackSpeed;

    public float swordBonusAttackDamage;

    public float maxHealth;

    public float helmetBonusHealth;

    public float shieldDamageMidigation;

    public float xpToLevelUp;

}
