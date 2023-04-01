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
    public Image ActiveSkillGetUI; // 최종적으로 선택한 액티브스킬의 구체적 정보창
    public Image ActiveSkillGetImage;
    public TextMeshProUGUI ActiveSkillGetOption;

    public Image PassiveSkillGetUI; // 최종적으로 선택한 패시브스킬의 구체적 정보창
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

    public void OnEnable() // 스킬선택 UI가 활성화될때마다 실행되는 코드
    {
        // Active 스킬리스트 초기화
        selectActiveSkillList = SkillTable.Instance.ActiveSkillList;
        selectActiveSkillList = selectActiveSkillList.OrderBy(i => Random.value).ToList();// 리스트의 값 랜덤으로 정렬
        ActiveSkillUISet(ActiveSkillImage, ActiveSkillName, ActiveSkillOption, selectActiveSkillList[0]); // 액티브스킬 ui 셋팅

        // Passive 스킬리스트 초기화
        selectPassiveSkillList = SkillTable.Instance.PassiveSkillList;
        selectPassiveSkillList = selectPassiveSkillList.OrderBy(i => Random.value).ToList();
        PassiveSkillUISet(PassiveSkillImage, PassiveSkillName, PassiveSkillOption, selectPassiveSkillList[0]);
    }


    public void ActiveSkillGet(BaseSkill selectSkill) 
    {
        ActiveSkillGetOption.text = ""; // 스킬 옵션 텍스트UI 초기화
        ActiveSkillGetUI.gameObject.SetActive(true);

        switch (PlayerTable.Instance.playerSkillCount)
        {
            case 2: // 스킬창이 3칸이 비어있다면, 즉 채워져야 하는 스킬이 세번째 버튼                
                PlayerTable.Instance.playerSkillList[2] = selectSkill; // playerSkillList에 선택한 스킬 추가 나중에 가득찼을 때 불가능한 조건 넣기                 
                PlayerTable.Instance.ThirdSkillAvailableCount = selectSkill.AvailableCount; // 세번째 버튼에 추가될 스킬의 사용가능 횟수를 선택한 스킬의 사용가능 횟수로 초기화
                SkillTable.Instance.ActiveSkillList.Remove(selectSkill); // 뽑은 스킬은 중복되지 않게 스킬리스트에서 삭제                
                PlayerTable.Instance.playerSkillCount += 1;
                break;
            case 3:
                PlayerTable.Instance.playerSkillList[3] = selectSkill;                
                PlayerTable.Instance.FourthSkillAvailableCount = selectSkill.AvailableCount;
                SkillTable.Instance.ActiveSkillList.Remove(selectSkill); // 뽑은 스킬은 중복되지 않게 스킬리스트에서 삭제
                PlayerTable.Instance.playerSkillCount += 1;
                break;
            case 4:
                PlayerTable.Instance.playerSkillList[4] = selectSkill;                
                PlayerTable.Instance.FifthSkillAvailableCount = selectSkill.AvailableCount;
                SkillTable.Instance.ActiveSkillList.Remove(selectSkill); // 뽑은 스킬은 중복되지 않게 스킬리스트에서 삭제
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

    public void ActiveSkillUISet(Image ActiveSkillImage, TextMeshProUGUI ActiveSkillName, TextMeshProUGUI ActiveSkillOption, BaseSkill selectSkill) // 액티브스킬 ui 셋팅 함수
    {
        ActiveSkillOption.text = "";
        ActiveSkillImage.sprite = selectSkill.SkillSprite;
        ActiveSkillName.text = selectSkill.Name;
        for (int i = 0; i < selectSkill.SkillText.Length; i++)
        {
            ActiveSkillOption.text += selectSkill.SkillText[i] + "\n";
        }
    }

    public void PassiveSkillUISet(Image PassiveSkillImage, TextMeshProUGUI PassiveSkillName, TextMeshProUGUI PassiveSkillOption, PassiveSkill selectSkill) // 패시브스킬 ui 셋팅 함수
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
