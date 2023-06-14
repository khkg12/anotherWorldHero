using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefenseSkill : BaseSkill
{
    public override void SkillUse(Character character)
    {
        character.characterAni.SetTrigger("IsDefense");
        character.DefenseAmount += (int)(SkillPercentage * PlayerTable.Instance.Defense); // defense public으로 접근할지 고민해볼것
    }
}
