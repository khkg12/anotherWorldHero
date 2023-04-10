using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SkillSelect : MonoBehaviour
{
    public Button ActiveSkillBtn;
    public Button PassiveSkillBtn;    
    public Button RerollBtn;
    public Button ContinueBtn;
    
    public List<BaseSkill> selectActiveSkillList;
    public List<PassiveSkill> selectPassiveSkillList;
    public Image ActiveSkillGetUI; // ���������� ������ ��Ƽ�꽺ų�� ��ü�� ����â
    public Image ActiveSkillGetImage;
    public TextMeshProUGUI ActiveSkillGetOption;

    public Image PassiveSkillGetUI; // ���������� ������ �нú꽺ų�� ��ü�� ����â
    public Image PassiveSkillGetImage;
    public TextMeshProUGUI PassiveSkillGetOption;

    public Image ActiveSkillImage;
    public TextMeshProUGUI ActiveSkillName;
    public TextMeshProUGUI ActiveSkillOption;

    public Image PassiveSkillImage;
    public TextMeshProUGUI PassiveSkillName;
    public TextMeshProUGUI PassiveSkillOption;    


    private void Start()
    {                                
        ActiveSkillBtn.onClick.AddListener(() => ActiveSkillGet(selectActiveSkillList[0])); 
        PassiveSkillBtn.onClick.AddListener(() => PassiveSkillGet(selectPassiveSkillList[0]));        
        RerollBtn.onClick.AddListener(() => SkillReroll());
        ContinueBtn.onClick.AddListener(() => DialogManager.Instance.NextPage(UiManager.Instance.SkillSelectUI, selectActiveSkillList));  
    }

    public void OnEnable() // ��ų���� UI�� Ȱ��ȭ�ɶ����� ����Ǵ� �ڵ�
    {
        // Active ��ų����Ʈ �ʱ�ȭ
        selectActiveSkillList = SkillTable.Instance.ActiveSkillList;
        selectActiveSkillList = selectActiveSkillList.OrderBy(i => Random.value).ToList();// ����Ʈ�� �� �������� ����
        ActiveSkillUISet(ActiveSkillImage, ActiveSkillName, ActiveSkillOption, selectActiveSkillList[0]); // ��Ƽ�꽺ų ui ����

        // Passive ��ų����Ʈ �ʱ�ȭ
        selectPassiveSkillList = SkillTable.Instance.PassiveSkillList;
        selectPassiveSkillList = selectPassiveSkillList.OrderBy(i => Random.value).ToList();
        PassiveSkillUISet(PassiveSkillImage, PassiveSkillName, PassiveSkillOption, selectPassiveSkillList[0]);
    }


    public void ActiveSkillGet(BaseSkill selectSkill) 
    {
        ActiveSkillGetOption.text = ""; // ��ų �ɼ� �ؽ�ƮUI �ʱ�ȭ
        ActiveSkillGetUI.gameObject.SetActive(true);

        switch (PlayerTable.Instance.playerSkillCount)
        {
            case 2: // ��ųâ�� 3ĭ�� ����ִٸ�, �� ä������ �ϴ� ��ų�� ����° ��ư                
                PlayerTable.Instance.playerSkillList[2] = selectSkill; // playerSkillList�� ������ ��ų �߰� ���߿� ����á�� �� �Ұ����� ���� �ֱ�                 
                PlayerTable.Instance.ThirdSkillAvailableCount = selectSkill.AvailableCount; // ����° ��ư�� �߰��� ��ų�� ��밡�� Ƚ���� ������ ��ų�� ��밡�� Ƚ���� �ʱ�ȭ
                SkillTable.Instance.ActiveSkillList.Remove(selectSkill); // ���� ��ų�� �ߺ����� �ʰ� ��ų����Ʈ���� ����                
                PlayerTable.Instance.playerSkillCount += 1;
                break;
            case 3:
                PlayerTable.Instance.playerSkillList[3] = selectSkill;                
                PlayerTable.Instance.FourthSkillAvailableCount = selectSkill.AvailableCount;
                SkillTable.Instance.ActiveSkillList.Remove(selectSkill); // ���� ��ų�� �ߺ����� �ʰ� ��ų����Ʈ���� ����
                PlayerTable.Instance.playerSkillCount += 1;
                break;
            case 4:
                PlayerTable.Instance.playerSkillList[4] = selectSkill;                
                PlayerTable.Instance.FifthSkillAvailableCount = selectSkill.AvailableCount;
                SkillTable.Instance.ActiveSkillList.Remove(selectSkill); // ���� ��ų�� �ߺ����� �ʰ� ��ų����Ʈ���� ����
                PlayerTable.Instance.playerSkillCount += 1; 
                break;
        }

        
        ActiveSkillGetImage.sprite = selectSkill.SkillSprite;
        for (int i = 0; i < selectSkill.SkillText.Length; i++)
        {
            ActiveSkillGetOption.text += selectSkill.SkillText[i] + "\n";
        }        
    }

    public void PassiveSkillGet(PassiveSkill selectSkill) 
    {
        PassiveSkillGetOption.text = "";
        PassiveSkillGetUI.gameObject.SetActive(true);

        PlayerTable.Instance.MaxHp += selectSkill.MaxHp;        
        PlayerTable.Instance.Atk += selectSkill.Atk;
        PlayerTable.Instance.Defense += selectSkill.Def;
        PlayerTable.Instance.Critical += selectSkill.Cri;
        PlayerTable.Instance.Dodge += selectSkill.Dod;        
        
        PassiveSkillGetImage.sprite = selectSkill.Skillsprite;
        for (int i = 0; i < selectSkill.SkillText.Length; i++)
        {
            PassiveSkillGetOption.text += selectSkill.SkillText[i] + "\n";
        }        
    }

    public void ActiveSkillUISet(Image ActiveSkillImage, TextMeshProUGUI ActiveSkillName, TextMeshProUGUI ActiveSkillOption, BaseSkill selectSkill) // ��Ƽ�꽺ų ui ���� �Լ�
    {
        ActiveSkillOption.text = "";
        ActiveSkillImage.sprite = selectSkill.SkillSprite;
        ActiveSkillName.text = selectSkill.Name;
        for (int i = 0; i < selectSkill.SkillText.Length; i++)
        {
            ActiveSkillOption.text += selectSkill.SkillText[i] + "\n";
        }
    }

    public void PassiveSkillUISet(Image PassiveSkillImage, TextMeshProUGUI PassiveSkillName, TextMeshProUGUI PassiveSkillOption, PassiveSkill selectSkill) // �нú꽺ų ui ���� �Լ�
    {
        PassiveSkillOption.text = "";
        PassiveSkillImage.sprite = selectSkill.Skillsprite;
        PassiveSkillName.text = selectSkill.Name;
        for (int i = 0; i < selectSkill.SkillText.Length; i++)
        {
            PassiveSkillOption.text += selectSkill.SkillText[i] + "\n";
        }
    }

    public void SkillReroll()
    {
        ActiveSkillOption.text = "";
        PassiveSkillOption.text = "";
        selectActiveSkillList = selectActiveSkillList.OrderBy(i => Random.value).ToList();
        selectPassiveSkillList = selectPassiveSkillList.OrderBy(i => Random.value).ToList();
        ActiveSkillUISet(ActiveSkillImage, ActiveSkillName, ActiveSkillOption, selectActiveSkillList[0]);
        PassiveSkillUISet(PassiveSkillImage, PassiveSkillName, PassiveSkillOption, selectPassiveSkillList[0]);
    }
}
