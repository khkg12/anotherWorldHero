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

    public Image firstBlessingImage;
    public TextMeshProUGUI firstBlessingName;
    public TextMeshProUGUI firstBlessingOption;

    public Image SecondBlessingImage;
    public TextMeshProUGUI SecondBlessingName;
    public TextMeshProUGUI SecondBlessingOption;

    public Image ThirdBlessingImage;
    public TextMeshProUGUI ThirdBlessingName;
    public TextMeshProUGUI ThirdBlessingOption;


    private void Start()
    {        
        FirstBtn.onClick.AddListener(() => BlessingGet(selectBlessingList[0])); // 첫번째 버튼 눌렀을 때 첫번째 아이템 능력치 제공
        SecondBtn.onClick.AddListener(() => BlessingGet(selectBlessingList[1]));
        ThirdBtn.onClick.AddListener(() => BlessingGet(selectBlessingList[2]));
        RerollBtn.onClick.AddListener(() => BlessingReroll());
        ContinueBtn.onClick.AddListener(() => DialogManager.Instance.NextPage(UiManager.Instance.BlessingSelectUI, selectBlessingList));        
    }

    public void OnEnable()
    {
        selectBlessingList = BlessingTable.Instance.BlessingList;
        selectBlessingList = selectBlessingList.OrderBy(i => Random.value).ToList(); // 리스트의 값 랜덤으로 정렬

        BlessingUISet(firstBlessingImage, firstBlessingName, firstBlessingOption, selectBlessingList[0]); // 첫번째 축복 ui 셋팅
        BlessingUISet(SecondBlessingImage, SecondBlessingName, SecondBlessingOption, selectBlessingList[1]);
        BlessingUISet(ThirdBlessingImage, ThirdBlessingName, ThirdBlessingOption, selectBlessingList[2]);
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
        
        BlessingGetImage.sprite = selectBlessing.BlessingSprite;
        BlessingGetName.text = selectBlessing.BlessingName;
        for (int i = 0; i < selectBlessing.Option.Length; i++)
        {
            if (selectBlessing.Option[i].Contains("Lv")) // 특성값만 추가
            {
                BlessingGetOption.text += selectBlessing.Option[i] + "(최대 Lv. 5)\n";
            }            
        }

        if (selectBlessing.maxHp > 0)
        {
            BlessingGetOption.text += $"최대체력 : <color=#369341>{PlayerTable.Instance.MaxHp} (+{selectBlessing.maxHp})</color>" + "\n";
        }        
        if (selectBlessing.Atk > 0)
        {
            BlessingGetOption.text += $"공격력 : <color=#369341>{PlayerTable.Instance.Atk} (+{selectBlessing.Atk})</color>" + "\n";
        }
        if (selectBlessing.Def > 0)
        {
            BlessingGetOption.text += $"방어력 : <color=#369341>{PlayerTable.Instance.Defense} (+{selectBlessing.Def})</color>" + "\n";
        }
        if (selectBlessing.Cri > 0)
        {
            BlessingGetOption.text += $"치명타 : <color=#369341>{PlayerTable.Instance.Critical}% (+{selectBlessing.Cri}%)</color>" + "\n";
        }
        if (selectBlessing.Dod > 0)
        {
            BlessingGetOption.text += $"회피율 : <color=#369341>{PlayerTable.Instance.Dodge}% (+{selectBlessing.Dod}%)</color>" + "\n";
        }
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
