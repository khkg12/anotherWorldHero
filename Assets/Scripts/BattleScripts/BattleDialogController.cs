using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public enum BattleType
{
    Attack,
    CriAttack,    
    DotAttack,
    GetShield,
    GetBuff,
    Dodge,
    GetStun,
    EndureStun,
}

public class BattleDialogController : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollbar;
    private float PreviousSize;

    [SerializeField] private TextMeshProUGUI DialogText;

    private void Update()
    {
        if (scrollbar.size < 1 && PreviousSize != scrollbar.size) // 스크롤바 사이즈가 변하지않을 때, 즉 대사가 추가되지 않을 땐 스크롤 움직일 수 있게 조건추가, 대사가 추가되어 스크롤 사이즈가 변경되었다면 그 때 스크롤위치를 0으로
        {
            scrollbar.value = 0;
            PreviousSize = scrollbar.size;
        }
    }

    public void PlayerAddText(BattleType type, SkillData skillData)
    {        
        string str = string.Empty;
        switch (type)
        {
            case BattleType.Attack:                
                str = $"{skillData.Name}! {skillData.Damage}피해입힘!";
                break;
            case BattleType.CriAttack:
                str = $"치명타! {skillData.Name}! {skillData.CriDamage}피해입힘!";
                break;
            case BattleType.Dodge:
                str = $"적은 공격을 회피했다!";
                break;
            case BattleType.GetStun:
                str = $"적을 기절시켰다! 적은 행동할 수 없다!";
                break;
            case BattleType.EndureStun:
                str = $"적은 기절을 견디었다!";
                break;
        }
        DialogText.text += str+"\n"; // 여기서 효과줄것, 한글자씩 출력되는 함수설정 후 넣던지                
    }

    public void MonsterAddText(BattleType type, SkillData skillData)
    {        
        string str = string.Empty;
        switch (type)
        {
            case BattleType.Attack:
                str = $"적의 {skillData.Name}! {skillData.Damage}피해입음! ({skillData.Defense}방어)";
                break;
            case BattleType.CriAttack:
                str = $"치명타! 적의 {skillData.Name}! {skillData.CriDamage}피해입음! ({skillData.CriDenfense}방어)";
                break;
            case BattleType.Dodge:
                str = $"적의 공격을 회피하였다!";
                break;
            case BattleType.GetStun:
                str = $"적의 공격에 기절하였다! 행동할 수 없다!";
                break;
            case BattleType.EndureStun:
                str = $"불굴의 의지로 기절을 견디었다!";
                break;
        }
        DialogText.text += str+"\n"; // 여기서 효과줄것, 한글자씩 출력되는 함수설정 후 넣던지                
    }

    public void ActionAddText(MonsterSkill monsterSkill)
    {
        DialogText.text += monsterSkill.SkillText;
        DialogText.text += " 무엇을 할까?\n";
        
    }

    public void LineAddText()
    {
        DialogText.text += "\n";
    }
}

    
