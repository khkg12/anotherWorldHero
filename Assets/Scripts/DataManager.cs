using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using TMPro;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

public struct Dialogs
{
    public string Talker;
    public string Line;
}
public class SceneData
{
    public int Round;
    public string Situation;
    public string standingCg;
    public Dialogs[] Dialog;
    public Dialogs[] selectDialog;
}

public class RandomSceneData
{
    public int Index;
    public string FirstBtn;
    public string SecondBtn;
    public float[] RecoveryAmount;
    public float[] ReduceAmount;
    public float[] AtkIncPer;
    public float[] DefIncPer;
    public int[] Cri;
    public int[] Dod;
    public Dialogs[] Dialog;
    public Dialogs[] FirstDialog;
    public Dialogs[] SecondDialog;
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


    [SerializeField]
    private TextAsset FirstActDialogFile;
    [SerializeField]
    private TextAsset SecondActDialogFile;
    [SerializeField]
    private TextAsset ThirdActDialogFile;
    [SerializeField]
    private TextAsset FourthActDialogFile;


    public SceneData[] sceneData;

    [SerializeField]
    private TextAsset RandomDialogFile;
    public RandomSceneData[] RandomSceneData;
    public TextMeshProUGUI talkerName;    

    private void OnEnable()
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

        sceneData = JsonConvert.DeserializeObject<SceneData[]>(FirstActDialogFile.text);  // 대사저장
        RandomSceneData = JsonConvert.DeserializeObject<RandomSceneData[]>(RandomDialogFile.text); // 랜덤이벤트 대사저장
        DialogManager.Instance.ActChangeAction += NowActChangeEvent; 
    }

    private void NowActChangeEvent()
    {
        switch (GameManager.Instance.NowAct)
        {
            case 2:
                sceneData = JsonConvert.DeserializeObject<SceneData[]>(SecondActDialogFile.text);
                break;
            case 3:
                sceneData = JsonConvert.DeserializeObject<SceneData[]>(ThirdActDialogFile.text);
                break;
            case 4:
                sceneData = JsonConvert.DeserializeObject<SceneData[]>(FourthActDialogFile.text);
                break;
        }        
    }
}


