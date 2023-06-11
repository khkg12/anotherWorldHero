using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

public class MonsterClass : MonoBehaviour
{
    // �̸� �ٲܰ�
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
        Debug.Log("Damaged����Ϸ�");
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

    public IEnumerator MonsterDamaged(BaseSkill Skill)
    {
        Debug.Log("��ù�ڵ� MonsterDamaged����Ϸ�");
        int SkillCount = 0;
        var skillData = new SkillData() // ��ų�� �޴� �ʿ��� ����������, �ʿ��Ѱ͸� ����
        {
            Name = Skill.Name,
            Damage = (int)(target.atk * Skill.SkillPercentage),
            CriDamage = (int)(target.atk * Skill.SkillPercentage * Skill.CriMultiple)
        };        
        for (int i = 0; i < Skill.SkillTimes; i++)
        {
            // ��ų����Ʈ ����
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
            if (bm.Instance.CriAttack(target.critical)) // ġ��Ÿ �����̶��
            {
                Damaged(skillData.CriDamage);
                battleDialogController.PlayerAddText(BattleType.CriAttack, skillData);                
                bm.Instance.FloatingText(bm.Instance.MonsterDamageTextList, skillData.CriDamage, SkillCount);
            }
            else
            {
                Debug.Log("MonsterDamaged����Ϸ�");
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
        Debug.Log("����Ϸ�");
        StartCoroutine(MonsterDamaged(Skill));
    }
}
