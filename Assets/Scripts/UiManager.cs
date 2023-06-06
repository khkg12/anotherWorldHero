using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using UnityEngine.EventSystems;
using System;

public class UiManager : MonoBehaviour
{

    public static UiManager Instance
    {
        get
        {
            if(_Instance == null)
            {
                _Instance = FindObjectOfType<UiManager>();
            }
            return _Instance;
        }
    }
    private static UiManager _Instance;

    public TextMeshProUGUI RoundText;
    public TextMeshProUGUI DialogText;
    public TextMeshProUGUI talkerName;
    public GameObject CliokAlarm;

    public Image ItemSelectUI;        
    public Image SkillSelectUI;
    public Image BlessingSelectUI; 
    
    public Button StartBattleBtn;
    public Button ItemSelectBtn;
    public Button BlessingSelectBtn;
    public Button SkillSelectBtn;
    public Button NextRoundBtn;    
    public Button NextActBtn;
    public Button RandomSelectBtn;


    // 랜덤 선택지 버튼
    public Button FirstRandomSelectBtn;
    public Button SecondRandomSelectBtn;
    public TextMeshProUGUI FirstRandomSelectText;
    public TextMeshProUGUI SecondRandomSelectText;

    //휴식 선택지 버튼
    public Button HpBtn;
    public Button SkillPtBtn;

    //보스전 승리후 선택지 버튼
    public Button MercyBtn;
    public Button PunishBtn;

    // 메인씬 캐릭터 hp
    public TextMeshProUGUI HpText;

    // 플레이어 정보창 버튼 
    public Button MainPlayerInfoBtn;
    public Button PlayerInfoBtn;

    public Image FadeImg;

    public Action RestBtnEvent; // act추가하고 rest시 취할 action 테스트할것 onenable, destroy 적용시켜 해볼것

    private void Start()
    {                           
        BlessingSelectBtn.onClick.AddListener(() => BlessingSelectUI.gameObject.SetActive(true));
        SkillSelectBtn.onClick.AddListener(() => SkillSelectUI.gameObject.SetActive(true));
        ItemSelectBtn.onClick.AddListener(()=>ItemSelectUI.gameObject.SetActive(true));
        StartBattleBtn.onClick.AddListener(GameManager.Instance.LoadBattleScene);
        NextRoundBtn.onClick.AddListener(()=>StartCoroutine(DialogManager.Instance.nextDialog(GameManager.Instance.NowRound)));
        RandomSelectBtn.onClick.AddListener(()=> StartCoroutine(DialogManager.Instance.nextRandomDialog()));
        MainPlayerInfoBtn.onClick.AddListener(() => GameManager.Instance.PlayerInfoUI.gameObject.SetActive(true));
        PlayerInfoBtn.onClick.AddListener(() => GameManager.Instance.PlayerInfoUI.gameObject.SetActive(true));
        HpBtn.onClick.AddListener(() => StartCoroutine(DialogManager.Instance.nextDialog(GameManager.Instance.NowRound)));
        SkillPtBtn.onClick.AddListener(() => StartCoroutine(DialogManager.Instance.nextDialog(GameManager.Instance.NowRound)));
        MercyBtn.onClick.AddListener(() => DialogManager.Instance.nextDialogFlag = true); // 보스sprite 놀라면서 수줍어 하는 애니실행시키기
        PunishBtn.onClick.AddListener(() => ItemSelectUI.gameObject.SetActive(true)); // 보스disappear 애니 실행
        NextActBtn.onClick.AddListener(() =>DialogManager.Instance.ActChangeAction());
    }    

    private void Update()
    {
        HpText.text = $"{PlayerTable.Instance.Hp}/{PlayerTable.Instance.MaxHp}";
        RoundText.text = $"{GameManager.Instance.NowRound} / 12";
    }
}

