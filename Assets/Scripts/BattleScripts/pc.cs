using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.TextCore.Text;
using System;
using System.Reflection;
using Unity.VisualScripting;

public class pc : MonoBehaviour
{
    [SerializeField] private Image nowPlayerHpBar;
    [SerializeField] private TextMeshProUGUI nowHpText;    
    [SerializeField] private Button PlayerInfoBtn;
    [SerializeField] private GameObject StunEffect;
    [SerializeField] private TextMeshProUGUI StunText;
    [SerializeField] private List<Button> BtnSkill;
    [SerializeField] private List<Image> ImageSkill;    
    [SerializeField] private MonsterClass Target;    
    private Character character;
    public static Action BtnEnableAction { get; private set; } // 이 스크립트 내에서만 set이 가능하게함, 즉 읽기전용

    private void Awake()
    {        
        character = GetComponent<Character>();
        BtnEnableAction += BtnEnable;
    }

    private void Start()
    {
        character.Initialize(Target); // pc를 캐릭터 오브젝트에 붙히고, 초기화시킴
        nowPlayerHpBar.fillAmount = PlayerTable.Instance.Hp / PlayerTable.Instance.MaxHp;        
        PlayerInfoBtn.onClick.AddListener(() => GameManager.Instance.PlayerInfoUI.gameObject.SetActive(true));

        for(int i = 0; i < character.skillList.Count; ++i) // 가지고 있는 스킬갯수만큼만 버튼활성화
        {
            int j = i; // 바로 i를 람다식에 넣으면 참조형식으로 클로저가 진행되어 마지막 변수값, 즉 4로 입력이되기 때문에 매 순간 할당되는 j를 정의해 해결
            ImageSkill[i].sprite = character.skillList[i].SkillSprite;
            BtnSkill[j].onClick.AddListener(() => StartCoroutine(character.UseSkill(j)));
            BtnSkill[j].onClick.AddListener(() => BtnDisable(j));
            if (i == 0) continue; // i가 0일땐 공격스킬이므로 패스            
            if (PlayerTable.Instance.SkillAvailableCount[i] > 0) BtnSkill[i].interactable = true; // 전투시작 시 스킬사용횟수가 0보다 크면 버튼활성화
            else BtnSkill[i].interactable = false;  // 아니면 비활성화
        }                
    }

    private void Update()
    {
        nowPlayerHpBar.fillAmount = Mathf.Lerp(nowPlayerHpBar.fillAmount, PlayerTable.Instance.Hp / PlayerTable.Instance.MaxHp, Time.deltaTime * 5f);
        nowHpText.text = $"{PlayerTable.Instance.Hp} / {PlayerTable.Instance.MaxHp}";
    }
    
    public void BtnDisable(int index)
    {
        Debug.Log("일단 인덱스" + index);      
        if(index != 0) PlayerTable.Instance.SkillAvailableCount[index]--; // 0, 즉 공격이 아닐경우 사용한 스킬의 사용회수를 하나 줄임        
        // 어차피 버튼을 클릭하면 모든 버튼은 비활성화됨 -> 따라서 버튼 클릭시 모든버튼 비활성화시킨 뒤 -> 활성화시킬 타이밍에 사용가능횟수 체크하고 비활성화
        for (int i = 0; i < character.skillList.Count; ++i)
        {
            BtnSkill[i].interactable = false;
        }
    }

    public void BtnEnable()
    {
        for (int i = 0; i < character.skillList.Count; ++i)
        {
            if(i == 0) BtnSkill[i].interactable = true; // 공격은 횟수가 무제한이므로 무조건 활성화
            else // 나머지의 경우
            {
                if (PlayerTable.Instance.SkillAvailableCount[i] > 0) BtnSkill[i].interactable = true;
            }
        }
    }

    /*
    public void playerResurrection() // 플레이어 부활
    {
        PlayerTable.Instance.Hp = 0.5f * PlayerTable.Instance.MaxHp;
        PlayerTable.Instance.ResChance -= 1;
    }
    */
}
