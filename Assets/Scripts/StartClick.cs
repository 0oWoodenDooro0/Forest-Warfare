using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartClick : MonoBehaviour
{
    private Button _button;

    private void Start()
    {
        _button = gameObject.GetComponent<Button>();
        _button.onClick.AddListener(OnStartClick);
    }

    private void OnStartClick()
    {
        SceneManager.LoadScene("Game");
    }
}