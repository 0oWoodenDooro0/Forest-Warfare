using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LoadGame : MonoBehaviour
{
    public GameObject panel;
    public GameObject[] backgrounds;
    public GameObject[] charaters;
    private GameObject _background;
    private Camera _camera;

    private void Start()
    {
        _camera = gameObject.GetComponent<Camera>();
        _background = backgrounds[Random.Range(0, backgrounds.Length)];
        var canvas = _background.GetComponent<Canvas>();
        canvas.worldCamera = _camera;
        canvas = panel.GetComponent<Canvas>();
        canvas.worldCamera = _camera;
        var player1Index = StartClick.GameData.Player1Index == -1 ? Random.Range(0, charaters.Length) : StartClick.GameData.Player1Index;
        var player2Index = StartClick.GameData.Player2Index == -1 ? Random.Range(0, charaters.Length) : StartClick.GameData.Player2Index;
        Instantiate(panel, Vector3.zero, new Quaternion(0, 0, 0, 0));
        Instantiate(_background, Vector3.zero, new Quaternion(0, 0, 0, 0));
        Instantiate(charaters[player1Index], new Vector3(-5, -3, 0), new Quaternion(0, 0, 0, 0));
        var player2 = Instantiate(charaters[player2Index], new Vector3(5, -3, 0), new Quaternion(0, 0, 0, 0));
        player2.tag = "Player2";
    }
}