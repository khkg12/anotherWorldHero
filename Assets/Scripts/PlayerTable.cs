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
    
    public float MaxHp // 프로퍼티로 하는 이유는 한계치를 정하기위해 
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
                _Hp = MaxHp; // 체력회복 시 현재 HP가 MAXHP보다 많다면 MAXHP로 고정
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

    // 캐릭터 특성 
    public int IronBody // 철통 : 매턴 일정방어력을 가지는 쉴드생성
    {
        get => _IronBody;
        set
        {
            _IronBody = value;
            if(value >= 5)
            {
                _IronBody = 5; // 최대레벨 5
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

    public int Scare // 공포 : 몬스터 공격력 감소
    {
        get => _Scare;
        set
        {
            _Scare = value;
            if (value >= 5) 
            {
                _Scare = 5; // 최대레벨 5
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

    public int FightingSpirit // 투지 : 3턴시마다 공격력 증가
    {
        get => _FightingSpirit;
        set
        {
            _FightingSpirit = value;
            if (value >= 5)
            {
                _FightingSpirit = 5; // 최대레벨 5
            }
            if (SceneManager.GetActiveScene().name == "BattleScene") // if 현재 씬이 BattleScene일 때만
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

    public int WillPower //  의지 : cc기 일정기회 무시
    {
        get => _WillPower;
        set
        {
            _WillPower = value;
            if (value >= 5)
            {
                _WillPower = 5; // 최대레벨 5
            }
            if (SceneManager.GetActiveScene().name == "BattleScene") // if 현재 씬이 BattleScene일 때만
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
    public List<int> SkillAvailableCount; // 스킬사용가능횟수, 공격은 변하지 않기 때문에 [0]은사용하지않음
    public List<int> SkillFixedCount; // 값 비교를 위한 스킬사용횟수 고정값    
}

[System.Serializable]
public class StatusText
{
    public string StatusName;
    public string OptionText;
    public string[] LevelText;      
}





