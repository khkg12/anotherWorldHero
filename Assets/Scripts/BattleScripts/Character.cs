using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using JetBrains.Annotations;

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
    private int atk;
    private int critical;
    private int dodge;
    private int defense;
    private bool MonsterStun;    

    [SerializeField] private BattleDialogController battleDialogController;
    public Animator characterAni;
    public List<BaseSkill> skillList;
    public int DefenseAmount;
    public int StunStack;
    public bool SkillEnd;
    public bool TurnFree;

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
        var skillData = new SkillData() { Name = skill.Name }; // 이름만 정보줌, 받는쪽에서 정보취합
        
        if (skill.CoolTime > 0 && bm.Instance.SkillCoolTime[index] == 0) // 쿨타임 존재하는 스킬이라면
        {
            bm.Instance.SkillCoolTime[index] += skill.CoolTime; // 쿨타임 추가
            bm.Instance.CoolTimeImage[index].gameObject.SetActive(true); // 쿨타임 이미지 띄우기
        }        

        skill.SkillUse(this);

        yield return new WaitForSeconds(1f);
        if (TurnFree == true) // 턴소모하지 않는 스킬을 사용 후 로직종료
        {
            TurnFree = false;
            battleDialogController.LineAddText(); // 무엇을 할까만 띄우기
            pc.BtnEnableAction(); // 버튼활성화       
            yield break;
        }

        if(MonsterStun == true) 
        {
            MonsterStun = false;            
        }
        else
        {
            target.UseSkill(); // 몬스터 스킬함수 실행                            
        }

        yield return new WaitUntil(() => SkillEnd == true); // 스킬이 끝났을 때
        SkillEnd = false;
        target.SelectMonsterSkill(); // 몬스터의 다음스킬 정하고
        battleDialogController.ActionAddText(target.monsterSkill); // 띄우기
        pc.BtnEnableAction(); // 버튼활성화                                             
        DefenseAmount = 0; // 플레이어의 방어도 0으로 초기화
        yield break;
    }

    public async void Damaged(float DamageAmount)
    {
        Debug.Log("체력"+PlayerTable.Instance.Hp);
        PlayerTable.Instance.Hp -= DamageAmount;
        characterAni.SetTrigger("IsHit");
        if (PlayerTable.Instance.Hp <= 0)
        {
            await Task.Delay(100);
            BattleManager.Instance.ResurrectionUI.gameObject.SetActive(true);
        }
    } // 플레이어한테 대미지주는 함수

    public IEnumerator TakeDamaged(BaseSkill Skill)
    {        
        int SkillCount = 0;
        var skillData = new SkillData() // 스킬을 받는 쪽에서 데이터취합, 필요한것만 정의, 캐싱하기
        {
            Name = Skill.Name,
            Damage = (int)(atk * Skill.SkillPercentage),
            CriDamage = (int)(atk * Skill.SkillPercentage * Skill.CriMultiple)
        };

        if (target.Dodge()) // 타겟, 즉 몬스터가 회피에 성공했다면
        {
            target.monsterAni.SetTrigger("IsDodge");
            battleDialogController.PlayerAddText(BattleType.Dodge, skillData);
        }
        else
        {
            characterAni.SetTrigger("IsAttack"); // 공격스킬 애니 실행
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
                    SkillEnd = true; // 몬스터가 기절하므로 여기서 스킬로직을 끝내줘야함
                    yield break;
                }
            }
        }        
        yield break;
    } 

    public void startTakeDamaged(BaseSkill Skill)
    {        
        StartCoroutine(TakeDamaged(Skill));
    }  

    // 방어스킬 함수
    public void TakeDefense(BaseSkill Skill)
    {
        characterAni.SetTrigger("IsDefense");
        DefenseAmount += (int)(Skill.SkillPercentage * defense); // defense public으로 접근할지 고민해볼것
        var skillData = new SkillData() // 스킬을 받는 쪽에서 데이터취합, 필요한것만 정의, 캐싱하기
        {
            Name = Skill.Name,
            Defense = DefenseAmount,
        };        
        battleDialogController.PlayerAddText(BattleType.GetShield, skillData);        
    }
    
    // 버프스킬 함수
    public void TakeBuff()
    {
        
    }
       
    public bool Dodge()
    {
        return bm.Instance.DodgeSucess(dodge);
    }    
}
