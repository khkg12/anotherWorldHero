using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using System;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<GameManager>();
            }
            return _Instance;
        }
    }
    private static GameManager _Instance;

    public SpriteRenderer FadeObj; // 페이드 오브젝트 
    public SpriteRenderer IncreaseFadeObj;
    public SpriteRenderer DecreaseFadeObj;
    public Action RoundChangeAction; // Action : 매개변수가 없는 void타입의 함수만 등록가능한 delegate 명령어 
    public Action AfterVictory;

    public int NowAct;
    public int NowRound
    {
        get => _NowRound;                        
        set
        {            
            _NowRound = value; // 라운드 증가 시마다 페이드 활성화 -> 라운드에 따라 백그라운드가 바뀌기때문             
        }       
    }
    public int _NowRound;    

    // 플레이어 정보창
    public Image PlayerInfoUI;

    // 몬스터 등장/데드 씬 플래그 변수
    public bool IsMonsterDead;
    // 몬스터 등장/데드 씬 update무한반복 방지 플래그
    public bool IsAni;

    [SerializeField]
    private SpriteRenderer BackGroundSprite; // 백그라운드
    private List<BgImage> BgImageList; // 보스를 잡고 액트의 넘버가 변경될 때 act에 맞는 리스트로 변경하는 함수를 event에 등록해서 사용해볼것 
    private Animator BackGroundAnimator;


    void Awake()
    {        
        var thisObj = FindObjectsOfType<GameManager>();  // FindObjectsOfType : 해당하는 타입의 오브젝트를 찾아서 배열로줌
        if (thisObj.Length == 1)
        {
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }        
        PlayerTable.Instance.playerSkillList[0] = SkillTable.Instance.attack; // 처음 게임시작 시 공격과 수비 기본스킬로 플레이어 스킬리스트에 추가
        PlayerTable.Instance.playerSkillList[1] = SkillTable.Instance.defense;
        if (PlayerTable.Instance.playerSkillList[2].Name != "")
        {
            PlayerTable.Instance.playerSkillList[2] = SkillMatch($"{PlayerTable.Instance.playerSkillList[2].Name}"); // 스킬저장
        }
        if (PlayerTable.Instance.playerSkillList[3].Name != "")
        {
            PlayerTable.Instance.playerSkillList[3] = SkillMatch($"{PlayerTable.Instance.playerSkillList[3].Name}"); 
        }
        if (PlayerTable.Instance.playerSkillList[4].Name != "")
        {
            PlayerTable.Instance.playerSkillList[4] = SkillMatch($"{PlayerTable.Instance.playerSkillList[4].Name}"); 
        }                

        IsMonsterDead = false; // 몬스터 등장 씬 트리거정하는 변수 false로 초기화 (몬스터와 싸우기전이기때문)
        IsAni = true; // 처음엔 true여야 실행되므로 true로 초기화

        BackGroundSprite = GetComponentsInChildren<SpriteRenderer>()[0]; //자식오브젝트에 있으므로 가져옴
        BackGroundAnimator = GetComponentsInChildren<Animator>()[0];        

        AfterVictory += LoadMainScene;
        NowAct = 1;
        BgImageList = BackGroundTable.Instance.FirstActBgList;

        SkillTable.Instance.ActiveSkillList = new List<BaseSkill>() { SkillTable.Instance.doubleAttack, SkillTable.Instance.baldo, SkillTable.Instance.stunBoom }; // 스킬리스트 초기화
    }

    private void OnEnable()
    {
        RoundChangeAction += Fade; // 라운드가 변할 시 사용될 함수들 중 하나인 fade등록
        RoundChangeAction += NowRoundChangedEvent; // action에 등록        
        DialogManager.Instance.ActChangeAction += NowActChangeEvent;
    }

    public void LoadBattleScene() // 전투시작 버튼 눌렀을 때 호출되는 함수 -> 고로 전투씬을 띄움
    {
        // 배틀 이미지 띄우기
        SceneManager.LoadScene("BattleScene");        
    }
    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");        
    }
    public void LoadDeadScene()
    {
        SceneManager.LoadScene("DeadScene");
    }
    public BaseSkill SkillMatch(string Name) // 이름체크 후 스킬매치
    {
        if (Name == "발도") return SkillTable.Instance.baldo;
        else if (Name == "더블 어택") return SkillTable.Instance.doubleAttack;
        else if (Name == "스턴붐") return SkillTable.Instance.stunBoom;
        else return null;
    }    
    public void Fade()
    {
        FadeObj.gameObject.SetActive(true);
    }

    private void NowRoundChangedEvent() // NowRound가 변경되었을 때 이벤트 등록
    {
        BackGroundSprite.sprite = BgImageList[NowRound - 1].bgSprite;
        transform.position = new Vector2(0, 0);
        BackGroundAnimator.Rebind();
        BackGroundAnimator.enabled = true;
    }

    private void NowActChangeEvent()
    {
        switch (NowAct)
        {
            case 2:
                BgImageList = BackGroundTable.Instance.SecondActBgList; // ACT별 배경리스트 따로만들고 ACT에 따라 그 리스트를 넣어줌
                break;
            case 3:
                BgImageList = BackGroundTable.Instance.ThirdActBgList;
                break;
            case 4:
                BgImageList = BackGroundTable.Instance.FourthActBgList;
                break;
        }
        MonsterTable.Instance.MonsterNum = 0; // 몬스터 넘버초기화
    }
}
