using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;

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

        if (GameManager.Instance.NowRound == 0) 
        {            
            StartCoroutine(nextDialog(GameManager.Instance.NowRound));            
        }

        RandomEventList = new List<int> { 0, 1, 2, 3, 4, 5, 6 };
    }        

    public void StartDialogData() // �Ϲ� ��ȭâ ���� �Լ�
    {
        DialogData.Add(0, new string[] { "�ȳ�? �ݰ��� ������ �� ���? ���� �����̾�.", "\n�� ���� ������� ���� <color=red>ȥ������</color>�� ��������.", "\n�ʰ� �̼����� ������ ó�����ָ� �ʸ� �ǰ��� ������ ȥ�����¿��� ����� ���ٰ�." });
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

    public string GetDialog(Dictionary<int, string[]> dialogData, int key, int dialogIndex)
    {        
        return dialogData[key][dialogIndex];
    }

    public async void getDialog(Dictionary<int, string[]> dialogData, int key, int dialogIndex)
    {
        string[] DialogSplit = dialogData[key][dialogIndex].Split(" ");
        foreach (string Split in DialogSplit)
        {
            await Task.Delay(80);
            {
                UiManager.Instance.DialogText.text += Split + " ";
            }
        }
        await Task.Delay(500);        
        DialogFlag = true;
    }    

    public IEnumerator nextDialog(int NowRound)
    {
        RandomBackGround.gameObject.SetActive(false);
        GameManager.Instance.NowRound += 1;  // �Լ� ���� �� ������ �� ���� �� �������� ��Ʈ���� ����ϱ� ���� �̸� �ϳ��÷���
        UiManager.Instance.DialogText.text = "";
        for (int i = 0; i < DialogData[NowRound].Length; i++)
        {            
            yield return new WaitUntil(() => DialogFlag == true);
            {
                DialogFlag = false;
                getDialog(DialogData, NowRound, i);
            }            
        }
        yield return new WaitForSeconds(1f);
        switch (NowRound)
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
                UiManager.Instance.HpBtn.onClick.AddListener(() => HpBtnEvent());
                UiManager.Instance.SkillPtBtn.onClick.AddListener(() => SkillBtnEvent());
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

    public IEnumerator nextRandomDialog()
    {
        RandomBackGround.gameObject.SetActive(true);
        GameManager.Instance.FadeObj.gameObject.SetActive(true);
        UiManager.Instance.DialogText.text = "";
        RandomEventList = RandomEventList.OrderBy(i => Random.value).ToList();
        int rand = RandomEventList[0];
        RandomEventList.RemoveAt(0);        
        RandomBackGround.sprite = BackGroundTable.Instance.RandomBackGroundImageList[rand].bgSprite;

        for (int i = 0; i < RandomDialogData[rand].Length; i++)
        {
            yield return new WaitUntil(() => DialogFlag == true);
            {
                DialogFlag = false;
                getDialog(RandomDialogData, rand, i);
            }            
        }
        if (rand != 6)
        {
            yield return new WaitForSeconds(1.0f);
            {

                UiManager.Instance.FirstRandomSelectBtn.gameObject.SetActive(true);
                UiManager.Instance.SecondRandomSelectBtn.gameObject.SetActive(true);

            }
        }
        switch (rand)
        {
            case 0:
                BtnTextSet("�Դ´�", "������", rand);
                UiManager.Instance.FirstRandomSelectBtn.onClick.AddListener(() => BtnOnClickEvent(0.3f, 0, 0, 0, 0, 0)); // HPȸ����, HP���ҷ�, ���ݷ� ������, ���� ������, ġ��Ÿ, ȸ����                
                UiManager.Instance.SecondRandomSelectBtn.onClick.AddListener(() => BtnOnClickEvent(0, 0, 0, 0, 0, 0));
                break;
            case 1:
                BtnTextSet("��������", "�Ķ�����", rand);
                UiManager.Instance.FirstRandomSelectBtn.onClick.AddListener(() => BtnOnClickEvent(0, 0, 0, 0, 3, 0));
                UiManager.Instance.SecondRandomSelectBtn.onClick.AddListener(() => BtnOnClickEvent(0, 0, 0, 0, 0, 3));
                break;
            case 2:
                BtnTextSet("�����", "���� �ʴ´�", rand);
                UiManager.Instance.FirstRandomSelectBtn.onClick.AddListener(() => BtnOnClickEvent(0, 0.1f, 0, 0, 0, 0));
                UiManager.Instance.SecondRandomSelectBtn.onClick.AddListener(() => BtnOnClickEvent(0, 0, 0, 0, 3, 0));
                break;
            case 3:
                BtnTextSet("�����ش�", "���� �ʴ´�", rand);
                UiManager.Instance.FirstRandomSelectBtn.onClick.AddListener(() => BtnOnClickEvent(0, 0.4f, 0, 0, 0, 0));
                UiManager.Instance.SecondRandomSelectBtn.onClick.AddListener(() => BtnOnClickEvent(0, 0, 0, 0, 0, 3));
                break;
            case 4:
                BtnTextSet("��������ȸ��", "����������ȸ��", rand);
                UiManager.Instance.FirstRandomSelectBtn.onClick.AddListener(() => BtnOnClickEvent(0, 0, 0, 0, 0, 0));
                UiManager.Instance.SecondRandomSelectBtn.onClick.AddListener(() => BtnOnClickEvent(0, 0.3f, 0, 0, 0, 0));
                break;
            case 5:
                BtnTextSet("������ ��", "������ ��", rand);
                UiManager.Instance.FirstRandomSelectBtn.onClick.AddListener(() => BtnOnClickEvent(0, 0, 0.1f, 0, 0, 0));
                UiManager.Instance.SecondRandomSelectBtn.onClick.AddListener(() => BtnOnClickEvent(0, 0, 0, 0.1f, 0, 0));
                break;
            case 6:
                yield return new WaitForSeconds(1.0f);
                UiManager.Instance.BlessingSelectBtn.gameObject.SetActive(true);
                break;
        }
    }

    public void BtnTextSet(string firstText, string SecondText, int rand)
    {
        UiManager.Instance.FirstRandomSelectText.text = firstText;
        UiManager.Instance.SecondRandomSelectText.text = SecondText;
        UiManager.Instance.FirstRandomSelectBtn.onClick.AddListener(() => StartCoroutine(PrintRandomSelectResult(FirstRandomResultDialogData, rand)));
        UiManager.Instance.SecondRandomSelectBtn.onClick.AddListener(() => StartCoroutine(PrintRandomSelectResult(SecondRandomResultDialogData, rand)));
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

    public IEnumerator PrintRandomSelectResult(Dictionary<int, string[]> DialogData, int rand)
    {
        UiManager.Instance.DialogText.text = "";
        for (int i = 0; i < DialogData[rand].Length; i++)
        {                        
            yield return new WaitUntil(() => DialogFlag == true);
            {
                DialogFlag = false;
                getDialog(DialogData, rand, i);
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
            if(PlayerTable.Instance.ThirdSkillAvailableCount > PlayerTable.Instance.thirdCount) PlayerTable.Instance.ThirdSkillAvailableCount = PlayerTable.Instance.thirdCount;    
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

