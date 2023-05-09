using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor.TextCore.Text;
using JetBrains.Annotations;
using TMPro;
using UnityEngine.EventSystems;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<DialogManager>();
            }
            return _Instance;
        }
    }
    private static DialogManager _Instance;
    
    // ���丮 �ؽ�Ʈ ���� ������Ʈ
    public Dictionary<int, string[]> DialogData;
    // ���� �̺�Ʈ �ؽ�Ʈ ���� ������Ʈ
    public Dictionary<int, string[]> RandomDialogData;
    // ���� �̺�Ʈ ù��° ��ư ��� �ؽ�Ʈ ���� ������Ʈ
    public Dictionary<int, string[]> FirstRandomResultDialogData;
    // ���� �̺�Ʈ �ι�° ��ư ��� �ؽ�Ʈ ���� ������Ʈ
    public Dictionary<int, string[]> SecondRandomResultDialogData;
    // ���� �̺�Ʈ �ε��� ����Ʈ
    public List<int> RandomEventList;
    
    public bool DialogFlag = true;
    public bool ClickFlag = false;
    public bool SkipFlag = false;    

    public TextMeshProUGUI talkerName;
    public GameObject CliokAlarm;

    void Awake()
    {                
        var thisObj = FindObjectsOfType<DialogManager>();  // FindObjectsOfType : �ش��ϴ� Ÿ���� ������Ʈ�� ã�Ƽ� �迭����
        if(thisObj.Length == 1)
        {
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }        

        DialogData = new Dictionary<int, string[]>();
        StartDialogData();
        RandomDialogData = new Dictionary<int, string[]>();
        StartRandomDialogData();
        FirstRandomResultDialogData = new Dictionary<int, string[]>();
        StartFirstRandomResultDialogData();
        SecondRandomResultDialogData = new Dictionary<int, string[]>();
        StartSecondRandomResultDialogData();        

        RandomEventList = new List<int> { 0, 1, 2, 3, 4, 5, 6 };
        
    }

    private void Start()
    {
        if (GameManager.Instance.NowRound == 0)
        {
            StartCoroutine(nextDialog(GameManager.Instance.NowRound));
        }
    }

    public void StartDialogData() // �Ϲ� ��ȭâ ���� �Լ�
    {
        DialogData.Add(0, new string[] { "�ȳ�? �ݰ��� ������ �� ���? ���� �����̾�.", "\n�� ���� ������� ���� ȥ�����¿� ��������.", "\n�ʰ� �̼����� ������ ó�����ָ� �ʸ� �ǰ��� ������ ȥ�����¿��� ����� ���ٰ�." });
        DialogData.Add(1, new string[] { "���ռ� �Ա��� �����ߴ�!!", "\n����� �����⸦ ǳ���", "\n���� ���� �Ŵ��� �Ҹ��� ���� ���� õõ�� ������ �����ߴ�." });
        DialogData.Add(2, new string[] { "���� �����ߴ�!!", "\n���ռ� 1���� �Ա��� ��Ű�� �ϱ� ��������.", "\n��ٶ� ���� ��� �������� õõ�� ����� ������ �ٰ��Դ�." });
        DialogData.Add(3, new string[] { "�ϱ� ������縦 �����߷ȴ�!", "\n���� ���� ��뿴���� ������ �� ġƮ���� ���� ����� �ʿ��ߴ�.", "\n����� ��ģ ���� ���� ���ռ� ������ �ȱ� �����ߴ�." });
        DialogData.Add(4, new string[] { "��𼱰� ������ ȭ���� ���ƿԴ�!", "\n�������� ��ü�ɷ����� ȭ���� ȸ������ ȭ���� ������ ����� �巯�´�.", "\n���� �û�� Ȱ������ �ܴ��� ������ ������ �˷ȴ�." });
        DialogData.Add(5, new string[] { "���� �ü��� �����߷ȴ�!", "\n���� ȭ���� �ſ� ��ī�ο����� �װ� ���ο���.", "\n����� ��ģ ���� ���� ���ռ� ������ �ȱ� �����ߴ�." });
        DialogData.Add(6, new string[] { "�쿬�� �߰��� ���� ����.", "\n�� ���� å�� �� �ϳ��� å���� ������ ����� ��������.", "\n����� �� å�� ��� ���ĺ��Ҵ�." });
        DialogData.Add(7, new string[] { "��ī�ο��� �˰��� ������ ����� Ž���ߴ�!", "\n�ָ��� �� �ο��� �Ƿ翧�� ���δ�.", "\n���� �ּ���� �ֹ��� �ܿ�� ����� ���� �ɾ���� �ִ�." });
        DialogData.Add(8, new string[] { "���� �ּ��縦 �����߷ȴ�!", "\n���� ������ �ܿ� ���Ƴ��� ���� ���� ������.", "\n����� ��ģ ���� ���� ���ռ� ������ �ȱ� �����ߴ�." });
        DialogData.Add(9, new string[] { "�Ŵ��� ���� ���θ��� �ִ�.", "\n�ֺ��� �ƹ��� ��ô�� ����. ����� ȭ����� �ǿ���.", "\n����� �޽��� ���ߴ�." });
    }

    public void StartRandomDialogData() // ���� ��ȭâ ���� �Լ�
    {
        RandomDialogData.Add(0, new string[] { "������ ���ư����µ� �ٴڿ� �ִ� ���Ű� �߿� �ε�����.", "\n���� ������ ���ſ����� �� �� ���� ������ ������, ���ϰ� ���ִ� ������ �͵� ����.", "\n��� �ұ�?"});
        RandomDialogData.Add(1, new string[] { "�쿬�� �߰��� â�� ����.","\n���� ������ ���� ä�� ���������� ���ǵ��� �����ϴ�.", "\n�� �߿��� ���� Ư���� ���̴� ���ǵ��� �߰��ߴ�."});
        RandomDialogData.Add(2, new string[] { "���� ���ڸ� �߰��ߴ�.", "\n���԰� ���� �����ϴ�."});
        RandomDialogData.Add(3, new string[] { "��𼱰� �������� ��� �Ҹ��� �鸰��.", "\n���� �Ҹ��� ���� ���� ã�ư����� ���� ������ ���׿� ��� �ִ� �ҳฦ �߰��ߴ�." });
        RandomDialogData.Add(4, new string[] { "������ Ʈ���� �ߵ��ϸ鼭 ���� ���� ȭ���� ��ó�� ��������ȴ�."});
        RandomDialogData.Add(5, new string[] { "�쿬�� �߰��� ���� ����.", "\nå���� ������ ���� ������ �ŴҴ� ���� ��� å�� �߰��ߴ�." });
        RandomDialogData.Add(6, new string[] { "�������� ����� ���մ� �� �� �ڷ簡 ���� ������ �����ִ�.", "\n���� �����̸� ������ �ͼ��� ���� ��������, �̰��� ���� ����� ���̶�� ����� �˰� �Ǿ���.", "\n���� ����� �ູ�� �귯���Դ�."});
    }

    public void StartFirstRandomResultDialogData() // ���� ������ ù��° ��� ��ȭâ ����
    {
        FirstRandomResultDialogData.Add(0, new string[] { "���� ���ϰ� �ϴ� ���Ÿ� �Ծ���.", "\n���Ŵ� ������ ���� ������ ����� ȸ������ �־���.", $"\n�ִ�ü���� 30%�� ȸ���Ͽ���!({PlayerTable.Instance.MaxHp * 0.3f}) ȸ��" });
        FirstRandomResultDialogData.Add(1, new string[] { "���� ������ ���̴�.", "\n������ ���ô� ���� �� �Ű��� ���ߵǾ���.", "\nġ��Ÿ Ȯ���� 3% �����Ͽ���!" });
        FirstRandomResultDialogData.Add(2, new string[] { "�̹��̾���!.", "\n�ִ�ü���� 10%�� �����Ͽ���." });
        FirstRandomResultDialogData.Add(3, new string[] { "�ҳ࿡�� ������ �ٰ�����.", "\n�������� �ҳడ �����ΰ��� ���޾Ҵ�!", "\n���Ϳ��� ����� ���Ͽ���!", "\n�ִ� ü���� 40%�� �����Ͽ���." });
        FirstRandomResultDialogData.Add(4, new string[] { "�������� ȸ�� �Ͽ����� 2~3���� ȭ�츸�� ����Դ�.", "\n���� ����س� �� �־���." });
        FirstRandomResultDialogData.Add(5, new string[] { "�����̶�� �ܾ ���� å�� ���ƴ�.", "\n�� ���� ���� �� �� ���� ������ ����� ���Դ�.", "\n���ݷ��� 10% �����Ͽ���!" });
        
    }

    public void StartSecondRandomResultDialogData() // ���� ������ �ι�° ��� ��ȭâ ����
    {
        SecondRandomResultDialogData.Add(0, new string[] { "Ȥ�� ���� ���Ÿ� ������ ��������", "\n��� �������� �ּ��� �����̶� �����ߴ�." });
        SecondRandomResultDialogData.Add(1, new string[] { "�Ķ� ������ ���̴�.", "\n������ ���ô� ���� ����� �ſ� ����������.", "\nȸ������ 3% �����Ͽ���!" });
        SecondRandomResultDialogData.Add(2, new string[] { "�ڼ��� ���캸�� ���� ���� �༼�� �ϴ� �̹��̾���.", "\n�̹��� �����ϱ� ���� ������ �� ġ����� ���� �� �־���.", "\nġ��Ÿ Ȯ���� 3% �����Ͽ���!" });
        SecondRandomResultDialogData.Add(3, new string[] { "�ڼ��� ���캸�� �ҳ� �༼�� �ϴ� ���Ϳ���.", "\n���Ͱ� ���������, �̸� ������ ���� ������ ���� �� �־���.", "\nȸ������ 3% �����Ͽ���!" });
        SecondRandomResultDialogData.Add(4, new string[] { "���������� ȸ���ߴ��� ���� �ִ� �������� ��κ��� ȭ���� ���ƿԴ�!", "\nȭ���� �ĳ����� ��� ȭ���� ���� �� ������.", "\n�ִ� ü���� 30%�� �����Ͽ���." });
        SecondRandomResultDialogData.Add(5, new string[] { "�����̶�� �ܾ ���� å�� ���ƴ�.", "\n�� ���� ���� �� �� ���� ������ ����� ���Դ�.", "\n������ 10% �����Ͽ���!" });
        
    }

    
    public IEnumerator getDialog(Dialogs[] Dialog, int lineIndex, int LastIndex)
    {
        string[] DialogSplit = Dialog[lineIndex].Line.Split(" ");
        string TalkerName = Dialog[lineIndex].Talker;
        talkerName.text = TalkerName;
        ClickFlag = false;
        SkipFlag = false;
        foreach (string Split in DialogSplit)
        {
            // ���콺Ŭ���� UiManager�� sceneData.Dialog[lineIndex].Line�� ���� foreach�� break;
            if (ClickFlag == true)
            {
                UiManager.Instance.DialogText.text = Dialog[lineIndex].Line;
                break;
            }
            yield return new WaitForSeconds(0.08f);
            {
                UiManager.Instance.DialogText.text += Split + " ";
            }
        }        
        if(lineIndex < LastIndex - 1) CliokAlarm.gameObject.SetActive(true); // ��ȭ�� �������� Ŭ���˶��� ����� �ȵǹǷ� ������ ��ȭ�ε����� ��������ȭ�ε������� ��������
        UiManager.Instance.DialogText.text += "\n";
        SkipFlag = true;        
    }

    public IEnumerator nextDialog(int NowRound)
    {        
        RandomBackGround.gameObject.SetActive(false);
        GameManager.Instance.NowRound += 1;  // �Լ� ���� �� ������ �� ���� �� �������� ��Ʈ���� ����ϱ� ���� �̸� �ϳ��÷���
        DialogFlag = true;
        int DialogSize = DataManager.Instance.sceneData[NowRound].Dialog.Length;
        for (int i = 0; i < DialogSize; i++)
        {            
            yield return new WaitUntil(() => DialogFlag == true);
            {
                CliokAlarm.gameObject.SetActive(false);
                UiManager.Instance.DialogText.text = "";
                DialogFlag = false;
                StartCoroutine(getDialog(DataManager.Instance.sceneData[NowRound].Dialog, i, DialogSize));                
            }                                    
        }        
        yield return new WaitForSeconds(1f);        
        switch (DataManager.Instance.sceneData[NowRound].Situation)
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
                UiManager.Instance.HpBtn.onClick.AddListener(() => HpBtnEvent());
                UiManager.Instance.SkillPtBtn.onClick.AddListener(() => SkillBtnEvent());
                MonsterTable.Instance.MonsterNum += 1; // �������� �¸��ϰ� ���� ����� �Ѿ �� MonsterNum�� �÷���
                GameManager.Instance.IsAni = true;
                break;
        }
    }

    
    public void NextPage<T>(Image UiImage, List<T> list) // Ư��, ��ų, ������ â�� ����ϱ� ��ư, ���� ���κи��ص� ������ �Ʒ� �ּ��� ����
    {        
        StartCoroutine(nextDialog(GameManager.Instance.NowRound));
        list = list.OrderBy(i => Random.value).ToList(); // start������ �ѹ� ����ȭ �Ǳ� ������ ��ȭ������ǰ� ������ ���� �� �ѹ� �� ����ȭ ���� ���� ���� �� �������� �ʵ��� ��
        UiImage.gameObject.SetActive(false);        
    }
    
    public SpriteRenderer RandomBackGround;
    
    // �׽�Ʈ�� �ڵ�

    public IEnumerator nextRandomDialog()
    {
        RandomBackGround.gameObject.SetActive(true);
        GameManager.Instance.FadeObj.gameObject.SetActive(true);
        UiManager.Instance.DialogText.text = "";
        RandomEventList = RandomEventList.OrderBy(i => Random.value).ToList();
        int rand = RandomEventList[0];
        RandomEventList.RemoveAt(0);
        RandomBackGround.sprite = BackGroundTable.Instance.RandomBackGroundImageList[rand].bgSprite;
        DialogFlag = true;
        int size = DataManager.Instance.RandomSceneData[rand].Dialog.Length;
        for (int i = 0; i < size; i++) // jsonData�� ����
        {
            yield return new WaitUntil(() => DialogFlag == true);
            {
                DialogFlag = false;
                getDialog(DataManager.Instance.RandomSceneData[rand].Dialog, i, size);
            }
        }
        yield return new WaitForSeconds(1.0f);        
        UiManager.Instance.FirstRandomSelectBtn.gameObject.SetActive(true);
        UiManager.Instance.SecondRandomSelectBtn.gameObject.SetActive(true);
        
        BtnTextSet(DataManager.Instance.RandomSceneData[rand].FristBtn, DataManager.Instance.RandomSceneData[rand].SecondBtn, rand);

        float[] RecoveryAmount = DataManager.Instance.RandomSceneData[rand].RecoveryAmount;  // �ΰ��� �������� ���� �迭��      
        float[] ReduceAmount = DataManager.Instance.RandomSceneData[rand].ReduceAmount;
        float[] AtkIncPer = DataManager.Instance.RandomSceneData[rand].AtkIncPer;
        float[] DefIncPer = DataManager.Instance.RandomSceneData[rand].DefIncPer;
        int[] Cri = DataManager.Instance.RandomSceneData[rand].Cri;
        int[] Dod = DataManager.Instance.RandomSceneData[rand].Dod;        
        UiManager.Instance.FirstRandomSelectBtn.onClick.AddListener(() => BtnOnClickEvent(RecoveryAmount[0], ReduceAmount[0], AtkIncPer[0], DefIncPer[0], Cri[0], Dod[0])); // HPȸ����, HP���ҷ�, ���ݷ� ������, ���� ������, ġ��Ÿ, ȸ����                
        UiManager.Instance.SecondRandomSelectBtn.onClick.AddListener(() => BtnOnClickEvent(RecoveryAmount[1], ReduceAmount[1], AtkIncPer[1], DefIncPer[1], Cri[1], Dod[1])); 
    }

    public void BtnTextSet(string firstText, string SecondText, int rand)
    {
        UiManager.Instance.FirstRandomSelectText.text = firstText;
        UiManager.Instance.SecondRandomSelectText.text = SecondText;
        UiManager.Instance.FirstRandomSelectBtn.onClick.AddListener(() => StartCoroutine(PrintRandomSelectResult(DataManager.Instance.RandomSceneData[rand].FirstDialog)));
        UiManager.Instance.SecondRandomSelectBtn.onClick.AddListener(() => StartCoroutine(PrintRandomSelectResult(DataManager.Instance.RandomSceneData[rand].SecondDialog)));
    }

    public void BtnOnClickEvent(float RecoveryAmount, float ReduceAmount, float AtkIncPer, float DefIncPer, int Cri, int Dod)
    {
        PlayerTable.Instance.Hp += RecoveryAmount * PlayerTable.Instance.MaxHp;
        PlayerTable.Instance.Hp -= ReduceAmount * PlayerTable.Instance.MaxHp;
        PlayerTable.Instance.Atk += AtkIncPer * PlayerTable.Instance.Atk;
        PlayerTable.Instance.Defense += DefIncPer * PlayerTable.Instance.Defense;
        PlayerTable.Instance.Critical += Cri;
        PlayerTable.Instance.Dodge += Dod;
    }

    public IEnumerator PrintRandomSelectResult(Dialogs[] Dialog)
    {
        UiManager.Instance.DialogText.text = "";
        DialogFlag = true;
        for (int i = 0; i < Dialog.Length; i++)
        {
            yield return new WaitUntil(() => DialogFlag == true);
            {
                DialogFlag = false;
                getDialog(Dialog, i, Dialog.Length);
            }
        }
        yield return new WaitForSeconds(0.4f);
        {
            UiManager.Instance.NextRoundBtn.gameObject.SetActive(true);
        }
    }

    public void HpBtnEvent()
    {
        PlayerTable.Instance.Hp += (int)(0.3f * PlayerTable.Instance.MaxHp);
    }

    public void SkillBtnEvent()
    {
        PlayerTable.Instance.SecondSkillAvailableCount += 2;
        if (PlayerTable.Instance.SecondSkillAvailableCount > PlayerTable.Instance.secondCount) PlayerTable.Instance.SecondSkillAvailableCount = PlayerTable.Instance.secondCount;
        if (PlayerTable.Instance.playerSkillList[2].Name != "")
        {
            PlayerTable.Instance.ThirdSkillAvailableCount += 2;
            if (PlayerTable.Instance.ThirdSkillAvailableCount > PlayerTable.Instance.thirdCount) PlayerTable.Instance.ThirdSkillAvailableCount = PlayerTable.Instance.thirdCount;
        }
        if (PlayerTable.Instance.playerSkillList[3].Name != "")
        {
            PlayerTable.Instance.FourthSkillAvailableCount += 2;
            if (PlayerTable.Instance.FourthSkillAvailableCount > PlayerTable.Instance.fourthCount) PlayerTable.Instance.FourthSkillAvailableCount = PlayerTable.Instance.fourthCount;
        }
        if (PlayerTable.Instance.playerSkillList[4].Name != "")
        {
            PlayerTable.Instance.FifthSkillAvailableCount += 2;
            if (PlayerTable.Instance.FifthSkillAvailableCount > PlayerTable.Instance.fifthCount) PlayerTable.Instance.FifthSkillAvailableCount = PlayerTable.Instance.fifthCount;
        }
    }

}



