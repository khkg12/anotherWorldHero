using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class bm : MonoBehaviour
{        
    public static bm Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<bm>();
            }
            return _Instance;
        }
    }
    private static bm _Instance;

    //부활 및 포기 버튼 텍스트
    public Image ResurrectionUI;
    public Image ResurrectionConfirmUI;
    public Button ResBtn;
    public Button DiscardBtn;
    public TextMeshProUGUI ResConfirmText;

    //스킬버튼
    /*
    public Button FirstSkillBtn;
    public Image FirstSkillImage;
    public Button SecondSkillBtn;
    public Image SecondSkillImage;
    public Button ThirdSkillBtn;
    public Image ThirdSkillImage;
    public Button FourthSkillBtn;
    public Image FourthSkillImage;
    public Button FifthSkillBtn;
    public Image FifthSkillImage;
    */
    [SerializeField] private BattleDialogController battleDialogController;    
    // 대미지 텍스트    
    public List<TextMeshProUGUI> PlayerDamageTextList;
    public List<TextMeshProUGUI> MonsterDamageTextList;

    public List<GameObject> PlayerPhysicalHitEffectList;
    public List<GameObject> PlayerMagicHitEffectList;

    public List<GameObject> MonsterPhysicalHitEffectList;
    public List<GameObject> MonsterMagicHitEffectList;

    public List<int> SkillCoolTime;
    public List<Image> CoolTimeImage;
    public List<TextMeshProUGUI> CoolTimeText;
    // 배틀라운드
    public int BattleRound;

    // 스킬 사용가능 횟수 텍스트
    [SerializeField] private List<TextMeshProUGUI> SkillCountText;
    


    private void Start()
    {
        /*
        BtnEnable(true);
        BattleRound = 1;
        // PlayerTable에 NowDefense를 두지말고 체력을 두지말고 player스크립트에 변수만들고 전투시작시 현재 playertable에 가지고 있는 값 넣기
        PlayerTable.Instance.NowDefense = 0; // 전투시작 시 방어도 초기화
        PlayerTable.Instance.StunStack = 0; // 전투시작 시 기절스택 초기화        
        PlayerTable.Instance.Scare = PlayerTable.Instance.Scare; // 첫 배틀씬 입장 후 시작시 공포특성 적용        
        PlayerTable.Instance.NowAtk = PlayerTable.Instance.Atk; // 첫 시작시 투지특성인 3턴마다 공격력 증가시킬 NowAtk변수에 전투시에 변동되지않는 Player의 Atk 적용 NowAtk은 전투시에만 사용할 변수
        PlayerTable.Instance.WillPower = PlayerTable.Instance.WillPower; // 첫 시작시 의지특성 부여                        

        //스킬버튼 이벤트
        //for문 
        FirstSkillBtn.onClick.AddListener(() => PressBtn(0));
        SecondSkillBtn.onClick.AddListener(() => PressBtn(1));
        ThirdSkillBtn.onClick.AddListener(() => PressBtn(2));
        FourthSkillBtn.onClick.AddListener(() => PressBtn(3));
        FifthSkillBtn.onClick.AddListener(() => PressBtn(4));

        //부활, 포기, 부활확인 버튼 이벤트 
        ResBtn.onClick.AddListener(() => ResurrectionEvent());
        DiscardBtn.onClick.AddListener(() => GameManager.Instance.LoadDeadScene());

        // 처음 전투를 시작할 때                
        nowmonster.monSkIllList = nowmonster.monSkIllList.OrderBy(i => Random.value).ToList();
        monsterSkill = nowmonster.monSkIllList[0];

        BattleDialogText.text = $"{monsterSkill.SkillText}\n무엇을 할까?";
        
        */
        StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator UpdateCoroutine() // 스킬사용횟수 차감하는 코드다음에 넣기
    {
        while (true)
        {           
            for(int i = 1; i < PlayerTable.Instance.playerSkillList.Count; ++i)
            {
                SkillCountText[i - 1].text = $"{PlayerTable.Instance.SkillAvailableCount[i]} / {PlayerTable.Instance.SkillFixedCount[i]}";
                // i-1은 스킬카운트텍스트는 4개고 스킬은 5개이기때문에 격차메꾸기
                CoolTimeText[i].text = $"{SkillCoolTime[i]}턴";
            }
            yield return new WaitForSeconds(0.1f);
        }
    }



    /*
    IEnumerator BtnClickEvent(int SkillNum)
    {
        playerSkill = PlayerTable.Instance.playerSkillList[SkillNum];        
        PlayerTable.Instance.IronBody = PlayerTable.Instance.IronBody; // 철통 스킬 매턴마다 발동        

        PlayerAttackAmount = PlayerTable.Instance.NowAtk * playerSkill.SkillPercentage; // 플레이어가 몬스터에게 줄 대미지 설정
        PlayerCriAttackAmount = PlayerTable.Instance.NowAtk * playerSkill.SkillPercentage * playerSkill.CriMultiple;

        yield return new WaitForEndOfFrame();
        {
            BtnEnable(false); // 버튼 비활성화                                  
        }

        yield return new WaitForSeconds(0.1f);
        {
            PlayerSkillEvent(playerSkill, SkillNum);
            if (nowmonster.nowMonsterHp <= 0) StopAllCoroutines(); // 몬스터의 체력이 0이 되면 코루틴함수 종료
            DefenseAmount = PlayerTable.Instance.NowDefense - nowmonster.nowMonsterAtk * monsterSkill.SkillPercentage <= 0 ? //몬스터 스킬 함수에서 사용할 변수값들 플레이어스킬쓴뒤 설정
            PlayerTable.Instance.NowDefense : nowmonster.nowMonsterAtk * monsterSkill.SkillPercentage;
            AttackAmount = PlayerTable.Instance.NowDefense - nowmonster.nowMonsterAtk * monsterSkill.SkillPercentage <= 0 ?
                nowmonster.nowMonsterAtk * monsterSkill.SkillPercentage - PlayerTable.Instance.NowDefense : 0;
            CriDefenseAmount = PlayerTable.Instance.NowDefense - nowmonster.nowMonsterAtk * monsterSkill.SkillPercentage * monsterSkill.CriMultiple <= 0 ?
                PlayerTable.Instance.NowDefense : nowmonster.nowMonsterAtk * monsterSkill.SkillPercentage * monsterSkill.CriMultiple;
            CriAttackAmount = PlayerTable.Instance.NowDefense - nowmonster.nowMonsterAtk * monsterSkill.SkillPercentage * monsterSkill.CriMultiple <= 0 ?
                nowmonster.nowMonsterAtk * monsterSkill.SkillPercentage * monsterSkill.CriMultiple - PlayerTable.Instance.NowDefense : 0;
        }

        yield return new WaitForSeconds(1f);
        {
            SkillCount = 0; // 플레이어 스킬 함수에서 skillcount(공유함)를 올리기 때문에 몬스터 스킬 함수 실행 전 초기화시킴
            MonsterSkillEvent(monsterSkill);
            nowmonster.monSkIllList = nowmonster.monSkIllList.OrderBy(i => Random.value).ToList(); // 몬스터 스킬 랜덤 , 몬스터가 스킬을 사용한 직후 다음에 사용할 스킬 정해둠       
            monsterSkill = nowmonster.monSkIllList[0];
        }

        yield return new WaitForSeconds(1f);
        {
            BattleDialogText.text += $"\n{monsterSkill.SkillText}";
            BattleRound += 1; // 어떻게할까가 대화창에 뜨기 직전에 배틀라운드 상승, UI도 추가할것
            if (BattleRound % 3 == 0) // 3턴마다 공격력증가시키는 투지특성 발동
            {
                PlayerTable.Instance.FightingSpirit = PlayerTable.Instance.FightingSpirit;
            }
        }

        yield return new WaitForSeconds(0.1f);
        {
            BattleDialogText.text += $"\n무엇을 할까?";
            BtnEnable(true);
            PlayerTable.Instance.NowDefense = 0; // 턴이 끝나면 방어도를 0으로
            SkillCount = 0;
        }
    }

    public void PlayerSkillEvent(BaseSkill playerSkill, int SkillNum)
    {
        if (PlayerTable.Instance.StunStack >= 1)
        {
            BattleDialogText.text += "\n기절상태! 움직일 수 없다!";
            nowplayer.StunEffect.gameObject.SetActive(true);
            PlayerTable.Instance.StunStack -= 1;
            return;
        }
        else
        {
            switch (SkillNum)
            {
                case 1: // 클릭한 스킬버튼이 2번째일때
                    PlayerTable.Instance.SecondSkillAvailableCount -= 1; // 스킬사용가능횟수를 1 줄이고                
                    if (PlayerTable.Instance.SecondSkillAvailableCount <= 0) // 줄인 뒤 가능횟수가 0이라면 버튼을 비활성화
                    {
                        SecondSkillBtn.interactable = false;
                    }
                    break;
                case 2:
                    PlayerTable.Instance.ThirdSkillAvailableCount -= 1;
                    if (PlayerTable.Instance.SecondSkillAvailableCount <= 0)
                    {
                        ThirdSkillBtn.interactable = false;
                    }
                    break;
                case 3:
                    PlayerTable.Instance.FourthSkillAvailableCount -= 1;
                    if (PlayerTable.Instance.SecondSkillAvailableCount <= 0)
                    {
                        FourthSkillBtn.interactable = false;
                    }
                    break;
                case 4:
                    PlayerTable.Instance.FifthSkillAvailableCount -= 1;
                    if (PlayerTable.Instance.SecondSkillAvailableCount <= 0)
                    {
                        FifthSkillBtn.interactable = false;
                    }
                    break;
            } // 스킬사용가능횟수 조건 체크, 0일경우가 없는 이유는 공격은 횟수제한이 없기때문, 기절상태일땐 사용가능횟수 줄어들지않음
            playerSkill.SkillOption(nowmonster, nowplayer); // 첫번째 버튼을 눌렀을 때 스킬리스트의 첫번째 스킬을 가져와 그 스킬의 옵션실행
        }
    }
    public void MonsterSkillEvent(MonsterSkill monsterSkill)
    {
        if (nowmonster.nowMonsterStunStack >= 1)
        {
            BattleDialogText.text += "\n적은 기절상태로 인해 움직일 수 없다!";
            nowmonster.nowMonsterStunStack -= 1;
            return;
        }
        else
        {
            monsterSkill.SkillOption(nowmonster, nowplayer);
        }
    }
    
    public void BtnEnable(bool isBtnOn)
    {
        FirstSkillBtn.interactable = isBtnOn;
        if (PlayerTable.Instance.SecondSkillAvailableCount > 0) SecondSkillBtn.interactable = isBtnOn;
        if (PlayerTable.Instance.playerSkillCount >= 3 && PlayerTable.Instance.ThirdSkillAvailableCount != 0) ThirdSkillBtn.interactable = isBtnOn;
        if (PlayerTable.Instance.playerSkillCount >= 4 && PlayerTable.Instance.FourthSkillAvailableCount != 0) FourthSkillBtn.interactable = isBtnOn;
        if (PlayerTable.Instance.playerSkillCount >= 5 && PlayerTable.Instance.FifthSkillAvailableCount != 0) FifthSkillBtn.interactable = isBtnOn;
    }
    
    
    */

    public void ResurrectionEvent() // 부활 버튼 클릭 시
    {        
        ResurrectionConfirmUI.gameObject.SetActive(true);
    }

    public void FloatingText(List<TextMeshProUGUI> DamageTextList, float Damage, int SkillCount)
    {
        DamageTextList[SkillCount].text = $"{Damage}";
        DamageTextList[SkillCount].gameObject.SetActive(true);
    }
    public bool CriAttack(int CriticalRate) // 크리티컬 확률
    {
        int range = Random.Range(1, 101);
        if (range < CriticalRate) return true;
        else return false;
    }
    public bool DodgeSucess(int DodgeRate) // 회피 확률
    {
        int range = Random.Range(1, 101);
        if (range < DodgeRate) return true;
        else return false;
    }    
}
