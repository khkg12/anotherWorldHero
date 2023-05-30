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
        MonsterSet(MonsterTable.Instance.MonsterNum); // 몬스터 스탯 설정        

        monSkIllList.Clear();  // 몬스터 스킬리스트 초기화
        switch (MonsterTable.Instance.MonsterNum)
        {
            case 0: // 마족기사일 때
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
            case 3:
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
        if (nowMonsterHp <= 0 && IsMonsterBoss == false) // 승리하였을 때 전투가 끝나므로 사실상 전투가 끝나고 실행될 코드들 집어넣음
        {
            await Task.Delay(200);                
            GameManager.Instance.IsMonsterDead = true; // 몬스터의 체력이 0, 즉 죽으면 플래그 true
            GameManager.Instance.IsAni = true;
            GameManager.Instance.LoadMainScene();
            await Task.Delay(100);
            UiManager.Instance.ItemSelectUI.gameObject.SetActive(true);
        }
        else if(nowMonsterHp <= 0 && IsMonsterBoss == true)
        {            
            // 몬스터 사망 애니
            await Task.Delay(500);
            GameManager.Instance.LoadMainScene();
            GameManager.Instance.AfterVictory();
            // nextDialog를 실행시켜야함 어떻게실행시킬지?
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

        if (BattleManager.Instance.CriAttack(PlayerTable.Instance.Critical)) // 치명타 공격이라면
        {
            nowMonster.MonsterDamaged(BattleManager.Instance.PlayerCriAttackAmount);
            BattleManager.Instance.BattleDialogText.text += $"\n\n치명타!! {Skill.Name}! {BattleManager.Instance.PlayerCriAttackAmount} 피해입힘!";
            BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, BattleManager.Instance.PlayerCriAttackAmount, BattleManager.Instance.SkillCount);
        }
        else
        {
            nowMonster.MonsterDamaged(BattleManager.Instance.PlayerAttackAmount);
            BattleManager.Instance.BattleDialogText.text += $"\n\n{Skill.Name}! {BattleManager.Instance.PlayerAttackAmount} 피해입힘!";
            BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, BattleManager.Instance.PlayerAttackAmount, BattleManager.Instance.SkillCount);
        }
        yield return null;
    }

    public IEnumerator MonsterMultiDamaged(MonsterController nowMonster, BaseSkill Skill, int SkillTimes, string SkillType)
    {
        for (int i = 0; i < SkillTimes; i++)
        {
            // 스킬이펙트 실행
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

            if (BattleManager.Instance.CriAttack(BattleManager.Instance.nowmonster.nowMonsterCri)) // 치명타 공격이라면
            {
                nowMonster.MonsterDamaged(BattleManager.Instance.PlayerCriAttackAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\n치명타!! {Skill.Name}! {BattleManager.Instance.PlayerCriAttackAmount} 피해입힘!";
                BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, BattleManager.Instance.PlayerCriAttackAmount, BattleManager.Instance.SkillCount);
            }
            else
            {
                nowMonster.MonsterDamaged(BattleManager.Instance.PlayerAttackAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\n{Skill.Name}! {BattleManager.Instance.PlayerAttackAmount} 피해입힘!";
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
        IsMonsterBoss = nowMonster.IsMonsterBoss;
        // 몬스터 정보창 UI
        monsterInfoNameText.text = nowMonster.MonsterName;
        MonsterImage.sprite = nowMonster.MonsterSprite;
        monsterHpText.text = $"체력 : {nowMonster.MonsterMaxHp}";
        monsterAtkText.text = $"공격력 : {nowMonster.MonsterAtk}";
        monsterCriText.text = $"치명타 : {nowMonster.MonsterCri}%";
        monsterDodText.text = $"회피율 : {nowMonster.MonsterDodge}%";
    }
}
 