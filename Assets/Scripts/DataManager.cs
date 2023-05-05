using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using TMPro;
using System.Threading.Tasks;

public struct Dialogs
{
    public string Talker;
    public string Line;
}
public class SceneData
{
    public int Round;
    public Dialogs[] Dialog;
}

public class DataManager : MonoBehaviour
{
    public TextAsset JsonFile;
    public SceneData[] sceneData;
    public TextMeshProUGUI talkerName;
    public bool DialogFlag = true;

    private void Awake()
    {
        sceneData = JsonConvert.DeserializeObject<SceneData[]>(JsonFile.text);  // �������
        int DialogSize = sceneData[GameManager.Instance.NowRound].Dialog.Length;
        if (GameManager.Instance.NowRound == 0)
        {
            StartCoroutine(nextDialog(GameManager.Instance.NowRound));
        }
    }
    
    public async void getDialog(SceneData sceneData, int lineIndex)
    {
        string[] DialogSplit = sceneData.Dialog[lineIndex].Line.Split(" ");
        string TalkerName = sceneData.Dialog[lineIndex].Talker;
        Debug.Log(TalkerName);
        talkerName.text = TalkerName;
        foreach (string Split in DialogSplit)
        {
            // ���콺Ŭ���� UiManager�� sceneData.Dialog[lineIndex].Line�� ���� foreach�� break;
            await Task.Delay(80);
            {
                UiManager.Instance.DialogText.text += Split + " ";
            }
        }
        UiManager.Instance.DialogText.text += "\n";
        await Task.Delay(500);
        DialogFlag = true;
        UiManager.Instance.DialogText.text = "";
    }

    public IEnumerator nextDialog(int NowRound)
    {
        // RandomBackGround.gameObject.SetActive(false);
        GameManager.Instance.NowRound += 1;  // �Լ� ���� �� ������ �� ���� �� �������� ��Ʈ���� ����ϱ� ���� �̸� �ϳ��÷���
        UiManager.Instance.DialogText.text = "";
        for (int i = 0; i < sceneData[NowRound].Dialog.Length; i++)
        {
            yield return new WaitUntil(() => DialogFlag == true);
            {
                DialogFlag = false;
                getDialog(sceneData[NowRound], i);
            }
        }
        yield return new WaitForSeconds(1f);
        /*switch (NowRound)
        {
            case 0: // case�߰� ������� �Ʒ��ڵ带 �����ؾ� �ϴ� NowRound�� 10�̸� case : 10 �߰�
                UiManager.Instance.BlessingSelectBtn.gameObject.SetActive(true); // Ư���߰��ϸ� Ư��â���� ����/  ������ ��ư -> Ư�� ��ư���� ����Ϸ�
                break;
            case 1:
                UiManager.Instance.NextRoundBtn.gameObject.SetActive(true);
                break;
            case 8:
                UiManager.Instance.NextRoundBtn.gameObject.SetActive(true);
                MonsterTable.Instance.MonsterNum += 1;
                break;
            case 2:
            case 4:
            case 7:
            case 10:
                UiManager.Instance.StartBattleBtn.gameObject.SetActive(true);
                break;
            case 3:
            case 5:
                UiManager.Instance.RandomSelectBtn.gameObject.SetActive(true);
                MonsterTable.Instance.MonsterNum += 1; // �������� �¸��ϰ� ���� ����� �Ѿ �� MonsterNum�� �÷���
                GameManager.Instance.IsAni = true;
                break;
            case 6:
                UiManager.Instance.SkillSelectBtn.gameObject.SetActive(true);
                break;
            case 9:
                UiManager.Instance.HpBtn.gameObject.SetActive(true);
                UiManager.Instance.SkillPtBtn.gameObject.SetActive(true);
                //UiManager.Instance.HpBtn.onClick.AddListener(() => HpBtnEvent());
                //UiManager.Instance.SkillPtBtn.onClick.AddListener(() => SkillBtnEvent());
                break;
        }*/
    }
}


