using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;


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
    public int NowRound
    {
        get => _NowRound;                        
        set
        {
            _NowRound = value; // ���� ���� �ø��� ���̵� Ȱ��ȭ -> ���忡 ���� ��׶��尡 �ٲ�⶧��
            FadeObj.gameObject.SetActive(true);            
        }       
    }
    public int _NowRound;

    // �÷��̾� ����â
    public Image PlayerInfoUI;

    // ���� ����/���� �� �÷��� ����
    public bool IsMonsterDead;
    // ���� ����/���� �� update���ѹݺ� ���� �÷���
    public bool IsAni;


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
        PlayerTable.Instance.playerSkillList[1] = SkillTable.Instance.stunBoom;        

        IsMonsterDead = false; // ���� ���� �� Ʈ�������ϴ� ���� false�� �ʱ�ȭ (���Ϳ� �ο�����̱⶧��)
        IsAni = true; // ó���� true���� ����ǹǷ� true�� �ʱ�ȭ
    }

    public void LoadBattleScene() // �������� ��ư ������ �� ȣ��Ǵ� �Լ� -> ��� �������� ���
    {
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
    
    
}
