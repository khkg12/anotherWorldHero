using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerDotSkill : BaseSkill
{    
    public Buff buff;
    public override void SkillUse(Character character)
    {
        buff = new Buff()
        {            
            
        };
        character.TakeBuff(buff);        
    }
}



