using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "PlayerTable", menuName = "ScriptableObjects/PlayerTable", order = 2)]
public class PlayerTable : ScriptableObject
{

    public static PlayerTable Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = Resources.Load("PlayerTable") as PlayerTable;
            }
            return _Instance;
        }
    }
    private static PlayerTable _Instance;
    
    public float MaxHp // ������Ƽ�� �ϴ� ������ �Ѱ�ġ�� ���ϱ����� 
    {
        get => _MaxHp;
        set
        {
            _MaxHp = value;            
        }
    }
    public float _MaxHp = 100;

    public float Hp
    {
        get => _Hp;
        set
        {
            if(value > MaxHp)
            {
                _Hp = MaxHp; // ü��ȸ�� �� ���� HP�� MAXHP���� ���ٸ� MAXHP�� ����
            }
            else if(value <= 0)
            {
                _Hp = 0;
            }
            else
            {
                _Hp = value;
            }            
        }
    }
    public float _Hp = 100;

    public float Atk
    {
        get => _Atk;
        set
        {
            _Atk = value;
        }
    }
    public float _Atk = 10;

    public float NowAtk
    {
        get => _NowAtk;
        set
        {            
            _NowAtk = value;
        }
    }
    public float _NowAtk;

    public float Defense
    {
        get => _Defense;
        set
        {
            _Defense = value;
        }
    }
    public float _Defense = 10;

    public float NowDefense;    

    public int Critical
    {
        get => _Critical;
        set
        {
            _Critical = value;
        }
    }
    public int _Critical = 5;

    public int Dodge
    {
        get => _Dodge;
        set
        {
            _Dodge = value;
        }
    }
    public int _Dodge = 5;

    public int StunStack
    {
        get => _StunStack;
        set
        {
            _StunStack = value;
        }
    }

    public int _StunStack = 0;

    public int ResChance
    {
        get => _ResChance;
        set
        {
            _ResChance = value;
        }
    }

    public int _ResChance = 2;

    // ĳ���� Ư�� 
    public int IronBody // ö�� : ���� ���������� ������ �������
    {
        get => _IronBody;
        set
        {
            _IronBody = value;
            if(value >= 5)
            {
                _IronBody = 5; // �ִ뷹�� 5
            }
            if(SceneManager.GetActiveScene().name == "BattleScene")
            {
                switch (value)
                {
                    case 1:
                        NowDefense += 2;
                        break;
                    case 2:
                        NowDefense += 3;
                        break;
                    case 3:
                        NowDefense += 4;
                        break;
                    case 4:
                        NowDefense += 6;
                        break;
                    case 5:
                        NowDefense += 9;
                        break;
                }            
            }
        }
    }
    public int _IronBody = 0;

    public int Scare // ���� : ���� ���ݷ� ����
    {
        get => _Scare;
        set
        {
            _Scare = value;
            if (value >= 5) 
            {
                _Scare = 5; // �ִ뷹�� 5
            }
            if (SceneManager.GetActiveScene().name == "BattleScene") 
            {
                switch (value)
                {
                    case 1:
                        BattleManager.Instance.nowmonster.nowMonsterAtk -= 1;
                        break;
                    case 2:
                        BattleManager.Instance.nowmonster.nowMonsterAtk -= 2;
                        break;
                    case 3:
                        BattleManager.Instance.nowmonster.nowMonsterAtk -= 4;
                        break;
                    case 4:
                        BattleManager.Instance.nowmonster.nowMonsterAtk -= 10;                        
                        break;
                    case 5:
                        BattleManager.Instance.nowmonster.nowMonsterAtk -= 21;
                        break;
                }
            }                
        }
    }
    public int _Scare = 0;

    public int FightingSpirit // ���� : 3�Ͻø��� ���ݷ� ����
    {
        get => _FightingSpirit;
        set
        {
            _FightingSpirit = value;
            if (value >= 5)
            {
                _FightingSpirit = 5; // �ִ뷹�� 5
            }
            if (SceneManager.GetActiveScene().name == "BattleScene") // if ���� ���� BattleScene�� ����
            {
                switch (value)
                {
                    
                    case 1:
                        NowAtk += 1;
                        break;
                    case 2:
                        NowAtk += 1;
                        break;
                    case 3:
                        NowAtk += 2;
                        break;
                    case 4:
                        NowAtk += 3;
                        break;
                    case 5:
                        NowAtk += 4;
                        break;
                }
            }                
        }
    }
    public int _FightingSpirit = 0;

    public int WillPower //  ���� : cc�� ������ȸ ����
    {
        get => _WillPower;
        set
        {
            _WillPower = value;
            if (value >= 5)
            {
                _WillPower = 5; // �ִ뷹�� 5
            }
            if (SceneManager.GetActiveScene().name == "BattleScene") // if ���� ���� BattleScene�� ����
            {
                switch (value)
                {

                    case 1:
                        StunStack -= 1;
                        break;
                    case 2:                        
                        StunStack -= 2;
                        break;
                    case 3:
                        StunStack -= 3;
                        break;
                    case 4:
                        StunStack -= 4;
                        break;
                    case 5:
                        StunStack -= 5;
                        break;
                }
            }
        }
    }
    public int _WillPower = 0;    

    public StatusText scareText;
    public StatusText ironBodyText;
    public StatusText fightingSpiritText;
    public StatusText willPowerText;    

    public List<BaseSkill> playerSkillList;
    public List<int> SkillAvailableCount; // ��ų��밡��Ƚ��, ������ ������ �ʱ� ������ [0]�������������
    public List<int> SkillFixedCount; // �� �񱳸� ���� ��ų���Ƚ�� ������    
}

[System.Serializable]
public class StatusText
{
    public string StatusName;
    public string OptionText;
    public string[] LevelText;      
}





