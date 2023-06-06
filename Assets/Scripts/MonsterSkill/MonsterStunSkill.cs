using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MonsterStunSkill : MonsterSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        SkillTable.Instance.MonsterStunAttackSkill(monster, player, Name, SkillType);
    }
}
