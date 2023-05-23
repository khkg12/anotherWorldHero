using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;
using System.Threading;
using System.Xml.Linq;

public class PlayerController : MonoBehaviour
{
    public Image nowPlayerHpBar;    
    public TextMeshProUGUI nowHpText;
    public TextMeshProUGUI maxHpText;
    public Animator playerAni;       
    public Button PlayerInfoBtn;

    public GameObject StunEffect;
    public TextMeshProUGUI StunText;

    private void Start()
    {
        nowPlayerHpBar.fillAmount = PlayerTable.Instance.Hp / PlayerTable.Instance.MaxHp;        
        nowHpText.text = $"{PlayerTable.Instance.Hp}";
        maxHpText.text = $"/ {PlayerTable.Instance.MaxHp}";
        PlayerInfoBtn.onClick.AddListener(() => GameManager.Instance.PlayerInfoUI.gameObject.SetActive(true));
    }    

    private void Update()
    {        
        nowPlayerHpBar.fillAmount = Mathf.Lerp(nowPlayerHpBar.fillAmount, PlayerTable.Instance.Hp / PlayerTable.Instance.MaxHp, Time.deltaTime * 5f);
        nowHpText.text = $"{PlayerTable.Instance.Hp}";                            
    }

    public async void playerDamaged(float DamageAmount)
    {
        double Damage = System.Math.Round(DamageAmount, 2);
        PlayerTable.Instance.Hp -= (float)Damage;
        playerAni.SetTrigger("IsHit");
        if (PlayerTable.Instance.Hp <= 0)
        {
            await Task.Delay(100);
            BattleManager.Instance.ResurrectionUI.gameObject.SetActive(true);
        }
    }

    public IEnumerator PlayerSingleDamaged(MonsterController nowMonster, string SkillName, string SkillType)
    {
        if (SkillType == "Physical")
        {
            BattleManager.Instance.PlayerPhysicalHitEffectList[2].gameObject.SetActive(false);
            BattleManager.Instance.PlayerPhysicalHitEffectList[2].gameObject.SetActive(true);
        }
        else
        {
            BattleManager.Instance.PlayerMagicHitEffectList[2].gameObject.SetActive(false);
            BattleManager.Instance.PlayerMagicHitEffectList[2].gameObject.SetActive(true);
        }

        if (BattleManager.Instance.CriAttack(BattleManager.Instance.nowmonster.nowMonsterCri)) // 치명타 공격이라면
        {
            playerDamaged(BattleManager.Instance.CriAttackAmount);
            BattleManager.Instance.BattleDialogText.text += $"\n치명타!! 적의 {SkillName}! {BattleManager.Instance.CriAttackAmount}피해! ({BattleManager.Instance.CriDefenseAmount}방어)";
            BattleManager.Instance.FloatingText(BattleManager.Instance.PlayerDamageTextList, BattleManager.Instance.CriAttackAmount, BattleManager.Instance.SkillCount);
        }
        else
        {
            playerDamaged(BattleManager.Instance.AttackAmount);
            BattleManager.Instance.BattleDialogText.text += $"\n적의 {SkillName}! {BattleManager.Instance.AttackAmount}피해! ({BattleManager.Instance.DefenseAmount}방어)";
            BattleManager.Instance.FloatingText(BattleManager.Instance.PlayerDamageTextList, BattleManager.Instance.AttackAmount, BattleManager.Instance.SkillCount);
        }
        yield return null;  
    }

    public IEnumerator PlayerMultiDamaged(MonsterController nowMonster, string SkillName, int SkillTimes, string SkillType)
    {
        for(int i = 0; i < SkillTimes; i++)
        {
            // 스킬이펙트 실행
            if(SkillType == "Physical")
            {
                BattleManager.Instance.PlayerPhysicalHitEffectList[i].gameObject.SetActive(false);
                BattleManager.Instance.PlayerPhysicalHitEffectList[i].gameObject.SetActive(true);
            }
            else
            {
                BattleManager.Instance.PlayerMagicHitEffectList[i].gameObject.SetActive(false);
                BattleManager.Instance.PlayerMagicHitEffectList[i].gameObject.SetActive(true);
            }

            if (BattleManager.Instance.CriAttack(BattleManager.Instance.nowmonster.nowMonsterCri)) // 치명타 공격이라면
            {
                playerDamaged(BattleManager.Instance.CriAttackAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n치명타!! 적의 {SkillName}! {BattleManager.Instance.CriAttackAmount}피해! ({BattleManager.Instance.CriDefenseAmount}방어)";
                BattleManager.Instance.FloatingText(BattleManager.Instance.PlayerDamageTextList, BattleManager.Instance.CriAttackAmount, BattleManager.Instance.SkillCount);
            }
            else
            {
                playerDamaged(BattleManager.Instance.AttackAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n적의 {SkillName}! {BattleManager.Instance.AttackAmount}피해! ({BattleManager.Instance.DefenseAmount}방어)";
                BattleManager.Instance.FloatingText(BattleManager.Instance.PlayerDamageTextList, BattleManager.Instance.AttackAmount, BattleManager.Instance.SkillCount);
            }
            BattleManager.Instance.SkillCount += 1;
            yield return new WaitForSeconds(0.2f);
        }        
    }

    public void startPlayerSingleDamaged(string SkillName, string SkillType)
    {
        StartCoroutine(PlayerSingleDamaged(BattleManager.Instance.nowmonster, SkillName, SkillType));
    }

    public void startPlayerMultiDamaged(string SkillName, int SkillTimes, string SkillType)
    {
        StartCoroutine(PlayerMultiDamaged(BattleManager.Instance.nowmonster, SkillName, SkillTimes, SkillType));
    }
    
    public void playerResurrection() // 플레이어 부활
    {
        PlayerTable.Instance.Hp = 0.5f * PlayerTable.Instance.MaxHp;
        PlayerTable.Instance.ResChance -= 1;
    }
}


