using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using TMPro;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

public struct Dialogs
{
    public string Talker;
    public string Line;
}
public class SceneData
{
    public int Round;
    public string Situation;
    public Dialogs[] Dialog;
}



public class DataManager : MonoBehaviour
{    
    public static DataManager Instance
    {
        get
        {
            if(_Instance == null)
            {
                _Instance = FindObjectOfType<DataManager>();
            }
            return _Instance;
        }
    }
    private static DataManager _Instance;

    public TextAsset JsonFile;
    public SceneData[] sceneData;
    public TextMeshProUGUI talkerName;
    public bool DialogFlag = true;

    private void Awake()
    {
        var thisObj = FindObjectsOfType<DataManager>();
        if(thisObj.Length == 1)
        {
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        sceneData = JsonConvert.DeserializeObject<SceneData[]>(JsonFile.text);  // �������
        int DialogSize = sceneData[GameManager.Instance.NowRound].Dialog.Length;
        
    }
    /*
    public async void getDialog(SceneData sceneData, int lineIndex)
    {
        string[] DialogSplit = sceneData.Dialog[lineIndex].Line.Split(" ");
        string TalkerName = sceneData.Dialog[lineIndex].Talker;        
        talkerName.text = TalkerName;
        DialogManager.Instance.ClickFlag = false;
        // if(TalkerName == "Player") talkerName.text = "������ ���߿� �Է��� json���� ����� string�� �־��ֱ�";
        foreach (string Split in DialogSplit)
        {
            // ���콺Ŭ���� UiManager�� sceneData.Dialog[lineIndex].Line�� ���� foreach�� break;
            if (DialogManager.Instance.ClickFlag == true)
            {
                UiManager.Instance.DialogText.text = sceneData.Dialog[lineIndex].Line;
                break;
            }
            await Task.Delay(80);
            {
                UiManager.Instance.DialogText.text += Split + " ";
            }
        }
        UiManager.Instance.DialogText.text += "\n";
        DialogManager.Instance.ClickFlag = false;

        // �ڷ�ƾ���� ���� -> bool �����߰��ؼ� �ٷ� ���ٿ� ��, ��� ��簡 ������ ��쿡 true�� �Ǵ� ���� ������ �� true�� �� 
        await Task.Delay(500);        
        DialogFlag = true;        
    } */

    public IEnumerator getDialog(SceneData sceneData, int lineIndex)
    {
        string[] DialogSplit = sceneData.Dialog[lineIndex].Line.Split(" ");
        string TalkerName = sceneData.Dialog[lineIndex].Talker;
        talkerName.text = TalkerName;
        DialogManager.Instance.ClickFlag = false;
        DialogManager.Instance.SkipFlag = false;
        foreach (string Split in DialogSplit)
        {
            // ���콺Ŭ���� UiManager�� sceneData.Dialog[lineIndex].Line�� ���� foreach�� break;
            if (DialogManager.Instance.ClickFlag == true)
            {
                UiManager.Instance.DialogText.text = sceneData.Dialog[lineIndex].Line;
                break;
            }
            yield return new WaitForSeconds(0.08f);
            {
                UiManager.Instance.DialogText.text += Split + " ";
            }
        }
        UiManager.Instance.DialogText.text += "\n";
        DialogManager.Instance.SkipFlag = true;
        yield return new WaitForSeconds(5f);
        DialogFlag = true;
    }

    public IEnumerator nextDialog(int NowRound)
    {
        // RandomBackGround.gameObject.SetActive(false);
        GameManager.Instance.NowRound += 1;  // �Լ� ���� �� ������ �� ���� �� �������� ��Ʈ���� ����ϱ� ���� �̸� �ϳ��÷���        
        for (int i = 0; i < sceneData[NowRound].Dialog.Length; i++)
        {
            yield return new WaitUntil(() => DialogFlag == true);
            {
                UiManager.Instance.DialogText.text = "";
                DialogFlag = false;                
                StartCoroutine(getDialog(sceneData[NowRound], i));
            }
        }
        yield return new WaitForSeconds(1f);
        switch (sceneData[NowRound].Situation)
        {
            case "Blessing": // case�߰� ������� �Ʒ��ڵ带 �����ؾ� �ϴ� NowRound�� 10�̸� case : 10 �߰�
                UiManager.Instance.BlessingSelectBtn.gameObject.SetActive(true); 
                break;
            case "Dialog":
                UiManager.Instance.NextRoundBtn.gameObject.SetActive(true);
                break;
            case "Battle":
                UiManager.Instance.StartBattleBtn.gameObject.SetActive(true);
                break;
            case "Victory":            
                UiManager.Instance.RandomSelectBtn.gameObject.SetActive(true);
                MonsterTable.Instance.MonsterNum += 1; // �������� �¸��ϰ� ���� ����� �Ѿ �� MonsterNum�� �÷���
                GameManager.Instance.IsAni = true;
                break;
            case "Skill":
                UiManager.Instance.SkillSelectBtn.gameObject.SetActive(true);
                break;
            case "Rest":
                UiManager.Instance.HpBtn.gameObject.SetActive(true);
                UiManager.Instance.SkillPtBtn.gameObject.SetActive(true);
                //UiManager.Instance.HpBtn.onClick.AddListener(() => HpBtnEvent());
                //UiManager.Instance.SkillPtBtn.onClick.AddListener(() => SkillBtnEvent());
                MonsterTable.Instance.MonsterNum += 1; // �������� �¸��ϰ� ���� ����� �Ѿ �� MonsterNum�� �÷���
                GameManager.Instance.IsAni = true;
                break;
        }
    }
}


