using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dead : MonoBehaviour
{
    public Button RestartBtn;
    void Start()
    {
        RestartBtn.onClick.AddListener(() => LoadMainScene()); // ��ŸƮ �� �߰��ϸ� ��ŸƮ������ ����
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}

