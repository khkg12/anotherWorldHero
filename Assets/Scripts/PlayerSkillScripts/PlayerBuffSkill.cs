using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffSkill : BaseSkill
{
    public override void SkillUse(Character character)
    {
        character.TakeBuff();
    }
}
