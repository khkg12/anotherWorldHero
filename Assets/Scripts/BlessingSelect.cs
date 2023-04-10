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
        FirstBtn.onClick.AddListener(() => BlessingGet(selectBlessingList[0])); // ù��° ��ư ������ �� ù��° ������ �ɷ�ġ ����
        SecondBtn.onClick.AddListener(() => BlessingGet(selectBlessingList[1]));
        ThirdBtn.onClick.AddListener(() => BlessingGet(selectBlessingList[2]));
        RerollBtn.onClick.AddListener(() => BlessingReroll());
        ContinueBtn.onClick.AddListener(() => DialogManager.Instance.NextPage(UiManager.Instance.BlessingSelectUI, selectBlessingList));        
    }

    public void OnEnable()
    {
        selectBlessingList = BlessingTable.Instance.BlessingList;
        selectBlessingList = selectBlessingList.OrderBy(i => Random.value).ToList(); // ����Ʈ�� �� �������� ����

        BlessingUISet(firstBlessingImage, firstBlessingName, firstBlessingOption, selectBlessingList[0]); // ù��° �ູ ui ����
        BlessingUISet(SecondBlessingImage, SecondBlessingName, SecondBlessingOption, selectBlessingList[1]);
        BlessingUISet(ThirdBlessingImage, ThirdBlessingName, ThirdBlessingOption, selectBlessingList[2]);
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
        
        BlessingGetImage.sprite = selectBlessing.BlessingSprite;
        BlessingGetName.text = selectBlessing.BlessingName;
        for (int i = 0; i < selectBlessing.Option.Length; i++)
        {
            if (selectBlessing.Option[i].Contains("Lv")) // Ư������ �߰�
            {
                BlessingGetOption.text += selectBlessing.Option[i] + "(�ִ� Lv. 5)\n";
            }            
        }

        if (selectBlessing.maxHp > 0)
        {
            BlessingGetOption.text += $"�ִ�ü�� : <color=#369341>{PlayerTable.Instance.MaxHp} (+{selectBlessing.maxHp})</color>" + "\n";
        }        
        if (selectBlessing.Atk > 0)
        {
            BlessingGetOption.text += $"���ݷ� : <color=#369341>{PlayerTable.Instance.Atk} (+{selectBlessing.Atk})</color>" + "\n";
        }
        if (selectBlessing.Def > 0)
        {
            BlessingGetOption.text += $"���� : <color=#369341>{PlayerTable.Instance.Defense} (+{selectBlessing.Def})</color>" + "\n";
        }
        if (selectBlessing.Cri > 0)
        {
            BlessingGetOption.text += $"ġ��Ÿ : <color=#369341>{PlayerTable.Instance.Critical}% (+{selectBlessing.Cri}%)</color>" + "\n";
        }
        if (selectBlessing.Dod > 0)
        {
            BlessingGetOption.text += $"ȸ���� : <color=#369341>{PlayerTable.Instance.Dodge}% (+{selectBlessing.Dod}%)</color>" + "\n";
        }
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
