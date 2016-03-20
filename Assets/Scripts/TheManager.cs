using UnityEngine;
using System.Collections;

public enum GameState
{
    Playing,
    Paused
}

public class TheManager : MonoBehaviour {

    public GameState gameState;
    public GameObject pauseMenu;

    // Use this for initialization
    void Start () {
	
	}

    public void Pause()
    {
        gameState = GameState.Paused;
        pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        gameState = GameState.Playing;
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            switch (gameState)
            {
                case GameState.Playing:
                    Pause();
                    break;
                case GameState.Paused:
                    Resume();
                    break;
                default:
                    break;
            }
    }
}
