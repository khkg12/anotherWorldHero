using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Runtime;

public class mc : MonoBehaviour
{
    [SerializeField] private Character Target;

    [SerializeField] private Image nowMonsterHpBar;    
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

    private List<Monster> monsterList;

    private int PoisonDotCount;
    private int FireDotCount;
    private MonsterClass monster;

    private void Awake()
    {
        /*
        switch (GameManager.Instance.NowAct)
        {
            case 1:
                monsterList = MonsterTable.Instance.MonsterList;
                break;
            case 2:
                monsterList = MonsterTable.Instance.SecondActMonsterList;
                break;
            case 3:
                monsterList = MonsterTable.Instance.ThirdActMonsterList;
                break;
            case 4:
                monsterList = MonsterTable.Instance.FourthActMonsterList;
                break;
        }*/
        monsterList = MonsterTable.Instance.MonsterList; // ���� �ּ� Ȱ��ȭ�ϰ� ���ڵ� �����
        monster = GetComponent<MonsterClass>();  
    }

    private void Start()
    {
        monster.Initialize(monsterList, MonsterTable.Instance.MonsterNum, Target);
        MonsterUISet(MonsterTable.Instance.MonsterNum);        
    }

    public void Update()
    {
        nowMonsterHpBar.fillAmount = Mathf.Lerp(nowMonsterHpBar.fillAmount, monster.nowMonsterHp / monster.nowMonsterMaxHp, Time.deltaTime * 5f);
    }
    

    public void MonsterUISet(int MonsterNum)
    {
        Monster nowMonster = monsterList[MonsterNum];
        // ���� ����â UI
        monsterNameText.text = nowMonster.MonsterName;
        monsterInfoNameText.text = nowMonster.MonsterName;
        MonsterImage.sprite = nowMonster.MonsterSprite;
        monsterHpText.text = $"ü�� : {nowMonster.MonsterMaxHp}";
        monsterAtkText.text = $"���ݷ� : {nowMonster.MonsterAtk}";
        monsterCriText.text = $"ġ��Ÿ : {nowMonster.MonsterCri}%";
        monsterDodText.text = $"ȸ���� : {nowMonster.MonsterDodge}%";
    }

}
