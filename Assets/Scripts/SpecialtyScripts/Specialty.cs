using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpeicaltyType
{
    Scare,
    WillPower,
    IronBody,
    FightingSpirit,
}

[CreateAssetMenu]
public class Specialty : ScriptableObject // 개전특성일 경우
{
    // 개전특성, 상시특성 구분, 레벨구조, 실행로직    
    // 아이템에 고유번호를 넣고 특성에도 고유번호를 넣음 아이템을 겟했을때 가지고 있는 고유번호를 들고 특성의 고유번호를 받아서 그 특성을 레벨업시켜주는 구조
    // 그럴려면 캐릭터는 모든 특성을 일단 가지고 있어야함 레벨 0부터
    // ex a : 1000 A : 1 a라는 아이템이 가지고 있는 고유번호는 1000, A라는 특성이 가지고 있는 고유번호는 1, a를 얻었을 경우 고유번호 1000을 보고 1이라는 고유
    // 번호를 가진 특성 A의 레벨값을 올려줌
    public string specialtyName;
    public int specialtyId;
    public int specialtyLevel; // 최대레벨 제한            
    public int[] specialtyAmount;
    public SpeicaltyType speicaltyType;

    // 스페셜리스트를 돌면서 id값을 찾아서 해당하는 특성을 찾고 그 특성의 레벨을 올린다? 
    public void StartSpeicalty(Character character, MonsterClass monster)
    {
        switch (speicaltyType)
        {
            case SpeicaltyType.Scare:
                monster.nowMonsterAtk -= specialtyAmount[specialtyLevel]; // specialtyLevel로 할지, playertable.instance.scare로 할지
                break;
            case SpeicaltyType.WillPower:                
                character.StunStack -= specialtyAmount[specialtyLevel];
                break;            
        }
    }      

    public void AlwaysSpecialty(Character character, MonsterClass monster)
    {
        switch (speicaltyType)
        {
            case SpeicaltyType.IronBody:
                character.DefenseAmount += specialtyAmount[specialtyLevel];
                break;
            case SpeicaltyType.FightingSpirit:
                if(bm.Instance.BattleRound % 3 == 0)
                {
                    character.atk += specialtyAmount[specialtyLevel];
                }                
                break;            
        }
    }
}
