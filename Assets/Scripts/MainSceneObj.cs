using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneObj : MonoBehaviour
{
    public SpriteRenderer GoddessSprite;
    public SpriteRenderer MonsterSprite;
    public SpriteRenderer BossSprite;
    public Animator MonsterObjAni;
    string MonsterTrigger;    

    private void Start()
    {                
        StartCoroutine(UpdateCoroutine());        
    }

    /*private IEnumerator UpdateCoroutine()
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
    }*/

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {            
            switch (DataManager.Instance.sceneData[GameManager.Instance.NowRound - 1].standingCg)
            {
                case "Goddess":
                    GoddessSprite.gameObject.SetActive(true); // 스프라이트 enum or 배열에 idle, shy, happy 등을 넣어두고 가져오기
                    break;
                case "Monster":
                    MonsterSprite.gameObject.SetActive(true);
                    MonsterSprite.sprite = MonsterTable.Instance.MonsterList[MonsterTable.Instance.MonsterNum].MonsterSprite;
                    if (GameManager.Instance.IsAni == true)
                    {
                        MonsterTrigger = GameManager.Instance.IsMonsterDead == true ? "IsDead" : "IsAppear";
                        MonsterObjAni.SetTrigger(MonsterTrigger);
                        GameManager.Instance.IsMonsterDead = false;
                        GameManager.Instance.IsAni = false;
                    }
                    break;
                case "Boss":
                    BossSprite.gameObject.SetActive(true);
                    BossSprite.sprite = MonsterTable.Instance.MonsterList[MonsterTable.Instance.MonsterNum].MonsterSprite;
                    break;
                default:
                    MonsterSprite.gameObject.SetActive(false);
                    GoddessSprite.gameObject.SetActive(false);
                    BossSprite.gameObject.SetActive(false);
                    break;
            }
            yield return new WaitForEndOfFrame();
        }
    }  
}
