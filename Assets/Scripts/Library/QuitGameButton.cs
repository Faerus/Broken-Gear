using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGameButton : MonoBehaviour
{
    private void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(this.QuitGame);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Delete))
        {
            this.QuitGame();
        }
    }

    private void QuitGame()
    {
        SceneLoader.Load(Scenes.Menu);
    }
}
