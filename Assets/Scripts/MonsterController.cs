using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;
using System.Threading;
using UnityEditor.Experimental.GraphView;

public class MonsterController : MonoBehaviour
{
    public SpriteRenderer nowMonsterSprite;
    public string nowMonsterName;
    public float nowMonsterHp;
    public float nowMonsterMaxHp;
    public int nowMonsterDodge;
    public int nowMonsterCri;
    public int nowMonsterStunStack;
    public bool IsMonsterBoss;
    public List<MonsterSkill> monSkIllList;

    public Image nowMonsterHpBar;    
    public Animator monsterAni;
    public TextMeshProUGUI monsterNameText;

    public Image MonsterImage;
    public TextMeshProUGUI monsterInfoNameText;
    public TextMeshProUGUI monsterHpText;
    public TextMeshProUGUI monsterAtkText;
    public TextMeshProUGUI monsterCriText;
    public TextMeshProUGUI monsterDodText;

    public float nowMonsterAtk
    {
        get => _nowMonsterAtk;
        set
        {
            _nowMonsterAtk = value <= 0 ? 0 : value;
        }
    }
    public float _nowMonsterAtk;

    private List<Monster> monsterList;

    private int PoisonDotCount;
    private int FireDotCount;

    private void Awake()
    {
        switch (GameManager.Instance.NowAct)
        {
            case 1:
                monsterList = MonsterTable.Instance.MonsterList;
                break;
            case 2:
                monsterList = MonsterTable.Instance.SecondActMonsterList;
                break;
            case 3:
                monsterList = MonsterTable.Instance.ThirdActMonsterList;
                break;
            case 4:
                monsterList = MonsterTable.Instance.FourthActMonsterList;
                break;
        }
        MonsterSet(MonsterTable.Instance.MonsterNum); // ���� ���� ����                
    }

    public void Update()
    {                
        nowMonsterHpBar.fillAmount = Mathf.Lerp(nowMonsterHpBar.fillAmount, nowMonsterHp / nowMonsterMaxHp, Time.deltaTime * 5f);                    
    }

    public async void MonsterDamaged(float DamageAmount)
    {        
        nowMonsterHp -= DamageAmount;
        monsterAni.SetTrigger("IsHit");        
        if (nowMonsterHp <= 0 && IsMonsterBoss == false) // �¸��Ͽ��� �� ������ �����Ƿ� ��ǻ� ������ ������ ����� �ڵ�� �������
        {
            await Task.Delay(200);                
            GameManager.Instance.IsMonsterDead = true; // ������ ü���� 0, �� ������ �÷��� true
            GameManager.Instance.IsAni = true;
            GameManager.Instance.LoadMainScene();
            await Task.Delay(100);
            UiManager.Instance.ItemSelectUI.gameObject.SetActive(true);
        }
        else if(nowMonsterHp <= 0 && IsMonsterBoss == true)
        {            
            // ���� ��� �ִ�
            await Task.Delay(500);            
            GameManager.Instance.AfterVictory();
            // nextDialog�� ������Ѿ��� ��Խ����ų��?
        }
    }

