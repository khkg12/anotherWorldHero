using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBackGround : MonoBehaviour
{
    public SpriteRenderer BackGroundSprite;
    void Start()
    {
        BackGroundSprite.sprite = BackGroundTable.Instance.BattleBackGroundImageList[MonsterTable.Instance.MonsterNum].bgSprite;
    }
}
