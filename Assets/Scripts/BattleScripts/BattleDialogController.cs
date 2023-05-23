using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogController : MonoBehaviour
{
    [SerializeField]
    private Scrollbar scrollbar;
    private float PreviousSize;

    private void Update()
    {
        if(scrollbar.size < 1 && PreviousSize != scrollbar.size) // ��ũ�ѹ� ����� ���������� ��, �� ��簡 �߰����� ���� �� ��ũ�� ������ �� �ְ� �����߰�, ��簡 �߰��Ǿ� ��ũ�� ����� ����Ǿ��ٸ� �� �� ��ũ����ġ�� 0����
        {
            scrollbar.value = 0;
            PreviousSize = scrollbar.size;
        }
    }
}
