using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu]
public class PlayerTurnFreeAttackSkill : BaseSkill
{    
    public override void SkillUse(Character character)
    {
        character.TurnFree = true;        
        character.startTakeDamaged(this);
    }    
}
