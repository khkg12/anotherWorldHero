using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGround : MonoBehaviour
{
    public SpriteRenderer BackGroundSprite;
    private List<BgImage> BgImageList; // 보스를 잡고 액트의 넘버가 변경될 때 act에 맞는 리스트로 변경하는 함수를 event에 등록해서 사용해볼것 

    private void Start()
    {
        StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            switch (GameManager.Instance.NowRound)
            { // case 0 이 없는 이유 : nextDialog함수에서 현재라운드에 해당하는 대화를 실행시키는 것에 동시에 NowRound를 증가시키기 때문, 즉 case 1이 case 0을 뜻함
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                    BackGroundSprite.sprite = BackGroundTable.Instance.MainBackGroundImageList[GameManager.Instance.NowRound - 1].bgSprite;                    
                    break;
            }                        
            yield return new WaitForSeconds(0.05f);
        }
    }    
}
