using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

public class MonsterClass : MonoBehaviour
{
    // 이름 바꿀것
    private Sprite nowMonsterSprite;
    private string nowMonsterName;
    public float nowMonsterHp;
    public float nowMonsterMaxHp;    
    private int nowMonsterCri;
    private int nowMonsterStunStack;
    private bool IsMonsterBoss;
    private float nowMonsterAtk
    {
        get => _nowMonsterAtk;
        set
        {
            _nowMonsterAtk = value <= 0 ? 0 : value;
        }
    }
    private float _nowMonsterAtk;    
    private List<MonsterSkill> monSkIllList;    
    [SerializeField] private BattleDialogController battleDialogController;

    private Character target;

    public int nowMonsterDodge;
    public Animator monsterAni;


    public void Initialize(List<Monster> monsterList, int MonsterNum, Character Target)
    {
        Monster nowMonster = monsterList[MonsterNum];
        nowMonsterSprite = nowMonster.MonsterSprite;
        nowMonsterName = nowMonster.MonsterName;
        nowMonsterHp = nowMonster.MonsterHp;
        nowMonsterMaxHp = nowMonster.MonsterMaxHp;
        nowMonsterAtk = nowMonster.MonsterAtk;
        nowMonsterDodge = nowMonster.MonsterDodge;
        nowMonsterCri = nowMonster.MonsterCri;
        nowMonsterStunStack = nowMonster.MonsterStunStack;        
        IsMonsterBoss = nowMonster.IsMonsterBoss;
        monSkIllList = nowMonster.monsterSkillList;        
        this.target = Target;        
    }

    public async void Damaged(float DamageAmount)
    {
        Debug.Log("Damaged실행완료");
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

    public IEnumerator MonsterDamaged(BaseSkill Skill)
    {
        Debug.Log("맨첫코드 MonsterDamaged실행완료");
        int SkillCount = 0;
        var skillData = new SkillData() // 스킬을 받는 쪽에서 데이터취합, 필요한것만 정의
        {
            Name = Skill.Name,
            Damage = (int)(target.atk * Skill.SkillPercentage),
            CriDamage = (int)(target.atk * Skill.SkillPercentage * Skill.CriMultiple)
        };        
        for (int i = 0; i < Skill.SkillTimes; i++)
        {
            // 스킬이펙트 실행
            if (Skill.SkillType == "Physical")
            {
                bm.Instance.MonsterPhysicalHitEffectList[i].gameObject.SetActive(false);
                bm.Instance.MonsterPhysicalHitEffectList[i].gameObject.SetActive(true);
            }
            else
            {
                bm.Instance.MonsterMagicHitEffectList[i].gameObject.SetActive(false);
                bm.Instance.MonsterMagicHitEffectList[i].gameObject.SetActive(true);
            }
            if (bm.Instance.CriAttack(target.critical)) // 치명타 공격이라면
            {
                Damaged(skillData.CriDamage);
                battleDialogController.PlayerAddText(BattleType.CriAttack, skillData);                
                bm.Instance.FloatingText(bm.Instance.MonsterDamageTextList, skillData.CriDamage, SkillCount);
            }
            else
            {
                Debug.Log("MonsterDamaged실행완료");
                Damaged(skillData.Damage);
                battleDialogController.PlayerAddText(BattleType.Attack, skillData);                
                bm.Instance.FloatingText(bm.Instance.MonsterDamageTextList, skillData.Damage, SkillCount);
            }
            SkillCount += 1;
            yield return new WaitForSeconds(0.2f);
        }
        yield return null;
    }

    public void startMonsterDamaged(BaseSkill Skill)
    {
        Debug.Log("실행완료");
        StartCoroutine(MonsterDamaged(Skill));
    }
}
