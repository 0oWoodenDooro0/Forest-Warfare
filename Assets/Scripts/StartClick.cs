using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartClick : MonoBehaviour
{
    public Button button;

    private void Start()
    {
        button.onClick.AddListener(OnStartClick);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void OnStartClick()
    {
        SceneManager.LoadScene("Game");
    }
}