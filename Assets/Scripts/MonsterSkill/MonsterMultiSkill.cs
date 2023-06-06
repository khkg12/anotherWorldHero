using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

[CreateAssetMenu]
public class MonsterMultiSkill : MonsterSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        SkillTable.Instance.MonsterMultiAttackSkill(monster, player, Name, SkillTimes, SkillType); // ���� �ַο� 2�������̹Ƿ� 2�Ű������־���
    }
}
