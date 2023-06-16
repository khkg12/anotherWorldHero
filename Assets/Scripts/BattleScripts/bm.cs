using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class bm : MonoBehaviour
{        
    public static bm Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<bm>();
            }
            return _Instance;
        }
    }
    private static bm _Instance;

    //��Ȱ �� ���� ��ư �ؽ�Ʈ
    public Image ResurrectionUI;
    public Image ResurrectionConfirmUI;
    public Button ResBtn;
    public Button DiscardBtn;
    public TextMeshProUGUI ResConfirmText;

    //��ų��ư
    /*
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
    */
    [SerializeField] private BattleDialogController battleDialogController;    
    // ����� �ؽ�Ʈ    
    public List<TextMeshProUGUI> PlayerDamageTextList;
    public List<TextMeshProUGUI> MonsterDamageTextList;

    public List<GameObject> PlayerPhysicalHitEffectList;
    public List<GameObject> PlayerMagicHitEffectList;

    public List<GameObject> MonsterPhysicalHitEffectList;
    public List<GameObject> MonsterMagicHitEffectList;

    public List<int> SkillCoolTime;
    public List<Image> CoolTimeImage;
    public List<TextMeshProUGUI> CoolTimeText;
    // ��Ʋ����
    public int BattleRound;

    // ��ų ��밡�� Ƚ�� �ؽ�Ʈ
    [SerializeField] private List<TextMeshProUGUI> SkillCountText;
    


    private void Start()
    {
        /*
        BtnEnable(true);
        BattleRound = 1;
        // PlayerTable�� NowDefense�� �������� ü���� �������� player��ũ��Ʈ�� ��������� �������۽� ���� playertable�� ������ �ִ� �� �ֱ�
        PlayerTable.Instance.NowDefense = 0; // �������� �� �� �ʱ�ȭ
        PlayerTable.Instance.StunStack = 0; // �������� �� �������� �ʱ�ȭ        
        PlayerTable.Instance.Scare = PlayerTable.Instance.Scare; // ù ��Ʋ�� ���� �� ���۽� ����Ư�� ����        
        PlayerTable.Instance.NowAtk = PlayerTable.Instance.Atk; // ù ���۽� ����Ư���� 3�ϸ��� ���ݷ� ������ų NowAtk������ �����ÿ� ���������ʴ� Player�� Atk ���� NowAtk�� �����ÿ��� ����� ����
        PlayerTable.Instance.WillPower = PlayerTable.Instance.WillPower; // ù ���۽� ����Ư�� �ο�                        

        //��ų��ư �̺�Ʈ
        //for�� 
        FirstSkillBtn.onClick.AddListener(() => PressBtn(0));
        SecondSkillBtn.onClick.AddListener(() => PressBtn(1));
        ThirdSkillBtn.onClick.AddListener(() => PressBtn(2));
        FourthSkillBtn.onClick.AddListener(() => PressBtn(3));
        FifthSkillBtn.onClick.AddListener(() => PressBtn(4));

        //��Ȱ, ����, ��ȰȮ�� ��ư �̺�Ʈ 
        ResBtn.onClick.AddListener(() => ResurrectionEvent());
        DiscardBtn.onClick.AddListener(() => GameManager.Instance.LoadDeadScene());

        // ó�� ������ ������ ��                
        nowmonster.monSkIllList = nowmonster.monSkIllList.OrderBy(i => Random.value).ToList();
        monsterSkill = nowmonster.monSkIllList[0];

        BattleDialogText.text = $"{monsterSkill.SkillText}\n������ �ұ�?";
        
        */
        StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator UpdateCoroutine() // ��ų���Ƚ�� �����ϴ� �ڵ������ �ֱ�
    {
        while (true)
        {           
            for(int i = 1; i < PlayerTable.Instance.playerSkillList.Count; ++i)
            {
                SkillCountText[i - 1].text = $"{PlayerTable.Instance.SkillAvailableCount[i]} / {PlayerTable.Instance.SkillFixedCount[i]}";
                // i-1�� ��ųī��Ʈ�ؽ�Ʈ�� 4���� ��ų�� 5���̱⶧���� �����޲ٱ�
                CoolTimeText[i].text = $"{SkillCoolTime[i]}��";
            }
            yield return new WaitForSeconds(0.1f);
        }
    }



    /*
    IEnumerator BtnClickEvent(int SkillNum)
    {
        playerSkill = PlayerTable.Instance.playerSkillList[SkillNum];        
        PlayerTable.Instance.IronBody = PlayerTable.Instance.IronBody; // ö�� ��ų ���ϸ��� �ߵ�        

        PlayerAttackAmount = PlayerTable.Instance.NowAtk * playerSkill.SkillPercentage; // �÷��̾ ���Ϳ��� �� ����� ����
        PlayerCriAttackAmount = PlayerTable.Instance.NowAtk * playerSkill.SkillPercentage * playerSkill.CriMultiple;

        yield return new WaitForEndOfFrame();
        {
            BtnEnable(false); // ��ư ��Ȱ��ȭ                                  
        }

        yield return new WaitForSeconds(0.1f);
        {
            PlayerSkillEvent(playerSkill, SkillNum);
            if (nowmonster.nowMonsterHp <= 0) StopAllCoroutines(); // ������ ü���� 0�� �Ǹ� �ڷ�ƾ�Լ� ����
            DefenseAmount = PlayerTable.Instance.NowDefense - nowmonster.nowMonsterAtk * monsterSkill.SkillPercentage <= 0 ? //���� ��ų �Լ����� ����� �������� �÷��̾ų���� ����
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
            nowmonster.monSkIllList = nowmonster.monSkIllList.OrderBy(i => Random.value).ToList(); // ���� ��ų ���� , ���Ͱ� ��ų�� ����� ���� ������ ����� ��ų ���ص�       
            monsterSkill = nowmonster.monSkIllList[0];
        }

        yield return new WaitForSeconds(1f);
        {
            BattleDialogText.text += $"\n{monsterSkill.SkillText}";
            BattleRound += 1; // ����ұ ��ȭâ�� �߱� ������ ��Ʋ���� ���, UI�� �߰��Ұ�
            if (BattleRound % 3 == 0) // 3�ϸ��� ���ݷ�������Ű�� ����Ư�� �ߵ�
            {
                PlayerTable.Instance.FightingSpirit = PlayerTable.Instance.FightingSpirit;
            }
        }

        yield return new WaitForSeconds(0.1f);
        {
            BattleDialogText.text += $"\n������ �ұ�?";
            BtnEnable(true);
            PlayerTable.Instance.NowDefense = 0; // ���� ������ ���� 0����
            SkillCount = 0;
        }
    }

    public void PlayerSkillEvent(BaseSkill playerSkill, int SkillNum)
    {
        if (PlayerTable.Instance.StunStack >= 1)
        {
            BattleDialogText.text += "\n��������! ������ �� ����!";
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
            playerSkill.SkillOption(nowmonster, nowplayer); // ù��° ��ư�� ������ �� ��ų����Ʈ�� ù��° ��ų�� ������ �� ��ų�� �ɼǽ���
        }
    }
    public void MonsterSkillEvent(MonsterSkill monsterSkill)
    {
        if (nowmonster.nowMonsterStunStack >= 1)
        {
            BattleDialogText.text += "\n���� �������·� ���� ������ �� ����!";
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
        if (PlayerTable.Instance.SecondSkillAvailableCount > 0) SecondSkillBtn.interactable = isBtnOn;
        if (PlayerTable.Instance.playerSkillCount >= 3 && PlayerTable.Instance.ThirdSkillAvailableCount != 0) ThirdSkillBtn.interactable = isBtnOn;
        if (PlayerTable.Instance.playerSkillCount >= 4 && PlayerTable.Instance.FourthSkillAvailableCount != 0) FourthSkillBtn.interactable = isBtnOn;
        if (PlayerTable.Instance.playerSkillCount >= 5 && PlayerTable.Instance.FifthSkillAvailableCount != 0) FifthSkillBtn.interactable = isBtnOn;
    }
    
    
    */

    public void ResurrectionEvent() // ��Ȱ ��ư Ŭ�� ��
    {        
        ResurrectionConfirmUI.gameObject.SetActive(true);
    }

    public void FloatingText(List<TextMeshProUGUI> DamageTextList, float Damage, int SkillCount)
    {
        DamageTextList[SkillCount].text = $"{Damage}";
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
