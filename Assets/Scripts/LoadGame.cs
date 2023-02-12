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
        Instantiate(panel, Vector3.zero, new Quaternion(0, 0, 0, 0));
        Instantiate(_background, Vector3.zero, new Quaternion(0, 0, 0, 0));
        Instantiate(charaters[Random.Range(0, charaters.Length)], new Vector3(-5, -1, 0), new Quaternion(0, 0, 0, 0));
        var player2 = Instantiate(charaters[Random.Range(0, charaters.Length)], new Vector3(5, -1, 0), new Quaternion(0, 0, 0, 0));
        player2.tag = "Player2";
    }
}