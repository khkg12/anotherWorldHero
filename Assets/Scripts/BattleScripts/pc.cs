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
    public static Action BtnEnableAction { get; private set; } // �� ��ũ��Ʈ �������� set�� �����ϰ���, �� �б�����

    private void Awake()
    {        
        character = GetComponent<Character>();
        BtnEnableAction += BtnEnable;
    }

    private void Start()
    {
        character.Initialize(Target); // pc�� ĳ���� ������Ʈ�� ������, �ʱ�ȭ��Ŵ
        nowPlayerHpBar.fillAmount = PlayerTable.Instance.Hp / PlayerTable.Instance.MaxHp;        
        PlayerInfoBtn.onClick.AddListener(() => GameManager.Instance.PlayerInfoUI.gameObject.SetActive(true));

        for(int i = 0; i < character.skillList.Count; ++i) // ������ �ִ� ��ų������ŭ�� ��ưȰ��ȭ
        {
            int j = i; // �ٷ� i�� ���ٽĿ� ������ ������������ Ŭ������ ����Ǿ� ������ ������, �� 4�� �Է��̵Ǳ� ������ �� ���� �Ҵ�Ǵ� j�� ������ �ذ�
            ImageSkill[i].sprite = character.skillList[i].SkillSprite;
            BtnSkill[j].onClick.AddListener(() => StartCoroutine(character.UseSkill(j)));
            BtnSkill[j].onClick.AddListener(() => BtnDisable(j));
            if (i == 0) continue; // i�� 0�϶� ���ݽ�ų�̹Ƿ� �н�            
            if (PlayerTable.Instance.SkillAvailableCount[i] > 0) BtnSkill[i].interactable = true; // �������� �� ��ų���Ƚ���� 0���� ũ�� ��ưȰ��ȭ
            else BtnSkill[i].interactable = false;  // �ƴϸ� ��Ȱ��ȭ
        }                
    }

    private void Update()
    {
        nowPlayerHpBar.fillAmount = Mathf.Lerp(nowPlayerHpBar.fillAmount, PlayerTable.Instance.Hp / PlayerTable.Instance.MaxHp, Time.deltaTime * 5f);
        nowHpText.text = $"{PlayerTable.Instance.Hp} / {PlayerTable.Instance.MaxHp}";
    }
    
    public void BtnDisable(int index)
    {
        Debug.Log("�ϴ� �ε���" + index);      
        if(index != 0) PlayerTable.Instance.SkillAvailableCount[index]--; // 0, �� ������ �ƴҰ�� ����� ��ų�� ���ȸ���� �ϳ� ����        
        // ������ ��ư�� Ŭ���ϸ� ��� ��ư�� ��Ȱ��ȭ�� -> ���� ��ư Ŭ���� ����ư ��Ȱ��ȭ��Ų �� -> Ȱ��ȭ��ų Ÿ�ֿ̹� ��밡��Ƚ�� üũ�ϰ� ��Ȱ��ȭ
        for (int i = 0; i < character.skillList.Count; ++i)
        {
            BtnSkill[i].interactable = false;
        }
    }

    public void BtnEnable()
    {
        for (int i = 0; i < character.skillList.Count; ++i)
        {
            if(i == 0) BtnSkill[i].interactable = true; // ������ Ƚ���� �������̹Ƿ� ������ Ȱ��ȭ
            else // �������� ���
            {
                if (PlayerTable.Instance.SkillAvailableCount[i] > 0) BtnSkill[i].interactable = true;
            }
        }
    }

    /*
    public void playerResurrection() // �÷��̾� ��Ȱ
    {
        PlayerTable.Instance.Hp = 0.5f * PlayerTable.Instance.MaxHp;
        PlayerTable.Instance.ResChance -= 1;
    }
    */
}
