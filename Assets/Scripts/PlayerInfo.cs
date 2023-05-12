using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{    
    public TextMeshProUGUI AtkText;
    public TextMeshProUGUI DefText;
    public TextMeshProUGUI CriText;
    public TextMeshProUGUI DodText;

    public Button FirstSkillBtn;
    public Button SecondSkillBtn;
    public Button ThirdSkillBtn;
    public Button FourthSkillBtn;
    public Button FifthSkillBtn;

    public Image FirstSkillImage;
    public Image SecondSkillImage;
    public Image ThirdSkillImage;
    public Image FourthSkillImage;
    public Image FifthSkillImage;

    public TextMeshProUGUI FirstSkillName;
    public TextMeshProUGUI SecondSkillName;
    public TextMeshProUGUI ThirdSkillName;
    public TextMeshProUGUI FourthSkillName;
    public TextMeshProUGUI FifthSkillName;

    public TextMeshProUGUI SecondSkillAvailableText;
    public TextMeshProUGUI ThirdSkillAvailableText;
    public TextMeshProUGUI FourthSkillAvailableText;
    public TextMeshProUGUI FifthSkillAvailableText;

    // 선택한 스킬창 및 변수
    public Image SelectSkillUI;
    public Image SelectSkillImage;
    public TextMeshProUGUI SelectSkillName;
    public TextMeshProUGUI SelectSkillOption;

    // 특성 레벨 텍스트
    public TextMeshProUGUI IronBodyText;
    public TextMeshProUGUI FightingSpiritText;
    public TextMeshProUGUI WillPowerText;
    public TextMeshProUGUI ScareText;
   

    private void Start()
    {
        FirstSkillBtn.onClick.AddListener(()=>SkillUiSet(FirstSkillImage, FirstSkillName, PlayerTable.Instance.playerSkillList[0].SkillText));
        SecondSkillBtn.onClick.AddListener(() => SkillUiSet(SecondSkillImage, SecondSkillName, PlayerTable.Instance.playerSkillList[1].SkillText));
        ThirdSkillBtn.onClick.AddListener(() => SkillUiSet(ThirdSkillImage, ThirdSkillName, PlayerTable.Instance.playerSkillList[2].SkillText));
        FourthSkillBtn.onClick.AddListener(() => SkillUiSet(FourthSkillImage, FourthSkillName, PlayerTable.Instance.playerSkillList[3].SkillText));
        FifthSkillBtn.onClick.AddListener(() => SkillUiSet(FifthSkillImage, FifthSkillName, PlayerTable.Instance.playerSkillList[4].SkillText));                
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
        
        FirstSkillImage.sprite = PlayerTable.Instance.playerSkillList[0].SkillSprite;
        FirstSkillName.text = PlayerTable.Instance.playerSkillList[0].Name;
        SecondSkillImage.sprite = PlayerTable.Instance.playerSkillList[1].SkillSprite;
        SecondSkillName.text = PlayerTable.Instance.playerSkillList[1].Name;
        SecondSkillAvailableText.text = $"{PlayerTable.Instance.SecondSkillAvailableCount}/{PlayerTable.Instance.playerSkillList[1].AvailableCount}";                        

        if (PlayerTable.Instance.playerSkillList[2].Name != "") 
        {            
            ThirdSkillBtn.interactable = true; // 배틀씬 스킬버튼도 비활성화 OnOFF추가
            ThirdSkillImage.sprite = PlayerTable.Instance.playerSkillList[2].SkillSprite;
            ThirdSkillName.text = PlayerTable.Instance.playerSkillList[2].Name;
            ThirdSkillAvailableText.text = $"{PlayerTable.Instance.ThirdSkillAvailableCount}/{PlayerTable.Instance.playerSkillList[2].AvailableCount}";
        }
        if (PlayerTable.Instance.playerSkillList[3].Name != "")
        {            
            FourthSkillBtn.interactable = true;
            FourthSkillImage.sprite = PlayerTable.Instance.playerSkillList[3].SkillSprite;
            FourthSkillName.text = PlayerTable.Instance.playerSkillList[3].Name;
            FourthSkillAvailableText.text = $"{PlayerTable.Instance.FourthSkillAvailableCount}/{PlayerTable.Instance.playerSkillList[3].AvailableCount}";
        }
        if (PlayerTable.Instance.playerSkillList[3].Name != "")
        {            
            FifthSkillBtn.interactable = true;
            FifthSkillImage.sprite = PlayerTable.Instance.playerSkillList[4].SkillSprite;
            FifthSkillName.text = PlayerTable.Instance.playerSkillList[4].Name;
            FifthSkillAvailableText.text = $"{PlayerTable.Instance.FifthSkillAvailableCount}/{PlayerTable.Instance.playerSkillList[4].AvailableCount}";
        }
    }
}
