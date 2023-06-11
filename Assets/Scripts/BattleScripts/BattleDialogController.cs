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
                str = $"스킬을 회피하였다!";
                break;
        }
        DialogText.text += str;        
    }
}

    
