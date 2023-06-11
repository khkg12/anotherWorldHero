using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu]
public class PlayerAttackSkill : BaseSkill
{
    public override void SkillUse(MonsterClass target)
    {
        target.startMonsterDamaged(this);
    }
}
