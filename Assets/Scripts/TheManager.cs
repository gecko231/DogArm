using UnityEngine;
using System.Collections;

public enum GameState
{
    Playing,
    Paused,
    Cutscene
}

public class TheManager : MonoBehaviour {

    public GameState gameState;
    public GameObject pauseMenu;
    private float oldTimeScale;

    void Start () { }

    public void Log(string msg)
    {
        Debug.Log(msg);
    }

    public void CutsceneStart()
    {
        gameState = GameState.Cutscene;
    }

    public void Pause()
    {
        gameState = GameState.Paused;
        pauseMenu.SetActive(true);
        oldTimeScale = Time.timeScale;
        Time.timeScale = 0.0f;
    }

    public void Resume()
    {
        gameState = GameState.Playing;
        pauseMenu.SetActive(false);
        Time.timeScale = oldTimeScale;
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
