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
        RestartBtn.onClick.AddListener(() => LoadMainScene()); // 스타트 씬 추가하면 스타트씬으로 변경
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}

