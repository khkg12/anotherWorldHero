using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class BlessingSelect : MonoBehaviour
{
    public Button FirstBtn;
    public Button SecondBtn;
    public Button ThirdBtn;
    public Button RerollBtn;
    public Button ContinueBtn;    

    public List<Blessing> selectBlessingList;
    public Image BlessingGetUI; // 최종적으로 선택한 축복의 구체적 정보창
    public TextMeshProUGUI BlessingGetName;
    public Image BlessingGetImage;
    public TextMeshProUGUI BlessingGetOption;
    public Image NowBlessingLv;
    public Image NextBlessingLv;

    public Image firstBlessingImage;
    public TextMeshProUGUI firstBlessingName;
    public TextMeshProUGUI firstBlessingOption;

    public Image SecondBlessingImage;
    public TextMeshProUGUI SecondBlessingName;
    public TextMeshProUGUI SecondBlessingOption;

    public Image ThirdBlessingImage;
    public TextMeshProUGUI ThirdBlessingName;
    public TextMeshProUGUI ThirdBlessingOption;

    public int selectBlessingNum;
        

    private void Start()
    {        
        FirstBtn.onClick.AddListener(() => BlessingSelectUI(selectBlessingList[0])); // 첫번째 버튼 눌렀을 때 첫번째 아이템 능력치 제공
        FirstBtn.onClick.AddListener(() => selectBlessingNum = 0);
        SecondBtn.onClick.AddListener(() => BlessingSelectUI(selectBlessingList[1])); // delegate로 묶어야할듯 event나
        SecondBtn.onClick.AddListener(() => selectBlessingNum = 1);
        ThirdBtn.onClick.AddListener(() => BlessingSelectUI(selectBlessingList[2]));
        ThirdBtn.onClick.AddListener(() => selectBlessingNum = 2);

        RerollBtn.onClick.AddListener(() => BlessingReroll());
        ContinueBtn.onClick.AddListener(() => DialogManager.Instance.NextPage(UiManager.Instance.BlessingSelectUI, selectBlessingList));
        ContinueBtn.onClick.AddListener(() => BlessingGet(selectBlessingList[selectBlessingNum]));
    }

    public void OnEnable()
    {
        selectBlessingList = BlessingTable.Instance.BlessingList;
        selectBlessingList = selectBlessingList.OrderBy(i => Random.value).ToList(); // 리스트의 값 랜덤으로 정렬

        BlessingUISet(firstBlessingImage, firstBlessingName, firstBlessingOption, selectBlessingList[0]); // 첫번째 축복 ui 셋팅
        BlessingUISet(SecondBlessingImage, SecondBlessingName, SecondBlessingOption, selectBlessingList[1]);
        BlessingUISet(ThirdBlessingImage, ThirdBlessingName, ThirdBlessingOption, selectBlessingList[2]);
    }

    public void BlessingSelectUI(Blessing selectBlessing) 
    {
        BlessingGetOption.text = ""; // 축복 옵션 텍스트UI 초기화
        BlessingGetUI.gameObject.SetActive(true);       
        BlessingGetImage.sprite = selectBlessing.BlessingSprite;
        BlessingGetName.text = selectBlessing.BlessingName;        

        if (selectBlessing.maxHp > 0)
        {
            BlessingGetOption.text += $"<size=7>최대체력 : <color=#369341>{PlayerTable.Instance.MaxHp} -> {PlayerTable.Instance.MaxHp + selectBlessing.maxHp}</color></size>" + "\n\n";
        }        
        if (selectBlessing.Atk > 0)
        {
            BlessingGetOption.text += $"<size=7>공격력 : <color=#369341>{PlayerTable.Instance.Atk} -> {PlayerTable.Instance.Atk + selectBlessing.Atk}</color></size>" + "\n\n";
        }
        if (selectBlessing.Def > 0)
        {
            BlessingGetOption.text += $"<size=7>방어력 : <color=#369341>{PlayerTable.Instance.Defense} -> {PlayerTable.Instance.Defense + selectBlessing.Def} </color></size>" + "\n\n";
        }
        if (selectBlessing.Cri > 0)
        {
            BlessingGetOption.text += $"<size=7>치명타 : <color=#369341>{PlayerTable.Instance.Critical} -> {PlayerTable.Instance.Critical + selectBlessing.Cri}</color></size>" + "\n\n";
        }
        if (selectBlessing.Dod > 0)
        {
            BlessingGetOption.text += $"<size=7>회피율 : <color=#369341>{PlayerTable.Instance.Dodge} ->  {PlayerTable.Instance.Dodge + selectBlessing.Dod}</color></size>" + "\n\n";
        }

        
        if (selectBlessing.IronBodyPt > 0)
        {
            BlessingUIStatus(PlayerTable.Instance.ironBodyText , PlayerTable.Instance.IronBody, selectBlessing.IronBodyPt);
        }
        if (selectBlessing.ScarePt > 0)
        {
            BlessingUIStatus(PlayerTable.Instance.scareText, PlayerTable.Instance.Scare, selectBlessing.ScarePt);
        }
        if (selectBlessing.WillPowerPt > 0)
        {
            BlessingUIStatus(PlayerTable.Instance.willPowerText, PlayerTable.Instance.WillPower, selectBlessing.WillPowerPt);
        }
        if (selectBlessing.FightingSpiritPt > 0)
        {
            BlessingUIStatus(PlayerTable.Instance.fightingSpiritText, PlayerTable.Instance.FightingSpirit, selectBlessing.FightingSpiritPt);
        }
    }

    public void BlessingUIStatus(StatusText statusText, int PlayerStatus, int BlessingStatus)
    {        
        if (PlayerStatus == 0) // 최초로 획득하는 경우
        {
            BlessingGetOption.text += $"<size=7>신규특성 : {statusText.StatusName} (Lv.{BlessingStatus})</size>" + "\n\n";
            BlessingGetOption.text += statusText.OptionText + "\n\n";            
            NextBlessingLv.gameObject.SetActive(true);            
            NextBlessingLv.GetComponentInChildren<TextMeshProUGUI>().text = $"Lv {PlayerStatus + BlessingStatus} : " + statusText.LevelText[PlayerStatus + BlessingStatus - 1] + "\n";

        }
        else if(PlayerStatus == 5) // 최고레벨일 경우
        {
            BlessingGetOption.text += $"{statusText.StatusName} (Lv.5)" + "\n";
            BlessingGetOption.text += "이미 최고 레벨에 달성한 특성입니다." + "\n";
        }
        else
        {
            BlessingGetOption.text += $"<size=7>{statusText.StatusName} (Lv.{PlayerStatus}) ->  (Lv.{PlayerStatus + BlessingStatus})</size>" + "\n";
            BlessingGetOption.text += statusText.OptionText + "\n";
            NowBlessingLv.gameObject.SetActive(true);
            NextBlessingLv.gameObject.SetActive(true);
            if (PlayerStatus + BlessingStatus >= 5)
            {                
                NowBlessingLv.GetComponentInChildren<TextMeshProUGUI>().text = $"Lv.{PlayerStatus} : " + statusText.LevelText[PlayerStatus - 1] + "\n";
                NextBlessingLv.GetComponentInChildren<TextMeshProUGUI>().text = $"Lv 5 : " + statusText.LevelText[4] + "\n";                
            }
            else
            {
                NowBlessingLv.GetComponentInChildren<TextMeshProUGUI>().text = $"Lv.{PlayerStatus} : " + statusText.LevelText[PlayerStatus - 1] + "\n";
                NextBlessingLv.GetComponentInChildren<TextMeshProUGUI>().text = $"Lv {PlayerStatus + BlessingStatus} : " + statusText.LevelText[PlayerStatus + BlessingStatus - 1] + "\n";                
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(BlessingGetUI.rectTransform); // 레이아웃 강제 재정렬 시켜주는 코드, content size filter가 즉각적으로 렌더링이 안되기때문에 넣어주는 코드
    }

    public void BlessingUISet(Image BlessingImage, TextMeshProUGUI BlessingName, TextMeshProUGUI BlessingOption, Blessing selectBlessing) // 축복 ui 셋팅 함수
    {
        BlessingOption.text = "";
        BlessingImage.sprite = selectBlessing.BlessingSprite;
        BlessingName.text = selectBlessing.BlessingName;
        for (int i = 0; i < selectBlessing.Option.Length; i++)
        {
            BlessingOption.text += selectBlessing.Option[i] + "\n";
        }
    }

    public void BlessingGet(Blessing selectBlessing) // 축복 버튼 클릭 시 호출되는 함수 능력치 강화 
    {
        BlessingGetOption.text = ""; // 축복 옵션 텍스트UI 초기화
        BlessingGetUI.gameObject.SetActive(true);

        PlayerTable.Instance.Scare += selectBlessing.ScarePt;
        PlayerTable.Instance.WillPower += selectBlessing.WillPowerPt;
        PlayerTable.Instance.FightingSpirit += selectBlessing.FightingSpiritPt;
        PlayerTable.Instance.IronBody += selectBlessing.IronBodyPt;

        PlayerTable.Instance.MaxHp += selectBlessing.maxHp;
        PlayerTable.Instance.Hp += selectBlessing.maxHp;  // 잔존hp도 증가하는 최대체력만큼 상승
        PlayerTable.Instance.Atk += selectBlessing.Atk;
        PlayerTable.Instance.Defense += selectBlessing.Def;
        PlayerTable.Instance.Critical += selectBlessing.Cri;
        PlayerTable.Instance.Dodge += selectBlessing.Dod;        
    }    

    public void BlessingReroll()
    {
        firstBlessingOption.text = "";
        SecondBlessingOption.text = "";
        ThirdBlessingOption.text = "";
        selectBlessingList = selectBlessingList.OrderBy(i => Random.value).ToList();
        BlessingUISet(firstBlessingImage, firstBlessingName, firstBlessingOption, selectBlessingList[0]);
        BlessingUISet(SecondBlessingImage, SecondBlessingName, SecondBlessingOption, selectBlessingList[1]);
        BlessingUISet(ThirdBlessingImage, ThirdBlessingName, ThirdBlessingOption, selectBlessingList[2]);
    }
}
