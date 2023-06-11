using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

[CreateAssetMenu]
public class MonsterMultiSkill : MonsterSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        SkillTable.Instance.MonsterMultiAttackSkill(monster, player, Name, SkillTimes, SkillType); // 더블 애로우 2번공격이므로 2매개변수넣어줌
    }
}
