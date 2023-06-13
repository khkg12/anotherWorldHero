using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI AtkText;
    [SerializeField] private TextMeshProUGUI DefText;
    [SerializeField] private TextMeshProUGUI CriText;
    [SerializeField] private TextMeshProUGUI DodText;

    [SerializeField] private List<Button> SkillBtnList;
    [SerializeField] private List<Image> SkillImageList;    
    [SerializeField] private List<TextMeshProUGUI> SkillNameList;    
    [SerializeField] private List<TextMeshProUGUI> SkillCountTextList;

    // 선택한 스킬창 및 변수
    [SerializeField] private Image SelectSkillUI;
    [SerializeField] private Image SelectSkillImage;
    [SerializeField] private TextMeshProUGUI SelectSkillName;
    [SerializeField] private TextMeshProUGUI SelectSkillOption;

    // 특성 레벨 텍스트
    [SerializeField] private TextMeshProUGUI IronBodyText;
    [SerializeField] private TextMeshProUGUI FightingSpiritText;
    [SerializeField] private TextMeshProUGUI WillPowerText;
    [SerializeField] private TextMeshProUGUI ScareText;
   
    private void Start()
    {
        SkillBtnList[0].onClick.AddListener(() => SkillUiSet(SkillImageList[0], SkillNameList[0], PlayerTable.Instance.playerSkillList[0].SkillText));
        SkillBtnList[1].onClick.AddListener(() => SkillUiSet(SkillImageList[1], SkillNameList[1], PlayerTable.Instance.playerSkillList[1].SkillText));
        SkillBtnList[2].onClick.AddListener(() => SkillUiSet(SkillImageList[2], SkillNameList[2], PlayerTable.Instance.playerSkillList[2].SkillText));
        SkillBtnList[3].onClick.AddListener(() => SkillUiSet(SkillImageList[3], SkillNameList[3], PlayerTable.Instance.playerSkillList[3].SkillText));
        SkillBtnList[4].onClick.AddListener(() => SkillUiSet(SkillImageList[4], SkillNameList[4], PlayerTable.Instance.playerSkillList[4].SkillText));
    }

    public void SkillUiSet(Image skillImage, TextMeshProUGUI skillName, string[] skillOption)
    {
        SelectSkillImage.sprite = skillImage.sprite;        
        SelectSkillName.text = skillName.text;
        SelectSkillOption.text = "";
        for (int i = 0; i < skillOption.Length; i++)
        {
            SelectSkillOption.text += skillOption[i] + "\n";
        }
        SelectSkillUI.gameObject.SetActive(true);
    }
    
    public void OnEnable()
    {
        IronBodyText.text = $"철통\nLv. {PlayerTable.Instance.IronBody}";
        FightingSpiritText.text = $"투지\nLv. {PlayerTable.Instance.FightingSpirit}";
        WillPowerText.text = $"의지\nLv. {PlayerTable.Instance.WillPower}";
        ScareText.text = $"공포\nLv. {PlayerTable.Instance.Scare}";

        AtkText.text = $"{PlayerTable.Instance.Atk}";
        DefText.text = $"{PlayerTable.Instance.Defense}";
        CriText.text = $"{PlayerTable.Instance.Critical}%";
        DodText.text = $"{PlayerTable.Instance.Dodge}%";                        

        for (int i = 0; i < PlayerTable.Instance.playerSkillList.Count; i++) //캐릭터 정보창의 스킬TEXT UI
        {
            SkillBtnList[i].interactable = true;
            SkillImageList[i].sprite = PlayerTable.Instance.playerSkillList[i].SkillSprite;
            SkillNameList[i].text = PlayerTable.Instance.playerSkillList[i].Name;
            if (i == 0) continue; // 0일 땐 스킬사용제한횟수가 무제한인 공격스킬이므로 넘김
            SkillCountTextList[i].text = $"{PlayerTable.Instance.SkillAvailableCount[i]}/{PlayerTable.Instance.SkillFixedCount[i]}";
        }
    }
}