    public IEnumerator MonsterSingleDamaged(BaseSkill Skill, string SkillType)
    {
        if (SkillType == "Physical")
        {            
            BattleManager.Instance.MonsterPhysicalHitEffectList[0].gameObject.SetActive(false);
            BattleManager.Instance.MonsterPhysicalHitEffectList[0].gameObject.SetActive(true);
        }
        else
        {
            BattleManager.Instance.MonsterMagicHitEffectList[0].gameObject.SetActive(false);
            BattleManager.Instance.MonsterMagicHitEffectList[0].gameObject.SetActive(true);
        }

        if (BattleManager.Instance.CriAttack(PlayerTable.Instance.Critical)) // ġ��Ÿ �����̶��
        {
            MonsterDamaged(BattleManager.Instance.PlayerCriAttackAmount);
            BattleManager.Instance.BattleDialogText.text += $"\n\nġ��Ÿ!! {Skill.Name}! {BattleManager.Instance.PlayerCriAttackAmount} ��������!";
            BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, BattleManager.Instance.PlayerCriAttackAmount, BattleManager.Instance.SkillCount);
        }
        else
        {
            MonsterDamaged(BattleManager.Instance.PlayerAttackAmount);
            BattleManager.Instance.BattleDialogText.text += $"\n\n{Skill.Name}! {BattleManager.Instance.PlayerAttackAmount} ��������!";
            BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, BattleManager.Instance.PlayerAttackAmount, BattleManager.Instance.SkillCount);
        }
        yield return null;
    }

    public IEnumerator MonsterMultiDamaged(BaseSkill Skill, int SkillTimes, string SkillType)
    {
        for (int i = 0; i < SkillTimes; i++)
        {
            // ��ų����Ʈ ����
            if (SkillType == "Physical")
            {
                BattleManager.Instance.MonsterPhysicalHitEffectList[i].gameObject.SetActive(false);
                BattleManager.Instance.MonsterPhysicalHitEffectList[i].gameObject.SetActive(true);
            }
            else
            {
                BattleManager.Instance.MonsterMagicHitEffectList[i].gameObject.SetActive(false);
                BattleManager.Instance.MonsterMagicHitEffectList[i].gameObject.SetActive(true);
            }

            if (BattleManager.Instance.CriAttack(BattleManager.Instance.nowmonster.nowMonsterCri)) // ġ��Ÿ �����̶��
            {
                MonsterDamaged(BattleManager.Instance.PlayerCriAttackAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\nġ��Ÿ!! {Skill.Name}! {BattleManager.Instance.PlayerCriAttackAmount} ��������!";
                BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, BattleManager.Instance.PlayerCriAttackAmount, BattleManager.Instance.SkillCount);
            }
            else
            {
                MonsterDamaged(BattleManager.Instance.PlayerAttackAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\n{Skill.Name}! {BattleManager.Instance.PlayerAttackAmount} ��������!";
                BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, BattleManager.Instance.PlayerAttackAmount, BattleManager.Instance.SkillCount);
            }
            BattleManager.Instance.SkillCount += 1;
            yield return new WaitForSeconds(0.2f);
        }
    }

    public IEnumerator MonsterDotDamaged(MonsterController nowMonster, BaseSkill Skill, int SkillTimes, string SkillType)
    {
        if (SkillType == "Physical")
        {
            BattleManager.Instance.MonsterPhysicalHitEffectList[0].gameObject.SetActive(false);
            BattleManager.Instance.MonsterPhysicalHitEffectList[0].gameObject.SetActive(true);
        }
        else
        {
            BattleManager.Instance.MonsterMagicHitEffectList[0].gameObject.SetActive(false);
            BattleManager.Instance.MonsterMagicHitEffectList[0].gameObject.SetActive(true);
        }

        if (BattleManager.Instance.CriAttack(PlayerTable.Instance.Critical)) // ġ��Ÿ �����̶��
        {
            nowMonster.MonsterDamaged(BattleManager.Instance.PlayerCriAttackAmount);
            BattleManager.Instance.BattleDialogText.text += $"\n\nġ��Ÿ!! {Skill.Name}! {BattleManager.Instance.PlayerCriAttackAmount} ��������!";
            BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, BattleManager.Instance.PlayerCriAttackAmount, BattleManager.Instance.SkillCount);
        }
        else
        {
            nowMonster.MonsterDamaged(BattleManager.Instance.PlayerAttackAmount);
            BattleManager.Instance.BattleDialogText.text += $"\n\n{Skill.Name}! {BattleManager.Instance.PlayerAttackAmount} ��������!";
            BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, BattleManager.Instance.PlayerAttackAmount, BattleManager.Instance.SkillCount);
        }

        for(int i = 0; i < SkillTimes; i++)
        {
            
        }

        yield return null;
    }

    public void startMonsterSingleDamaged(BaseSkill Skill, string SkillType)
    {
        StartCoroutine(MonsterSingleDamaged(Skill, SkillType));
    }

    public void startMonsterMultiDamaged(BaseSkill Skill, int SkillTimes, string SkillType)
    {
        StartCoroutine(MonsterMultiDamaged(Skill, SkillTimes, SkillType));
    }    

    public void MonsterSet(int MonsterNum)
    {
        Monster nowMonster = monsterList[MonsterNum];
        nowMonsterSprite.sprite = nowMonster.MonsterSprite;
        nowMonsterName = nowMonster.MonsterName;
        nowMonsterHp = nowMonster.MonsterHp;
        nowMonsterMaxHp = nowMonster.MonsterMaxHp;
        nowMonsterAtk = nowMonster.MonsterAtk;
        nowMonsterDodge = nowMonster.MonsterDodge;
        nowMonsterCri = nowMonster.MonsterCri;
        nowMonsterStunStack = nowMonster.MonsterStunStack;
        monsterNameText.text = nowMonster.MonsterName;        
        IsMonsterBoss = nowMonster.IsMonsterBoss;
        monSkIllList = nowMonster.monsterSkillList;
        // ���� ����â UI
        monsterInfoNameText.text = nowMonster.MonsterName;
        MonsterImage.sprite = nowMonster.MonsterSprite;
        monsterHpText.text = $"ü�� : {nowMonster.MonsterMaxHp}";
        monsterAtkText.text = $"���ݷ� : {nowMonster.MonsterAtk}";
        monsterCriText.text = $"ġ��Ÿ : {nowMonster.MonsterCri}%";
        monsterDodText.text = $"ȸ���� : {nowMonster.MonsterDodge}%";
    }
    
}
 