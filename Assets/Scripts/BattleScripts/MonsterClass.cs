using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class MonsterClass : MonoBehaviour
{
    // �̸� �ٲܰ�
    [SerializeField] private SpriteRenderer nowMonsterSprite;
    private string nowMonsterName;
    public float nowMonsterHp; // mc���� ü�¹ٸ� �����ϱ� ������ ��¿������ public
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
        SelectMonsterSkill(); // ���ӽ��� �� mosterSkill �������� ä��
        battleDialogController.ActionAddText(monsterSkill); // ���Ͱ� ���������� ���� �ؽ�Ʈ ���
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
        if (nowMonsterHp <= 0 && IsMonsterBoss == false) // �¸��Ͽ��� �� ������ �����Ƿ� ��ǻ� ������ ������ ����� �ڵ�� �������
        {
            await Task.Delay(200);
            GameManager.Instance.IsMonsterDead = true; // ������ ü���� 0, �� ������ �÷��� true
            GameManager.Instance.IsAni = true;
            GameManager.Instance.LoadMainScene();
            await Task.Delay(100);
            UiManager.Instance.ItemSelectUI.gameObject.SetActive(true);
        }
        else if (nowMonsterHp <= 0 && IsMonsterBoss == true)
        {
            // ���� ��� �ִ�
            await Task.Delay(500);
            GameManager.Instance.AfterVictory();
            // nextDialog�� ������Ѿ��� ��Խ����ų��?
        }
    }

    public IEnumerator TakeDamaged(MonsterSkill Skill)
    {               
        int SkillCount = 0;
        int Damage = (int)(nowMonsterAtk * Skill.SkillPercentage);
        int CriDamage = (int)(nowMonsterAtk * Skill.SkillPercentage * Skill.CriMultiple);

        var skillData = new SkillData() // ��ų�� �޴� �ʿ��� ����������, �ʿ��Ѱ͸� ����
        {
            Name = Skill.Name,
            Damage = target.DefenseAmount - Damage <= 0 ? Damage - target.DefenseAmount : 0,
            CriDamage = target.DefenseAmount - CriDamage <= 0 ? CriDamage - target.DefenseAmount : 0,
            Defense = target.DefenseAmount - Damage <= 0 ? target.DefenseAmount : Damage,
            CriDenfense = target.DefenseAmount - CriDamage <= 0 ? target.DefenseAmount : CriDamage,            
        };

        if (target.Dodge()) // �÷��̾ ȸ��
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
                if (bm.Instance.CriAttack(nowMonsterCri)) // ġ��Ÿ �����̶��
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

            if (Skill.StunCount > 0) // ��ų�� ����ȿ���� �ִ� ��ų�̶��
            {
                target.StunStack += 1; // �÷��̾��� ���������� �ϳ� �ø���
                if (target.StunStack <= 0) // �÷��̾��� ���������� 0���϶�� ���������� �ʾҴٴ� ��
                {
                    battleDialogController.MonsterAddText(BattleType.EndureStun, skillData); // ������ �ߵ� �ؽ�Ʈ �߰�
                }
                else
                {
                    battleDialogController.MonsterAddText(BattleType.GetStun, skillData); // ���� ���ݿ� ����, ���� �ٷ� ���ͽ�ų����
                    target.DefenseAmount = 0; // �������°� �Ǿ� ö�배�� Ư���� ������ �ȵǰų�, ö��Ư���� �����ϰų�
                    SelectMonsterSkill(); // �������� ��������
                    yield return new WaitForSeconds(0.8f);
                    UseSkill(); // ��ų����
                    yield break; // �������ؼ� �������� ������ ��ų�� ����� �� �Ʒ��� �ڵ�� ����Ǹ� �ȵǹǷ� break����
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
        }; //�ִ� �ʿ��� ������ ������ �����Ͽ� ����
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
