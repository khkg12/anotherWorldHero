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

    public SpriteRenderer FadeObj; // ���̵� ������Ʈ 
    public SpriteRenderer IncreaseFadeObj;
    public SpriteRenderer DecreaseFadeObj;
    public Action RoundChangeAction; // Action : �Ű������� ���� voidŸ���� �Լ��� ��ϰ����� delegate ��ɾ� 
    public Action AfterVictory;

    public int NowAct;
    public int NowRound
    {
        get => _NowRound;                        
        set
        {            
            _NowRound = value; // ���� ���� �ø��� ���̵� Ȱ��ȭ -> ���忡 ���� ��׶��尡 �ٲ�⶧��             
        }       
    }
    public int _NowRound;    

    // �÷��̾� ����â
    public Image PlayerInfoUI;

    // ���� ����/���� �� �÷��� ����
    public bool IsMonsterDead;
    // ���� ����/���� �� update���ѹݺ� ���� �÷���
    public bool IsAni;

    [SerializeField]
    private SpriteRenderer BackGroundSprite; // ��׶���
    private List<BgImage> BgImageList; // ������ ��� ��Ʈ�� �ѹ��� ����� �� act�� �´� ����Ʈ�� �����ϴ� �Լ��� event�� ����ؼ� ����غ��� 
    private Animator BackGroundAnimator;


    void Awake()
    {        
        var thisObj = FindObjectsOfType<GameManager>();  // FindObjectsOfType : �ش��ϴ� Ÿ���� ������Ʈ�� ã�Ƽ� �迭����
        if (thisObj.Length == 1)
        {
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }        
        PlayerTable.Instance.playerSkillList[0] = SkillTable.Instance.attack; // ó�� ���ӽ��� �� ���ݰ� ���� �⺻��ų�� �÷��̾� ��ų����Ʈ�� �߰�
        PlayerTable.Instance.playerSkillList[1] = SkillTable.Instance.defense;
        if (PlayerTable.Instance.playerSkillList[2].Name != "")
        {
            PlayerTable.Instance.playerSkillList[2] = SkillMatch($"{PlayerTable.Instance.playerSkillList[2].Name}"); // ��ų����
        }
        if (PlayerTable.Instance.playerSkillList[3].Name != "")
        {
            PlayerTable.Instance.playerSkillList[3] = SkillMatch($"{PlayerTable.Instance.playerSkillList[3].Name}"); 
        }
        if (PlayerTable.Instance.playerSkillList[4].Name != "")
        {
            PlayerTable.Instance.playerSkillList[4] = SkillMatch($"{PlayerTable.Instance.playerSkillList[4].Name}"); 
        }                

        IsMonsterDead = false; // ���� ���� �� Ʈ�������ϴ� ���� false�� �ʱ�ȭ (���Ϳ� �ο�����̱⶧��)
        IsAni = true; // ó���� true���� ����ǹǷ� true�� �ʱ�ȭ

        BackGroundSprite = GetComponentsInChildren<SpriteRenderer>()[0]; //�ڽĿ�����Ʈ�� �����Ƿ� ������
        BackGroundAnimator = GetComponentsInChildren<Animator>()[0];        

        AfterVictory += LoadMainScene;
        NowAct = 1;
        BgImageList = BackGroundTable.Instance.FirstActBgList;

        SkillTable.Instance.ActiveSkillList = new List<BaseSkill>() { SkillTable.Instance.doubleAttack, SkillTable.Instance.baldo, SkillTable.Instance.stunBoom }; // ��ų����Ʈ �ʱ�ȭ
    }

    private void OnEnable()
    {
        RoundChangeAction += Fade; // ���尡 ���� �� ���� �Լ��� �� �ϳ��� fade���
        RoundChangeAction += NowRoundChangedEvent; // action�� ���        
        DialogManager.Instance.ActChangeAction += NowActChangeEvent;
    }

    public void LoadBattleScene() // �������� ��ư ������ �� ȣ��Ǵ� �Լ� -> ��� �������� ���
    {
        // ��Ʋ �̹��� ����
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
    public BaseSkill SkillMatch(string Name) // �̸�üũ �� ��ų��ġ
    {
        if (Name == "�ߵ�") return SkillTable.Instance.baldo;
        else if (Name == "���� ����") return SkillTable.Instance.doubleAttack;
        else if (Name == "���Ϻ�") return SkillTable.Instance.stunBoom;
        else return null;
    }    
    public void Fade()
    {
        FadeObj.gameObject.SetActive(true);
    }

    private void NowRoundChangedEvent() // NowRound�� ����Ǿ��� �� �̺�Ʈ ���
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
                BgImageList = BackGroundTable.Instance.SecondActBgList; // ACT�� ��渮��Ʈ ���θ���� ACT�� ���� �� ����Ʈ�� �־���
                break;
            case 3:
                BgImageList = BackGroundTable.Instance.ThirdActBgList;
                break;
            case 4:
                BgImageList = BackGroundTable.Instance.FourthActBgList;
                break;
        }
        MonsterTable.Instance.MonsterNum = 0; // ���� �ѹ��ʱ�ȭ
    }
}
