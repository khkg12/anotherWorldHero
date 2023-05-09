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
    
    // 스토리 텍스트 저장 오브젝트
    public Dictionary<int, string[]> DialogData;
    // 랜덤 이벤트 텍스트 저장 오브젝트
    public Dictionary<int, string[]> RandomDialogData;
    // 랜덤 이벤트 첫번째 버튼 결과 텍스트 저장 오브젝트
    public Dictionary<int, string[]> FirstRandomResultDialogData;
    // 랜덤 이벤트 두번째 버튼 결과 텍스트 저장 오브젝트
    public Dictionary<int, string[]> SecondRandomResultDialogData;
    // 랜덤 이벤트 인덱스 리스트
    public List<int> RandomEventList;
    
    public bool DialogFlag = true;
    public bool ClickFlag = false;
    public bool SkipFlag = false;    

    public TextMeshProUGUI talkerName;
    public GameObject CliokAlarm;

    void Awake()
    {                
        var thisObj = FindObjectsOfType<DialogManager>();  // FindObjectsOfType : 해당하는 타입의 오브젝트를 찾아서 배열로줌
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

    public void StartDialogData() // 일반 대화창 저장 함수
    {
        DialogData.Add(0, new string[] { "안녕? 반가워 정신이 좀 드니? 나는 여신이야.", "\n넌 지금 교통사고로 인해 혼수상태에 빠져있지.", "\n너가 이세계의 마왕을 처리해주면 너를 건강한 몸으로 혼수상태에서 깨어나게 해줄게." });
        DialogData.Add(1, new string[] { "마왕성 입구에 도착했다!!", "\n살벌한 분위기를 풍긴다", "\n문을 열자 거대한 소리를 내며 문은 천천히 열리기 시작했다." });
        DialogData.Add(2, new string[] { "적이 등장했다!!", "\n마왕성 1층의 입구를 지키는 하급 마족기사다.", "\n기다란 검을 들고 마족기사는 천천히 당신의 앞으로 다가왔다." });
        DialogData.Add(3, new string[] { "하급 마족기사를 쓰러뜨렸다!", "\n쉽지 않은 상대였지만 여신이 준 치트같은 힘은 상상을 초월했다.", "\n당신은 지친 숨을 고르며 마왕성 복도를 걷기 시작했다." });
        DialogData.Add(4, new string[] { "어디선가 강력한 화살이 날아왔다!", "\n초인적인 신체능력으로 화살을 회피하자 화살의 주인이 모습을 드러냈다.", "\n마족 궁사는 활시위를 겨누며 전투의 시작을 알렸다." });
        DialogData.Add(5, new string[] { "마족 궁수를 쓰러뜨렸다!", "\n그의 화살은 매우 날카로웠지만 그게 전부였다.", "\n당신은 지친 숨을 고르며 마왕성 복도를 걷기 시작했다." });
        DialogData.Add(6, new string[] { "우연히 발견한 서고에 들어갔다.", "\n수 많은 책들 중 하나의 책에서 강렬한 기운이 느껴졌다.", "\n당신은 그 책을 들어 펼쳐보았다." });
        DialogData.Add(7, new string[] { "날카로워진 촉각이 강렬한 기운을 탐지했다!", "\n멀리서 한 인영의 실루엣이 보인다.", "\n마족 주술사는 주문을 외우며 당신을 향해 걸어오고 있다." });
        DialogData.Add(8, new string[] { "마족 주술사를 쓰러뜨렸다!", "\n그의 마법을 겨우 막아내고 그의 목을 베었다.", "\n당신은 지친 숨을 고르며 마왕성 복도를 걷기 시작했다." });
        DialogData.Add(9, new string[] { "거대한 문이 가로막혀 있다.", "\n주변엔 아무런 기척이 없다. 당신은 화톳불을 피웠다.", "\n당신은 휴식을 취했다." });
    }

    public void StartRandomDialogData() // 랜덤 대화창 저장 함수
    {
        RandomDialogData.Add(0, new string[] { "앞으로 나아가려는데 바닥에 있던 열매가 발에 부딪혔다.", "\n낯선 형태의 열매에서는 알 수 없는 냄새가 나지만, 묘하게 맛있는 냄새인 것도 같다.", "\n어떻게 할까?"});
        RandomDialogData.Add(1, new string[] { "우연히 발견한 창고에 들어갔다.","\n벽면 선반을 가득 채운 형형색색의 포션들이 가득하다.", "\n그 중에서 유독 특별해 보이는 포션들을 발견했다."});
        RandomDialogData.Add(2, new string[] { "보물 상자를 발견했다.", "\n무게가 제법 묵직하다."});
        RandomDialogData.Add(3, new string[] { "어디선가 서글프게 우는 소리가 들린다.", "\n울음 소리가 나는 곳을 찾아가보니 구석 한편에서 숨죽여 울고 있는 소녀를 발견했다." });
        RandomDialogData.Add(4, new string[] { "숨겨진 트랩이 발동하면서 수십 개의 화살이 비처럼 쏟아져내렸다."});
        RandomDialogData.Add(5, new string[] { "우연히 발견한 서고에 들어갔다.", "\n책들이 빼곡히 꽂힌 서가를 거닐다 눈에 띄는 책을 발견했다." });
        RandomDialogData.Add(6, new string[] { "성스러운 기운을 내뿜는 검 한 자루가 땅에 굳건히 꽂혀있다.", "\n검의 손잡이를 잡으니 익숙한 힘이 느껴졌고, 이것이 전대 용사의 검이라는 사실을 알게 되었다.", "\n전대 용사의 축복이 흘러들어왔다."});
    }

    public void StartFirstRandomResultDialogData() // 랜덤 선택지 첫번째 결과 대화창 저장
    {
        FirstRandomResultDialogData.Add(0, new string[] { "참지 못하고 일단 열매를 먹었다.", "\n열매는 달콤한 맛이 났으며 기력을 회복시켜 주었다.", $"\n최대체력의 30%를 회복하였다!({PlayerTable.Instance.MaxHp * 0.3f}) 회복" });
        FirstRandomResultDialogData.Add(1, new string[] { "빨간 물약을 마셨다.", "\n물약을 마시는 순간 온 신경이 집중되었다.", "\n치명타 확률이 3% 증가하였다!" });
        FirstRandomResultDialogData.Add(2, new string[] { "미믹이었다!.", "\n최대체력의 10%가 감소하였다." });
        FirstRandomResultDialogData.Add(3, new string[] { "소녀에게 가까이 다가갔다.", "\n그제서야 소녀가 몬스터인것을 깨달았다!", "\n몬스터에게 기습을 당하였다!", "\n최대 체력의 40%가 감소하였다." });
        FirstRandomResultDialogData.Add(4, new string[] { "왼쪽으로 회피 하였더니 2~3발의 화살만이 날라왔다.", "\n쉽게 방어해낼 수 있었다." });
        FirstRandomResultDialogData.Add(5, new string[] { "진실이라는 단어가 적힌 책을 펼쳤다.", "\n그 순간 몸에 알 수 없는 강력한 기운이 들어왔다.", "\n공격력이 10% 증가하였다!" });
        
    }

    public void StartSecondRandomResultDialogData() // 랜덤 선택지 두번째 결과 대화창 저장
    {
        SecondRandomResultDialogData.Add(0, new string[] { "혹시 몰라 열매를 버리고 지나갔다", "\n배는 고팠지만 최선의 선택이라 생각했다." });
        SecondRandomResultDialogData.Add(1, new string[] { "파란 물약을 마셨다.", "\n물약을 마시는 순간 몸놀림이 매우 가벼워졌다.", "\n회피율이 3% 증가하였다!" });
        SecondRandomResultDialogData.Add(2, new string[] { "자세히 살펴보니 보물 상자 행세를 하는 미믹이었다.", "\n미믹이 공격하기 전에 선수를 쳐 치명상을 입힐 수 있었다.", "\n치명타 확률이 3% 증가하였다!" });
        SecondRandomResultDialogData.Add(3, new string[] { "자세히 살펴보니 소녀 행세를 하는 몬스터였다.", "\n몬스터가 기습했지만, 미리 간파한 덕에 공격을 피할 수 있었다.", "\n회피율이 3% 증가하였다!" });
        SecondRandomResultDialogData.Add(4, new string[] { "오른쪽으로 회피했더니 내가 있는 방향으로 대부분의 화살이 날아왔다!", "\n화살을 쳐냈지만 모든 화살을 막을 순 없었다.", "\n최대 체력의 30%가 감소하였다." });
        SecondRandomResultDialogData.Add(5, new string[] { "영웅이라는 단어가 적힌 책을 펼쳤다.", "\n그 순간 몸에 알 수 없는 강력한 기운이 들어왔다.", "\n방어력이 10% 증가하였다!" });
        
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
            // 마우스클릭시 UiManager를 sceneData.Dialog[lineIndex].Line로 변경 foreach문 break;
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
        if(lineIndex < LastIndex - 1) CliokAlarm.gameObject.SetActive(true); // 대화의 마지막은 클릭알람을 띄워선 안되므로 현재의 대화인덱스가 마지막대화인덱스보다 작을때만
        UiManager.Instance.DialogText.text += "\n";
        SkipFlag = true;        
    }

    public IEnumerator nextDialog(int NowRound)
    {        
        RandomBackGround.gameObject.SetActive(false);
        GameManager.Instance.NowRound += 1;  // 함수 실행 후 다음에 또 실행 시 다음라운드 스트링을 출력하기 위해 미리 하나올려둠
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
            case "Blessing": // case추가 예를들어 아래코드를 진행해야 하는 NowRound가 10이면 case : 10 추가
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
                MonsterTable.Instance.MonsterNum += 1; // 전투에서 승리하고 다음 라운드로 넘어간 뒤 MonsterNum을 올려줌
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
                MonsterTable.Instance.MonsterNum += 1; // 전투에서 승리하고 다음 라운드로 넘어간 뒤 MonsterNum을 올려줌
                GameManager.Instance.IsAni = true;
                break;
        }
    }

    
    public void NextPage<T>(Image UiImage, List<T> list) // 특성, 스킬, 아이템 창의 계속하기 버튼, 위와 따로분리해둔 이유는 아래 주석이 이유
    {        
        StartCoroutine(nextDialog(GameManager.Instance.NowRound));
        list = list.OrderBy(i => Random.value).ToList(); // start에서만 한번 랜덤화 되기 때문에 대화가종료되고 아이템 선택 시 한번 더 랜덤화 시켜 다음 등장 시 동일하지 않도록 함
        UiImage.gameObject.SetActive(false);        
    }
    
    public SpriteRenderer RandomBackGround;
    
    // 테스트용 코드

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
        for (int i = 0; i < size; i++) // jsonData로 변경
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

        float[] RecoveryAmount = DataManager.Instance.RandomSceneData[rand].RecoveryAmount;  // 두개의 선택지를 위해 배열로      
        float[] ReduceAmount = DataManager.Instance.RandomSceneData[rand].ReduceAmount;
        float[] AtkIncPer = DataManager.Instance.RandomSceneData[rand].AtkIncPer;
        float[] DefIncPer = DataManager.Instance.RandomSceneData[rand].DefIncPer;
        int[] Cri = DataManager.Instance.RandomSceneData[rand].Cri;
        int[] Dod = DataManager.Instance.RandomSceneData[rand].Dod;        
        UiManager.Instance.FirstRandomSelectBtn.onClick.AddListener(() => BtnOnClickEvent(RecoveryAmount[0], ReduceAmount[0], AtkIncPer[0], DefIncPer[0], Cri[0], Dod[0])); // HP회복량, HP감소량, 공격력 증가량, 방어력 증가량, 치명타, 회피율                
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



