using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{
    private void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(this.StartGameIn3);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            this.StartGameIn3();
        }
    }

    private void StartGameIn3()
    {
        this.Invoke("StartGame", 2);
    }

    private void StartGame()
    {
        SceneLoader.Load(Scenes.Game);
    }
}
