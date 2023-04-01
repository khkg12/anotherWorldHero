using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacteristicInfo : MonoBehaviour
{
    // 선택한 특성창 및 변수    
    public Image SelectCharacteristicUI;
    public TextMeshProUGUI CharacName;
    public TextMeshProUGUI CharacOptionText;
    public TextMeshProUGUI FirstLevelText;
    public TextMeshProUGUI SecondLevelText;
    public TextMeshProUGUI ThirdLevelText;
    public TextMeshProUGUI FourthLevelText;
    public TextMeshProUGUI FifthLevelText;
    public Image FirstLevelBlock;
    public Image SecondLevelBlock;
    public Image ThirdLevelBlock;
    public Image FourthLevelBlock;
    public Image FifthLevelBlock;

    // 특성 버튼
    public Button ScareBtn;
    public Button IronBodyBtn;
    public Button WillPowerBtn;
    public Button FightingSpiritBtn;
    // public Button ScareBtn; 특성추가할것

    void Start()
    {
        var scare = PlayerTable.Instance.scareText;
        var ironBody = PlayerTable.Instance.ironBodyText;
        var willPower = PlayerTable.Instance.willPowerText;
        var fightingSpirit = PlayerTable.Instance.fightingSpiritText;        
        
        ScareBtn.onClick.AddListener(() => SelectCharacUI("공포", PlayerTable.Instance.Scare, scare.OptionText, scare.LevelText));
        IronBodyBtn.onClick.AddListener(() => SelectCharacUI("철통", PlayerTable.Instance.IronBody, ironBody.OptionText, ironBody.LevelText));
        WillPowerBtn.onClick.AddListener(() => SelectCharacUI("의지", PlayerTable.Instance.WillPower, willPower.OptionText, willPower.LevelText));
        FightingSpiritBtn.onClick.AddListener(() => SelectCharacUI("투지", PlayerTable.Instance.FightingSpirit, fightingSpirit.OptionText, fightingSpirit.LevelText));
    }

    public void SelectCharacUI(string Name, int Level, string OptionText, string[] LevelText)
    {
        CharacName.text = $"{Name}\n(Lv. {Level})";
        CharacOptionText.text = OptionText;
        FirstLevelText.text = "<color=white>Lv. 1</color> : " + LevelText[0];
        SecondLevelText.text = "<color=yellow>Lv. 2</color> : " + LevelText[1];
        ThirdLevelText.text = "<color=yellow>Lv. 3</color> : " + LevelText[2];
        FourthLevelText.text = "<color=green>Lv. 4</color> : " + LevelText[3];
        FifthLevelText.text = "<color=blue>Lv. 5</color> : " + LevelText[4];

        switch (Level)
        {
            case 0:
                FirstLevelBlock.color = Color.gray;
                SecondLevelBlock.color = Color.gray;
                ThirdLevelBlock.color = Color.gray;
                FourthLevelBlock.color = Color.gray;
                FifthLevelBlock.color = Color.gray;
                break;
            case 1:
                FirstLevelBlock.color = Color.black;
                SecondLevelBlock.color = Color.gray;
                ThirdLevelBlock.color = Color.gray;
                FourthLevelBlock.color = Color.gray;
                FifthLevelBlock.color = Color.gray;
                break;
            case 2:
                FirstLevelBlock.color = Color.gray;
                SecondLevelBlock.color = Color.black;
                ThirdLevelBlock.color = Color.gray;
                FourthLevelBlock.color = Color.gray;
                FifthLevelBlock.color = Color.gray;
                break;
            case 3:
                FirstLevelBlock.color = Color.gray;
                SecondLevelBlock.color = Color.gray;
                ThirdLevelBlock.color = Color.black;
                FourthLevelBlock.color = Color.gray;
                FifthLevelBlock.color = Color.gray;
                break;
            case 4:
                FirstLevelBlock.color = Color.gray;
                SecondLevelBlock.color = Color.gray;
                ThirdLevelBlock.color = Color.gray;
                FourthLevelBlock.color = Color.black;
                FifthLevelBlock.color = Color.gray;
                break;
            case 5:
                FirstLevelBlock.color = Color.gray;
                SecondLevelBlock.color = Color.gray;
                ThirdLevelBlock.color = Color.gray;
                FourthLevelBlock.color = Color.gray;
                FifthLevelBlock.color = Color.black;
                break;
        }

        SelectCharacteristicUI.gameObject.SetActive(true);        
    }
}

