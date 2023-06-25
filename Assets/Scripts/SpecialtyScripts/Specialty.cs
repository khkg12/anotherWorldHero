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
public class Specialty : ScriptableObject // ����Ư���� ���
{
    // ����Ư��, ���Ư�� ����, ��������, �������    
    // �����ۿ� ������ȣ�� �ְ� Ư������ ������ȣ�� ���� �������� �������� ������ �ִ� ������ȣ�� ��� Ư���� ������ȣ�� �޾Ƽ� �� Ư���� �����������ִ� ����
    // �׷����� ĳ���ʹ� ��� Ư���� �ϴ� ������ �־���� ���� 0����
    // ex a : 1000 A : 1 a��� �������� ������ �ִ� ������ȣ�� 1000, A��� Ư���� ������ �ִ� ������ȣ�� 1, a�� ����� ��� ������ȣ 1000�� ���� 1�̶�� ����
    // ��ȣ�� ���� Ư�� A�� �������� �÷���
    public string specialtyName;
    public int specialtyId;
    public int specialtyLevel; // �ִ뷹�� ����            
    public int[] specialtyAmount;
    public SpeicaltyType speicaltyType;

    // ����ȸ���Ʈ�� ���鼭 id���� ã�Ƽ� �ش��ϴ� Ư���� ã�� �� Ư���� ������ �ø���? 
    public void StartSpeicalty(Character character, MonsterClass monster)
    {
        switch (speicaltyType)
        {
            case SpeicaltyType.Scare:
                monster.nowMonsterAtk -= specialtyAmount[specialtyLevel]; // specialtyLevel�� ����, playertable.instance.scare�� ����
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
