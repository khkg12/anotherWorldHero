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

    List<MonsterSkill> monSkIllList;    

    private void Awake()
    {        
        monSkIllList = MonsterTable.Instance.monsterSkillList;
        MonsterSet(MonsterTable.Instance.MonsterNum); // ���� ���� ����        

        monSkIllList.Clear();  // ���� ��ų����Ʈ �ʱ�ȭ
        switch (MonsterTable.Instance.MonsterNum)
        {
            case 0: // ��������� ��
                monSkIllList.Add(SkillTable.Instance.demonSlayerSlash);
                monSkIllList.Add(SkillTable.Instance.demonSlayerStabbing);
                monSkIllList.Add(SkillTable.Instance.demonSlayerSwordsmanship);
                break;
            case 1:
                monSkIllList.Add(SkillTable.Instance.demonArcherArrowShot);
                monSkIllList.Add(SkillTable.Instance.demonArcherDouble);
                monSkIllList.Add(SkillTable.Instance.demonArcherDarkArrow);
                break;
            case 2:
                monSkIllList.Add(SkillTable.Instance.demonShamanStunBall);
                monSkIllList.Add(SkillTable.Instance.demonShamanEnergyBolt);
                monSkIllList.Add(SkillTable.Instance.demonShamanDarkLightning);
                break;
        }
    }

    public void Update()
    {                
        nowMonsterHpBar.fillAmount = Mathf.Lerp(nowMonsterHpBar.fillAmount, nowMonsterHp / nowMonsterMaxHp, Time.deltaTime * 5f);                    
    }

    public async void MonsterDamaged(float DamageAmount)
    {        
        nowMonsterHp -= DamageAmount;
        monsterAni.SetTrigger("IsHit");
        if (nowMonsterHp <= 0) // �¸��Ͽ��� �� ������ �����Ƿ� ��ǻ� ������ ������ ����� �ڵ�� �������
        {
            await Task.Delay(200);
            GameManager.Instance.IsMonsterDead = true; // ������ ü���� 0, �� ������ �÷��� true
            GameManager.Instance.IsAni = true;
            GameManager.Instance.LoadMainScene();
            await Task.Delay(100);
            UiManager.Instance.ItemSelectUI.gameObject.SetActive(true);
        }
    }

    public IEnumerator MonsterSingleDamaged(MonsterController nowMonster, BaseSkill Skill, string SkillType)
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
            BattleManager.Instance.BattleDialogText.text += $"\n\nġ��Ÿ!! {Skill.Name} ��ų ����! \n{BattleManager.Instance.PlayerCriAttackAmount} ��ŭ ���ظ� ������!";
            BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, BattleManager.Instance.PlayerCriAttackAmount, BattleManager.Instance.SkillCount);
        }
        else
        {
            nowMonster.MonsterDamaged(BattleManager.Instance.PlayerAttackAmount);
            BattleManager.Instance.BattleDialogText.text += $"\n\n{Skill.Name}! \n{BattleManager.Instance.PlayerAttackAmount} ��ŭ ���ظ� ������!";
            BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, BattleManager.Instance.PlayerAttackAmount, BattleManager.Instance.SkillCount);
        }
        yield return null;
    }

    public IEnumerator MonsterMultiDamaged(MonsterController nowMonster, BaseSkill Skill, int SkillTimes, string SkillType)
    {
        for (int i = 0; i < SkillTimes; i++)
        {
            // ��ų����Ʈ ����
            if (SkillType == "Physical")
            {
                BattleManager.Instance.PlayerPhysicalHitEffectList[i].gameObject.SetActive(false);
                BattleManager.Instance.PlayerPhysicalHitEffectList[i].gameObject.SetActive(true);
            }
            else
            {
                BattleManager.Instance.PlayerMagicHitEffectList[i].gameObject.SetActive(false);
                BattleManager.Instance.PlayerMagicHitEffectList[i].gameObject.SetActive(true);
            }

            if (BattleManager.Instance.CriAttack(BattleManager.Instance.nowmonster.nowMonsterCri)) // ġ��Ÿ �����̶��
            {
                nowMonster.MonsterDamaged(BattleManager.Instance.PlayerCriAttackAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\nġ��Ÿ!! {Skill.Name} ��ų ����! \n{BattleManager.Instance.PlayerCriAttackAmount} ��ŭ ���ظ� ������!";
                BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, BattleManager.Instance.PlayerCriAttackAmount, BattleManager.Instance.SkillCount);
            }
            else
            {
                nowMonster.MonsterDamaged(BattleManager.Instance.PlayerAttackAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\n{Skill.Name}! \n{BattleManager.Instance.PlayerAttackAmount} ��ŭ ���ظ� ������!";
                BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, BattleManager.Instance.PlayerAttackAmount, BattleManager.Instance.SkillCount);
            }
            BattleManager.Instance.SkillCount += 1;
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void startMonsterSingleDamaged(BaseSkill Skill, string SkillType)
    {
        StartCoroutine(MonsterSingleDamaged(BattleManager.Instance.nowmonster, Skill, SkillType));
    }

    public void startMonsterMultiDamaged(BaseSkill Skill, int SkillTimes, string SkillType)
    {
        StartCoroutine(MonsterMultiDamaged(BattleManager.Instance.nowmonster, Skill, SkillTimes, SkillType));
    }


    public void MonsterSet(int MonsterNum)
    {
        Monster nowMonster = MonsterTable.Instance.MonsterList[MonsterNum];
        nowMonsterSprite.sprite = nowMonster.MonsterSprite;
        nowMonsterName = nowMonster.MonsterName;
        nowMonsterHp = nowMonster.MonsterHp;
        nowMonsterMaxHp = nowMonster.MonsterMaxHp;
        nowMonsterAtk = nowMonster.MonsterAtk;
        nowMonsterDodge = nowMonster.MonsterDodge;
        nowMonsterCri = nowMonster.MonsterCri;
        nowMonsterStunStack = nowMonster.MonsterStunStack;
        monsterNameText.text = nowMonster.MonsterName;        

        // ���� ����â UI
        monsterInfoNameText.text = nowMonster.MonsterName;
        MonsterImage.sprite = nowMonster.MonsterSprite;
        monsterHpText.text = $"ü�� : {nowMonster.MonsterMaxHp}";
        monsterAtkText.text = $"���ݷ� : {nowMonster.MonsterAtk}";
        monsterCriText.text = $"ġ��Ÿ : {nowMonster.MonsterCri}%";
        monsterDodText.text = $"ȸ���� : {nowMonster.MonsterDodge}%";
    }

    public void UseMonsterSkill() // ��Ÿ��ų�� �ƴ� Ÿ���� 2���̻��� ��ų ����Լ�
    {
        float DefenseAmount = PlayerTable.Instance.NowDefense - nowMonsterAtk * BattleManager.Instance.monsterSkill.SkillPercentage <= 0 ?
            PlayerTable.Instance.NowDefense : nowMonsterAtk * BattleManager.Instance.monsterSkill.SkillPercentage;
        float AttackAmount = PlayerTable.Instance.NowDefense - nowMonsterAtk * BattleManager.Instance.monsterSkill.SkillPercentage <= 0 ?
            nowMonsterAtk * BattleManager.Instance.monsterSkill.SkillPercentage - PlayerTable.Instance.NowDefense : 0;
        float CriDefenseAmount = PlayerTable.Instance.NowDefense - nowMonsterAtk * BattleManager.Instance.monsterSkill.SkillPercentage * BattleManager.Instance.monsterSkill.CriMultiple <= 0 ?
            PlayerTable.Instance.NowDefense : nowMonsterAtk * BattleManager.Instance.monsterSkill.SkillPercentage * BattleManager.Instance.monsterSkill.CriMultiple;
        float CriAttackAmount = PlayerTable.Instance.NowDefense - nowMonsterAtk * BattleManager.Instance.monsterSkill.SkillPercentage * BattleManager.Instance.monsterSkill.CriMultiple <= 0 ?
            nowMonsterAtk * BattleManager.Instance.monsterSkill.SkillPercentage * BattleManager.Instance.monsterSkill.CriMultiple - PlayerTable.Instance.NowDefense : 0;

        

        if (BattleManager.Instance.CriAttack(nowMonsterCri)) // ġ��Ÿ �����̶��
        {
            BattleManager.Instance.nowplayer.playerDamaged(CriAttackAmount);
            BattleManager.Instance.BattleDialogText.text += $"\nġ��Ÿ! {CriAttackAmount} ��ŭ ���ظ� �Ծ���! ({CriDefenseAmount})�����";
            BattleManager.Instance.FloatingText(BattleManager.Instance.PlayerDamageTextList, CriAttackAmount, BattleManager.Instance.SkillCount);
        }
        else
        {
            BattleManager.Instance.nowplayer.playerDamaged(AttackAmount);
            BattleManager.Instance.BattleDialogText.text += $"\n {AttackAmount} ��ŭ ���ظ� �Ծ���! ({DefenseAmount})�����";
            BattleManager.Instance.FloatingText(BattleManager.Instance.PlayerDamageTextList, AttackAmount, BattleManager.Instance.SkillCount);
        }
        BattleManager.Instance.SkillCount += 1;
    }
}
 