using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class MonsterClass : MonoBehaviour
{
    // 이름 바꿀것
    [SerializeField] private SpriteRenderer nowMonsterSprite;
    private string nowMonsterName;
    public float nowMonsterHp; // mc에서 체력바를 제어하기 때문에 어쩔수없이 public
    public float nowMonsterMaxHp;    
    private int nowMonsterCri;
    private int nowMonsterStunStack;
    private bool IsMonsterBoss;
    private float nowMonsterAtk;    
    private List<MonsterSkill> monsterSkIllList;
    
    [SerializeField] private BattleDialogController battleDialogController;
    private Character target;
    private int nowMonsterDodge;
    public MonsterSkill monsterSkill;
    public Animator monsterAni;
    public int StunStack;    
    public int DefenseAmount;

    private void Start()
    {        
        SelectMonsterSkill(); // 게임시작 시 mosterSkill 랜덤으로 채택
        battleDialogController.ActionAddText(monsterSkill); // 몬스터가 무슨공격을 할지 텍스트 띄움
    }

    public void Initialize(List<Monster> monsterList, int MonsterNum, Character Target)
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
        IsMonsterBoss = nowMonster.IsMonsterBoss;
        monsterSkIllList = nowMonster.monsterSkillList;        
        this.target = Target;        
    }

    public async void Damaged(float DamageAmount)
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
        else if (nowMonsterHp <= 0 && IsMonsterBoss == true)
        {
            // 몬스터 사망 애니
            await Task.Delay(500);
            GameManager.Instance.AfterVictory();
            // nextDialog를 실행시켜야함 어떻게실행시킬지?
        }
    }

    public IEnumerator TakeDamaged(MonsterSkill Skill)
    {               
        int SkillCount = 0;
        int Damage = (int)(nowMonsterAtk * Skill.SkillPercentage);
        int CriDamage = (int)(nowMonsterAtk * Skill.SkillPercentage * Skill.CriMultiple);

        var skillData = new SkillData() // 스킬을 받는 쪽에서 데이터취합, 필요한것만 정의
        {
            Name = Skill.Name,
            Damage = target.DefenseAmount - Damage <= 0 ? Damage - target.DefenseAmount : 0,
            CriDamage = target.DefenseAmount - CriDamage <= 0 ? CriDamage - target.DefenseAmount : 0,
            Defense = target.DefenseAmount - Damage <= 0 ? target.DefenseAmount : Damage,
            CriDenfense = target.DefenseAmount - CriDamage <= 0 ? target.DefenseAmount : CriDamage,            
        };

        if (target.Dodge()) // 플레이어가 회피
        {
            target.characterAni.SetTrigger("IsDodge");
            battleDialogController.MonsterAddText(BattleType.Dodge, skillData);            
        }
        else
        {
            monsterAni.SetTrigger("IsAttack");
            for (int i = 0; i < Skill.SkillTimes; i++)
            {
                if (Skill.SkillType == "Physical")
                {
                    bm.Instance.PlayerPhysicalHitEffectList[i].gameObject.SetActive(false);
                    bm.Instance.PlayerPhysicalHitEffectList[i].gameObject.SetActive(true);
                }
                else
                {
                    bm.Instance.PlayerMagicHitEffectList[i].gameObject.SetActive(false);
                    bm.Instance.PlayerMagicHitEffectList[i].gameObject.SetActive(true);
                }
                if (bm.Instance.CriAttack(nowMonsterCri)) // 치명타 공격이라면
                {
                    target.Damaged(skillData.CriDamage);
                    battleDialogController.MonsterAddText(BattleType.CriAttack, skillData);
                    bm.Instance.FloatingText(bm.Instance.PlayerDamageTextList, skillData.CriDamage, SkillCount);
                }
                else
                {
                    target.Damaged(skillData.Damage);
                    battleDialogController.MonsterAddText(BattleType.Attack, skillData);
                    bm.Instance.FloatingText(bm.Instance.PlayerDamageTextList, skillData.Damage, SkillCount);
                }
                SkillCount += 1;
                yield return new WaitForSeconds(0.2f);
            }

            if (Skill.StunCount > 0) // 스킬이 기절효과를 주는 스킬이라면
            {
                target.StunStack += 1; // 플레이어의 기절스택을 하나 올리고
                if (target.StunStack <= 0) // 플레이어의 기절스택이 0이하라면 기절당하지 않았다는 뜻
                {
                    battleDialogController.MonsterAddText(BattleType.EndureStun, skillData); // 기절을 견딤 텍스트 추가
                }
                else
                {
                    battleDialogController.MonsterAddText(BattleType.GetStun, skillData); // 적의 공격에 기절, 따라서 바로 몬스터스킬실행
                    target.DefenseAmount = 0; // 무방비상태가 되어 철통같은 특성이 실행이 안되거나, 철통특성만 적용하거나
                    SelectMonsterSkill(); // 랜덤으로 뽑은다음
                    yield return new WaitForSeconds(0.8f);
                    UseSkill(); // 스킬실행
                    yield break; // 기절당해서 연속으로 몬스터의 스킬이 실행될 때 아래의 코드는 실행되면 안되므로 break해줌
                }
            }
        }        
        yield return new WaitForSeconds(0.6f);
        target.SkillEnd = true;
        yield break;
    }

    public void startTakeDamaged(MonsterSkill Skill)
    {        
        StartCoroutine(TakeDamaged(Skill));
    }

    public void UseSkill()
    {
        var skillData = new SkillData()
        {
            Name = monsterSkill.Name,
        }; //주는 쪽에선 간략한 정보만 취합하여 제공
        monsterSkill.SkillUse(this);
    }    

    public bool Dodge()
    {
        return bm.Instance.DodgeSucess(nowMonsterDodge);
    }

    public void SelectMonsterSkill()
    {
        monsterSkill = monsterSkIllList[Random.Range(0, 3)];                
    }
}
