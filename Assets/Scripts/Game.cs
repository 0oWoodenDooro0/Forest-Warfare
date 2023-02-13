using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static bool IsGameOver;
    public static int Player1Blood;
    public static int Player2Blood;
    public static bool Player1God;
    public static bool Player2God;
    public Image leftBlood;
    public Image rightBlood;
    public Text time;
    private int _maxBlood;
    private float _time;

    private void Start()
    {
        Player1God = false;
        Player2God = false;
        IsGameOver = false;
        _time = 121;
        _maxBlood = 100;
        Player1Blood = _maxBlood;
        Player2Blood = _maxBlood;
    }

    private void Update()
    {
        if (!IsGameOver)
        {
            _time -= Time.deltaTime;
            if (_time < 0)
            {
                _time = 0;
                IsGameOver = true;
            }

            time.text = ((int)_time).ToString();
        }

        leftBlood.fillAmount = (float)Player1Blood / _maxBlood;
        rightBlood.fillAmount = (float)Player2Blood / _maxBlood;
    }
}