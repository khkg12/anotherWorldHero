using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;

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

    public void UsePlayerSkill() // 단타스킬이 아닌 타수가 2개이상인 스킬 사용함수
    {
        float DamageAmount = PlayerTable.Instance.NowAtk * BattleManager.Instance.playerSkill.SkillPercentage;
        float CriDamageAmount = DamageAmount * BattleManager.Instance.playerSkill.CriMultiple;
        
        if (BattleManager.Instance.CriAttack(PlayerTable.Instance.Critical)) // 치명타 공격이라면
        {
            BattleManager.Instance.nowmonster.MonsterDamaged(CriDamageAmount);
            BattleManager.Instance.BattleDialogText.text += $"\n치명타!! {CriDamageAmount} 만큼 피해를 입혔다!";
            BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, CriDamageAmount, BattleManager.Instance.SkillCount);                       
        }
        else
        {
            BattleManager.Instance.nowmonster.MonsterDamaged(DamageAmount);
            BattleManager.Instance.BattleDialogText.text += $"\n{DamageAmount} 만큼 피해를 입혔다!";
            BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, DamageAmount, BattleManager.Instance.SkillCount);      
        }
        BattleManager.Instance.SkillCount += 1;
    }   

    public void playerResurrection() // 플레이어 부활
    {
        PlayerTable.Instance.Hp = 0.5f * PlayerTable.Instance.MaxHp;
        PlayerTable.Instance.ResChance -= 1;
    }        
}


