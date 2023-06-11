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
        if (scrollbar.size < 1 && PreviousSize != scrollbar.size) // ��ũ�ѹ� ����� ���������� ��, �� ��簡 �߰����� ���� �� ��ũ�� ������ �� �ְ� �����߰�, ��簡 �߰��Ǿ� ��ũ�� ����� ����Ǿ��ٸ� �� �� ��ũ����ġ�� 0����
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
                str = $"{skillData.Name}! {skillData.Damage}��������!";
                break;
            case BattleType.CriAttack:
                str = $"ġ��Ÿ! {skillData.Name}! {skillData.CriDamage}��������!";
                break;
            case BattleType.Dodge:
                str = $"��ų�� ȸ���Ͽ���!";
                break;
        }
        DialogText.text += str;        
    }
}

    
