using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

public class BattleManager : MonoBehaviour
{
    public PlayerController nowplayer;        
    public MonsterController nowmonster;    

    public static BattleManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<BattleManager>();
            }
            return _Instance;
        }
    }
    private static BattleManager _Instance;

    //��Ȱ �� ���� ��ư �ؽ�Ʈ
    public Image ResurrectionUI;
    public Image ResurrectionConfirmUI;
    public Button ResBtn;    
    public Button DiscardBtn;
    public TextMeshProUGUI ResConfirmText;

    //��ų��ư
    public Button FirstSkillBtn;
    public Image FirstSkillImage;
    public Button SecondSkillBtn;
    public Image SecondSkillImage;
    public Button ThirdSkillBtn;
    public Image ThirdSkillImage;
    public Button FourthSkillBtn;
    public Image FourthSkillImage;
    public Button FifthSkillBtn;
    public Image FifthSkillImage;

    public TextMeshProUGUI BattleDialogText;
    // ����� �ؽ�Ʈ    
    public List<TextMeshProUGUI> PlayerDamageTextList; 
    public List<TextMeshProUGUI> MonsterDamageTextList;

    // ��Ʋ����
    public int BattleRound;

    // ��ų ��밡�� Ƚ�� �ؽ�Ʈ
    public TextMeshProUGUI SecondSkillText;
    public TextMeshProUGUI ThirdSkillText;
    public TextMeshProUGUI FourthSkillText;
    public TextMeshProUGUI FifthSkillText;

    public BaseSkill playerSkill;
    public MonsterSkill monsterSkill;
    public int SkillCount; // ����� �ؽ�Ʈ ���� ����

    public float DefenseAmount;
    public float AttackAmount;
    public float CriDefenseAmount;
    public float CriAttackAmount;    


    private void Start()
    {
        BtnEnable(true); 
        BattleRound = 1;
        PlayerTable.Instance.NowDefense = 0; // �������� �� �� �ʱ�ȭ
        PlayerTable.Instance.StunStack = 0; // �������� �� �������� �ʱ�ȭ        
        PlayerTable.Instance.Scare = PlayerTable.Instance.Scare; // ù ��Ʋ�� ���� �� ���۽� ����Ư�� ����        
        PlayerTable.Instance.NowAtk = PlayerTable.Instance.Atk; // ù ���۽� ����Ư���� 3�ϸ��� ���ݷ� ������ų NowAtk������ �����ÿ� ���������ʴ� Player�� Atk ���� NowAtk�� �����ÿ��� ����� ����
        PlayerTable.Instance.WillPower = PlayerTable.Instance.WillPower; // ù ���۽� ����Ư�� �ο�        

        FirstSkillImage.sprite = PlayerTable.Instance.playerSkillList[0].SkillSprite;
        SecondSkillImage.sprite = PlayerTable.Instance.playerSkillList[1].SkillSprite;
        ThirdSkillImage.sprite = PlayerTable.Instance.playerSkillList[2].SkillSprite; // ��Ʋ���� ����° ��ų �̹��� ������ ��ų�� �̹����� ä����            
        FourthSkillImage.sprite = PlayerTable.Instance.playerSkillList[3].SkillSprite;
        FourthSkillImage.sprite = PlayerTable.Instance.playerSkillList[4].SkillSprite;
        

        if (PlayerTable.Instance.SecondSkillAvailableCount <= 0) // �������� �� ����Ƚ���� 0�̶�� ��ư�� ��Ȱ��ȭ
        {
            SecondSkillBtn.interactable = false;
        }   // ��ų��밡��Ƚ�� üũ �� ��ư ��Ȱ��ȭ     
        if (PlayerTable.Instance.ThirdSkillAvailableCount <= 0)
        {
            ThirdSkillBtn.interactable = false;
        }        
        if (PlayerTable.Instance.FourthSkillAvailableCount <= 0)
        {
            FourthSkillBtn.interactable = false;
        }        
        if (PlayerTable.Instance.FifthSkillAvailableCount <= 0)
        {
            FifthSkillBtn.interactable = false;
        }

        //��ų��ư �̺�Ʈ        
        FirstSkillBtn.onClick.AddListener(() => PressBtn(0));
        SecondSkillBtn.onClick.AddListener(() => PressBtn(1));        
        ThirdSkillBtn.onClick.AddListener(() => PressBtn(2));
        FourthSkillBtn.onClick.AddListener(() => PressBtn(3));
        FifthSkillBtn.onClick.AddListener(() => PressBtn(4));

        //��Ȱ, ����, ��ȰȮ�� ��ư �̺�Ʈ 
        ResBtn.onClick.AddListener(() => ResurrectionEvent());
        DiscardBtn.onClick.AddListener(() => GameManager.Instance.LoadDeadScene());

        // ó�� ������ ������ ��
        MonsterTable.Instance.monsterSkillList = MonsterTable.Instance.monsterSkillList.OrderBy(i => Random.value).ToList(); // ���� ��ų ����        
        monsterSkill = MonsterTable.Instance.monsterSkillList[0];

        BattleDialogText.text += $"\n{monsterSkill.SkillText} ������ �ұ�?";

        StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            SecondSkillText.text = $"{PlayerTable.Instance.SecondSkillAvailableCount} / {PlayerTable.Instance.playerSkillList[1].AvailableCount}";
            if (PlayerTable.Instance.ThirdSkillAvailableCount >= 0)
            {
                ThirdSkillText.text = $"{PlayerTable.Instance.ThirdSkillAvailableCount} / {PlayerTable.Instance.playerSkillList[2].AvailableCount}";
            }
            if (PlayerTable.Instance.FourthSkillAvailableCount >= 0)
            {
                FourthSkillText.text = $"{PlayerTable.Instance.FourthSkillAvailableCount} / {PlayerTable.Instance.playerSkillList[3].AvailableCount}";
            }
            if (PlayerTable.Instance.FifthSkillAvailableCount >= 0)
            {
                FifthSkillText.text = $"{PlayerTable.Instance.FifthSkillAvailableCount} / {PlayerTable.Instance.playerSkillList[4].AvailableCount}";
            }
            yield return new WaitForSeconds(0.1f);
        }
    }    

    public void PressBtn(int SkillNum)
    {
        StartCoroutine("BtnClickEvent", SkillNum);
    }    

    IEnumerator BtnClickEvent(int SkillNum) 
    {        
        playerSkill = PlayerTable.Instance.playerSkillList[SkillNum];
        Debug.Log(playerSkill.Name);
        /*MonsterTable.Instance.monsterSkillList = MonsterTable.Instance.monsterSkillList.OrderBy(i => Random.value).ToList(); // ���� ��ų ����        
        monsterSkill = MonsterTable.Instance.monsterSkillList[0];*/ // �ڷ� �̵�
        PlayerTable.Instance.IronBody = PlayerTable.Instance.IronBody; // ö�� ��ų ���ϸ��� �ߵ�        

        yield return new WaitForEndOfFrame();
        {
            BtnEnable(false); // ��ư ��Ȱ��ȭ
            BattleDialogText.text = "";
            playerSkill.SkillDialog[0] = "";            
        }        
                
        yield return new WaitForSeconds(0.1f);
        {
            PlayerSkillEvent(playerSkill, SkillNum);
            DefenseAmount = PlayerTable.Instance.NowDefense - nowmonster.nowMonsterAtk * monsterSkill.SkillPercentage <= 0 ? //160 ~ 167 ���� ��ų �Լ����� ����� �������� �÷��̾ų���� ����
            PlayerTable.Instance.NowDefense : nowmonster.nowMonsterAtk * monsterSkill.SkillPercentage;
            AttackAmount = PlayerTable.Instance.NowDefense - nowmonster.nowMonsterAtk * monsterSkill.SkillPercentage <= 0 ?
                nowmonster.nowMonsterAtk * monsterSkill.SkillPercentage - PlayerTable.Instance.NowDefense : 0;
            CriDefenseAmount = PlayerTable.Instance.NowDefense - nowmonster.nowMonsterAtk * monsterSkill.SkillPercentage * monsterSkill.CriMultiple <= 0 ?
                PlayerTable.Instance.NowDefense : nowmonster.nowMonsterAtk * monsterSkill.SkillPercentage * monsterSkill.CriMultiple;
            CriAttackAmount = PlayerTable.Instance.NowDefense - nowmonster.nowMonsterAtk * monsterSkill.SkillPercentage * monsterSkill.CriMultiple <= 0 ?
                nowmonster.nowMonsterAtk * monsterSkill.SkillPercentage * monsterSkill.CriMultiple - PlayerTable.Instance.NowDefense : 0;
        }
        
        yield return new WaitForSeconds(1f);
        {
            SkillCount = 0; // �÷��̾� ��ų �Լ����� skillcount(������)�� �ø��� ������ ���� ��ų �Լ� ���� �� �ʱ�ȭ��Ŵ
            MonsterSkillEvent(monsterSkill);
            MonsterTable.Instance.monsterSkillList = MonsterTable.Instance.monsterSkillList.OrderBy(i => Random.value).ToList(); // ���� ��ų ���� , ���Ͱ� ��ų�� ����� ���� ������ ����� ��ų ���ص�       
            monsterSkill = MonsterTable.Instance.monsterSkillList[0];
        }                

        yield return new WaitForSeconds(1f);
        {
            BattleDialogText.text += $"\n{monsterSkill.SkillText} ������ �ұ�?";
            BattleRound += 1; // ����ұ ��ȭâ�� �߱� ������ ��Ʋ���� ���, UI�� �߰��Ұ�
            if(BattleRound % 3 == 0) // 3�ϸ��� ���ݷ�������Ű�� ����Ư�� �ߵ�
            {
                PlayerTable.Instance.FightingSpirit = PlayerTable.Instance.FightingSpirit;
            }                     
        }
     
        yield return new WaitForSeconds(0.1f);
        {         
            BtnEnable(true);
            PlayerTable.Instance.NowDefense = 0; // ���� ������ ���� 0����
            SkillCount = 0;
        }         
    }    

    public void PlayerSkillEvent(BaseSkill playerSkill, int SkillNum)
    {
        if (PlayerTable.Instance.StunStack >= 1)
        {
            BattleDialogText.text += "\n\n��������! ������ �� ����!";
            nowplayer.StunEffect.gameObject.SetActive(true);
            PlayerTable.Instance.StunStack -= 1;
            return;
        }
        else
        {
            switch (SkillNum)
            {
                case 1: // Ŭ���� ��ų��ư�� 2��°�϶�
                    PlayerTable.Instance.SecondSkillAvailableCount -= 1; // ��ų��밡��Ƚ���� 1 ���̰�                
                    if (PlayerTable.Instance.SecondSkillAvailableCount <= 0) // ���� �� ����Ƚ���� 0�̶�� ��ư�� ��Ȱ��ȭ
                    {
                        SecondSkillBtn.interactable = false;
                    }
                    break;
                case 2:
                    PlayerTable.Instance.ThirdSkillAvailableCount -= 1;                    
                    if (PlayerTable.Instance.SecondSkillAvailableCount <= 0)
                    {
                        ThirdSkillBtn.interactable = false;
                    }
                    break;
                case 3:
                    PlayerTable.Instance.FourthSkillAvailableCount -= 1;
                    if (PlayerTable.Instance.SecondSkillAvailableCount <= 0)
                    {
                        FourthSkillBtn.interactable = false;
                    }
                    break;
                case 4:
                    PlayerTable.Instance.FifthSkillAvailableCount -= 1;
                    if (PlayerTable.Instance.SecondSkillAvailableCount <= 0)
                    {
                        FifthSkillBtn.interactable = false;
                    }
                    break;
            } // ��ų��밡��Ƚ�� ���� üũ, 0�ϰ�찡 ���� ������ ������ Ƚ�������� ���⶧��, ���������϶� ��밡��Ƚ�� �پ��������
            playerSkill.SkillOption(nowmonster, nowplayer); // ù��° ��ư�� ������ �� ��ų����Ʈ�� ù��° ��ų�� ������ �� ��ų�� �ɼǽ���, �� �ڵ�� ������� �ٲ�� �ȵ� ���� : option�� ����Ǿ� �׶� ��ų���̷αװ� �������� ����                                    
        }
    }
    public void MonsterSkillEvent(MonsterSkill monsterSkill)
    {
        if (nowmonster.nowMonsterStunStack >= 1)
        {
            BattleDialogText.text += "\n\n���� �������·� ���� ������ �� ����!";
            nowmonster.nowMonsterStunStack -= 1;
            return;
        }
        else
        {            
            monsterSkill.SkillOption(nowmonster, nowplayer);                   
        }
    }
    public void BtnEnable(bool isBtnOn)
    {
        FirstSkillBtn.interactable = isBtnOn; 
        if(PlayerTable.Instance.SecondSkillAvailableCount > 0) SecondSkillBtn.interactable = isBtnOn;
        if (PlayerTable.Instance.playerSkillCount >= 3 && PlayerTable.Instance.ThirdSkillAvailableCount != 0) ThirdSkillBtn.interactable = isBtnOn;
        if (PlayerTable.Instance.playerSkillCount >= 4 && PlayerTable.Instance.FourthSkillAvailableCount != 0) FourthSkillBtn.interactable = isBtnOn;
        if (PlayerTable.Instance.playerSkillCount >= 5 && PlayerTable.Instance.FifthSkillAvailableCount != 0) FifthSkillBtn.interactable = isBtnOn;
    }     
    public void ResurrectionEvent() // ��Ȱ ��ư Ŭ�� ��
    {
        nowplayer.playerResurrection();
        ResurrectionConfirmUI.gameObject.SetActive(true);
    }
    public void FloatingText(List<TextMeshProUGUI> DamageTextList, float Damage, int SkillCount)
    {
        DamageTextList[SkillCount].text = $"-{Damage}";
        DamageTextList[SkillCount].gameObject.SetActive(true);
    }
    public bool CriAttack(int CriticalRate) // ũ��Ƽ�� Ȯ��
    {
        int range = Random.Range(1, 101);
        if (range < CriticalRate) return true;
        else return false;
    }
    public bool DodgeSucess(int DodgeRate) // ȸ�� Ȯ��
    {
        int range = Random.Range(1, 101);
        if (range < DodgeRate) return true;
        else return false;
    }
}


