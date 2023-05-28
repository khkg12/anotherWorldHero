using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGround : MonoBehaviour
{
    public SpriteRenderer BackGroundSprite;
    private List<BgImage> BgImageList; // ������ ��� ��Ʈ�� �ѹ��� ����� �� act�� �´� ����Ʈ�� �����ϴ� �Լ��� event�� ����ؼ� ����غ��� 

    private void Start()
    {
        StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            switch (GameManager.Instance.NowRound)
            { // case 0 �� ���� ���� : nextDialog�Լ����� ������忡 �ش��ϴ� ��ȭ�� �����Ű�� �Ϳ� ���ÿ� NowRound�� ������Ű�� ����, �� case 1�� case 0�� ����
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
