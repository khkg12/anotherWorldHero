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
    public Image BlessingGetUI; // ���������� ������ �ູ�� ��ü�� ����â
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
        FirstBtn.onClick.AddListener(() => BlessingSelectUI(selectBlessingList[0])); // ù��° ��ư ������ �� ù��° ������ �ɷ�ġ ����
        FirstBtn.onClick.AddListener(() => selectBlessingNum = 0);
        SecondBtn.onClick.AddListener(() => BlessingSelectUI(selectBlessingList[1])); // delegate�� ������ҵ� event��
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
        selectBlessingList = selectBlessingList.OrderBy(i => Random.value).ToList(); // ����Ʈ�� �� �������� ����

        BlessingUISet(firstBlessingImage, firstBlessingName, firstBlessingOption, selectBlessingList[0]); // ù��° �ູ ui ����
        BlessingUISet(SecondBlessingImage, SecondBlessingName, SecondBlessingOption, selectBlessingList[1]);
        BlessingUISet(ThirdBlessingImage, ThirdBlessingName, ThirdBlessingOption, selectBlessingList[2]);
    }

    public void BlessingSelectUI(Blessing selectBlessing) 
    {
        BlessingGetOption.text = ""; // �ູ �ɼ� �ؽ�ƮUI �ʱ�ȭ
        BlessingGetUI.gameObject.SetActive(true);       
        BlessingGetImage.sprite = selectBlessing.BlessingSprite;
        BlessingGetName.text = selectBlessing.BlessingName;        

        if (selectBlessing.maxHp > 0)
        {
            BlessingGetOption.text += $"<size=7>�ִ�ü�� : <color=#369341>{PlayerTable.Instance.MaxHp} -> {PlayerTable.Instance.MaxHp + selectBlessing.maxHp}</color></size>" + "\n\n";
        }        
        if (selectBlessing.Atk > 0)
        {
            BlessingGetOption.text += $"<size=7>���ݷ� : <color=#369341>{PlayerTable.Instance.Atk} -> {PlayerTable.Instance.Atk + selectBlessing.Atk}</color></size>" + "\n\n";
        }
        if (selectBlessing.Def > 0)
        {
            BlessingGetOption.text += $"<size=7>���� : <color=#369341>{PlayerTable.Instance.Defense} -> {PlayerTable.Instance.Defense + selectBlessing.Def} </color></size>" + "\n\n";
        }
        if (selectBlessing.Cri > 0)
        {
            BlessingGetOption.text += $"<size=7>ġ��Ÿ : <color=#369341>{PlayerTable.Instance.Critical} -> {PlayerTable.Instance.Critical + selectBlessing.Cri}</color></size>" + "\n\n";
        }
        if (selectBlessing.Dod > 0)
        {
            BlessingGetOption.text += $"<size=7>ȸ���� : <color=#369341>{PlayerTable.Instance.Dodge} ->  {PlayerTable.Instance.Dodge + selectBlessing.Dod}</color></size>" + "\n\n";
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
        if (PlayerStatus == 0) // ���ʷ� ȹ���ϴ� ���
        {
            BlessingGetOption.text += $"<size=7>�ű�Ư�� : {statusText.StatusName} (Lv.{BlessingStatus})</size>" + "\n\n";
            BlessingGetOption.text += statusText.OptionText + "\n\n";            
            NextBlessingLv.gameObject.SetActive(true);            
            NextBlessingLv.GetComponentInChildren<TextMeshProUGUI>().text = $"Lv {PlayerStatus + BlessingStatus} : " + statusText.LevelText[PlayerStatus + BlessingStatus - 1] + "\n";

        }
        else if(PlayerStatus == 5) // �ְ����� ���
        {
            BlessingGetOption.text += $"{statusText.StatusName} (Lv.5)" + "\n";
            BlessingGetOption.text += "�̹� �ְ� ������ �޼��� Ư���Դϴ�." + "\n";
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
        LayoutRebuilder.ForceRebuildLayoutImmediate(BlessingGetUI.rectTransform); // ���̾ƿ� ���� ������ �����ִ� �ڵ�, content size filter�� �ﰢ������ �������� �ȵǱ⶧���� �־��ִ� �ڵ�
    }

    public void BlessingUISet(Image BlessingImage, TextMeshProUGUI BlessingName, TextMeshProUGUI BlessingOption, Blessing selectBlessing) // �ູ ui ���� �Լ�
    {
        BlessingOption.text = "";
        BlessingImage.sprite = selectBlessing.BlessingSprite;
        BlessingName.text = selectBlessing.BlessingName;
        for (int i = 0; i < selectBlessing.Option.Length; i++)
        {
            BlessingOption.text += selectBlessing.Option[i] + "\n";
        }
    }

    public void BlessingGet(Blessing selectBlessing) // �ູ ��ư Ŭ�� �� ȣ��Ǵ� �Լ� �ɷ�ġ ��ȭ 
    {
        BlessingGetOption.text = ""; // �ູ �ɼ� �ؽ�ƮUI �ʱ�ȭ
        BlessingGetUI.gameObject.SetActive(true);

        PlayerTable.Instance.Scare += selectBlessing.ScarePt;
        PlayerTable.Instance.WillPower += selectBlessing.WillPowerPt;
        PlayerTable.Instance.FightingSpirit += selectBlessing.FightingSpiritPt;
        PlayerTable.Instance.IronBody += selectBlessing.IronBodyPt;

        PlayerTable.Instance.MaxHp += selectBlessing.maxHp;
        PlayerTable.Instance.Hp += selectBlessing.maxHp;  // ����hp�� �����ϴ� �ִ�ü�¸�ŭ ���
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
