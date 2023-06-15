using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerDefenseSkill : BaseSkill
{
    public override void SkillUse(Character character)
    {
        character.TakeDefense(this);
    }
}
