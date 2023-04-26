using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

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
    public TextMeshProUGUI DialogText;    

    public Image ItemSelectUI;        
    public Image SkillSelectUI;
    public Image BlessingSelectUI; 
    
    public Button StartBattleBtn;
    public Button ItemSelectBtn;
    public Button BlessingSelectBtn;
    public Button SkillSelectBtn;
    public Button NextRoundBtn;
    public Button RandomSelectBtn;

    // ���� ������ ��ư
    public Button FirstRandomSelectBtn;
    public Button SecondRandomSelectBtn;
    public TextMeshProUGUI FirstRandomSelectText;
    public TextMeshProUGUI SecondRandomSelectText;

    // ���ξ� ĳ���� hp
    public TextMeshProUGUI HpText;

    // �÷��̾� ����â ��ư 
    public Button MainPlayerInfoBtn;
    public Button PlayerInfoBtn;    



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
    }

    private void Update()
    {
        HpText.text = $"{PlayerTable.Instance.Hp}/{PlayerTable.Instance.MaxHp}";
    }

}
