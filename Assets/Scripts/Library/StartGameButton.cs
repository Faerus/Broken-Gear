using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{
    private void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(this.StartGame);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            this.StartGame();
        }
    }

    private void StartGame()
    {
        SceneLoader.Load(Scenes.Game);
    }
}
