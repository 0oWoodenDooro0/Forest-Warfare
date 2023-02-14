using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartClick : MonoBehaviour
{
    public Button button;
    public Button[] player1Character;
    public Button[] player2Character;

    private void Start()
    {
        GameData.Player1Index = -1;
        GameData.Player2Index = -1;
        for (var i = 0; i < player1Character.Length; i++)
        {
            var index = i;
            player1Character[i].onClick.AddListener((() => ChooseCharacter(player1Character, index)));
        }

        for (var i = 0; i < player2Character.Length; i++)
        {
            var index = i;
            player2Character[i].onClick.AddListener((() => ChooseCharacter(player2Character, index)));
        }
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

    private void ChooseCharacter(Button[] arr, int buttonIndex)
    {
        switch (arr)
        {
            case var _ when arr == player1Character:
                GameData.Player1Index = buttonIndex;
                break;
            case var _ when arr == player2Character:
                GameData.Player2Index = buttonIndex;
                break;
        }
    }

    public static class GameData
    {
        public static int Player1Index { get; set; }
        public static int Player2Index { get; set; }
    }
}