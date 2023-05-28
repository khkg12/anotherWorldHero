using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneObj : MonoBehaviour
{
    public SpriteRenderer GoddessSprite;
    public SpriteRenderer MonsterSprite;
    public Animator MonsterObjAni;
    string MonsterTrigger;    

    private void Start()
    {                
        StartCoroutine(UpdateCoroutine());        
    }

    private IEnumerator UpdateCoroutine()
    {        
        while (true)
        {
            if (GameManager.Instance.NowRound == 1)
            {
                GoddessSprite.gameObject.SetActive(true);
            }
            else
            {
                GoddessSprite.gameObject.SetActive(false);
            }
            switch (DataManager.Instance.sceneData[GameManager.Instance.NowRound-1].Situation)
            {
                case "Battle":                
                    MonsterSprite.gameObject.SetActive(true);
                    MonsterSprite.sprite = MonsterTable.Instance.MonsterList[MonsterTable.Instance.MonsterNum].MonsterSprite;                                        
                    if(GameManager.Instance.IsAni == true)
                    {
                        MonsterTrigger = GameManager.Instance.IsMonsterDead == true ? "IsDead" : "IsAppear";
                        MonsterObjAni.SetTrigger(MonsterTrigger);
                        GameManager.Instance.IsMonsterDead = false;
                        GameManager.Instance.IsAni = false;
                    }                    
                    break;
                default:
                    MonsterSprite.gameObject.SetActive(false);
                    break;
            }
            yield return new WaitForEndOfFrame();
        }
    }    
}
