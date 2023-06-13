using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MonsterAttackSkill : MonsterSkill
{    
    public override void SkillUse(MonsterClass monster)
    {
        monster.startTakeDamaged(this);
    }
}
