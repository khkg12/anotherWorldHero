using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

public class SkillData
{
    public string Name;
    public int Damage;
    public int CriDamage;
    public int Defense;
    public int CriDenfense;
}

public class Character : MonoBehaviour
{
    private MonsterClass target;
    public Animator characterAni;
    private int atk;
    private int critical;
    private int dodge;
    private int defense;

    private bool MonsterStun;
    [SerializeField] private BattleDialogController battleDialogController;
    public List<BaseSkill> skillList;
    public int DefenseAmount;
    public int StunStack;

    public void Initialize(MonsterClass Target) // 초기화 함수, Monster는 이 Character클래스와 같이 몬스터를 다룰 스크립트값을 만들고 가져오기
    {
        // hp는 유지되어야하므로 playertable의 hp에 직접할당, 아니면 몬스터가 죽고난뒤 값을 넣어주는것도 고려해볼것        
        atk = (int)PlayerTable.Instance.Atk;
        critical = PlayerTable.Instance.Critical;
        dodge = PlayerTable.Instance.Dodge;
        defense = (int)PlayerTable.Instance.Defense;
        skillList = PlayerTable.Instance.playerSkillList;
        this.target = Target;        
    }    

    public IEnumerator UseSkill(int index)
    {        
        BaseSkill skill = skillList[index];
        var skillData = new SkillData()
        {            
            Name = skill.Name,                        
        }; //주는쪽에서 정보를 모두 취합

        switch (skill.type)
        {
            case Type.Attack: // 스킬이 공격스킬이면 몬스터의 회피에 따라 실행조건을 넣어줘야함                
                    if (target.Dodge()) // 타겟, 즉 몬스터가 회피에 성공했다면
                    {
                        target.monsterAni.SetTrigger("IsDodge");
                        battleDialogController.PlayerAddText(BattleType.Dodge, skillData);                        
                }
                    else
                    {
                        characterAni.SetTrigger("IsAttack"); // 공격스킬 애니 실행
                        skill.SkillUse(this); // 버튼클릭 -> useSkill -> 처리 후 몬스터체력 0이면 종료 -> 아니라면 monsterSkill                    
                    }
                break;
            case Type.Defense:
                characterAni.SetTrigger("IsDefense"); // 방어스킬 애니 실행
                skill.SkillUse(this);
                break;
            case Type.Buff:
                // characterAni.SetTrigger("IsDefense"); 버프스킬 애니 실행
                skill.SkillUse(this);
                break;
        }
        yield return new WaitForSeconds(0.8f);     
        if(MonsterStun == true)
        {
            MonsterStun = false;
            pc.BtnEnableAction(); // 버튼활성화
        }
        else
        {
            target.UseSkill(); // 몬스터 스킬함수 실행                            
        }        
        yield break;
    }

    public async void Damaged(float DamageAmount)
    {
        PlayerTable.Instance.Hp -= DamageAmount;
        characterAni.SetTrigger("IsHit");
        if (PlayerTable.Instance.Hp <= 0)
        {
            await Task.Delay(100);
            BattleManager.Instance.ResurrectionUI.gameObject.SetActive(true);
        }
    }

    public IEnumerator TakeDamaged(BaseSkill Skill)
    {        
        int SkillCount = 0;
        var skillData = new SkillData() // 스킬을 받는 쪽에서 데이터취합, 필요한것만 정의
        {
            Name = Skill.Name,
            Damage = (int)(atk * Skill.SkillPercentage),
            CriDamage = (int)(atk * Skill.SkillPercentage * Skill.CriMultiple)
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
            if (bm.Instance.CriAttack(critical)) // 치명타 공격이라면
            {
                target.Damaged(skillData.CriDamage);
                battleDialogController.PlayerAddText(BattleType.CriAttack, skillData);                
                bm.Instance.FloatingText(bm.Instance.MonsterDamageTextList, skillData.CriDamage, SkillCount);
            }
            else
            {                
                target.Damaged(skillData.Damage);
                battleDialogController.PlayerAddText(BattleType.Attack, skillData);                
                bm.Instance.FloatingText(bm.Instance.MonsterDamageTextList, skillData.Damage, SkillCount);
            }
            SkillCount += 1;
            yield return new WaitForSeconds(0.2f);
        }

        if (Skill.StunCount > 0) // 스킬이 기절효과를 주는 스킬이라면
        {
            target.StunStack += 1; // 타겟의 기절스택을 하나 올리고
            if (target.StunStack <= 0) // 타겟의 기절스택이 0이하라면 기절당하지 않았다는 뜻
            {
                battleDialogController.PlayerAddText(BattleType.EndureStun, skillData); // 기절을 견딤 텍스트 추가
            }
            else
            {
                battleDialogController.PlayerAddText(BattleType.GetStun, skillData); // 몬스터 기절
                yield return new WaitForSeconds(0.5f);
                MonsterStun = true; // 몬스터 스턴 플래그함수
                target.SelectMonsterSkill(); // 몬스터의 다음스킬 정하고
                battleDialogController.ActionAddText(target.monsterSkill); // 띄우기                
                yield break;
            }
        }

        yield return null;
    }

    public void startTakeDamaged(BaseSkill Skill)
    {        
        StartCoroutine(TakeDamaged(Skill));
    }

    public bool Dodge()
    {
        return bm.Instance.DodgeSucess(dodge);
    }    
}
