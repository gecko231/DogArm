using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Global Game State. This is for stuff that reflects upon overall state and control
/// of the game, not just individual components.
/// </summary>
public enum GameState
{
    /// <summary>
    /// This should never occur from normal code usage: it represents a default value
    /// since enums are value types, and we can't make them nullable with this version
    /// of mono.
    /// </summary>
    Null,
    Playing,
    Paused,
    Cutscene,
    /// <summary>
    /// Something like an in-world GUI, like the quick select menu.
    /// </summary>
    InGameMenu,
}

/// <summary>
/// An event triggered by game state changing.
/// Arguments are the old state and new state, respectively.
/// </summary>
public class GameStateChange : UnityEvent<GameState, GameState> { };

public class TheManager : MonoBehaviour {
    private static TheManager instance;
    public static TheManager Instance { get { return instance; } }

    /// <summary>
    /// This section tracks the current overall game state,
    /// its history, and notifies listeners about changes.
    /// This is experimental, but if it's useful, we may factor
    /// it out into its own generic class.
    /// </summary>
    #region Game State Management

    /// <summary>
    /// Current game state.
    /// Still public so that it is exposed in the editor.
    /// Don't set this from within code.
    /// </summary>
    public GameState _gameState = GameState.Playing;

    private GameState oldState;

    /// <summary>
    /// A list of event listeners for state changes.
    /// Use this if it simplifies your code, but if you really need to check each
    /// Update, you can just keep a reference to TheManager.
    /// </summary>
    public GameStateChange stateChangeListeners = new GameStateChange();

    public GameState gameState
    {
        get
        {
            return _gameState;
        }
        set
        {
            oldState = _gameState;
            _gameState = value;
            stateChangeListeners.Invoke(oldState, _gameState);
        }
    }

    public GameObject pauseMenu;
    private float oldTimeScale;

    public void CutsceneStart()
    {
        gameState = GameState.Cutscene;
    }

    public void Pause()
    {
        gameState = GameState.Paused;
        // TODO: move this to a separate script?
        pauseMenu.SetActive(true);
        oldTimeScale = Time.timeScale;
        Time.timeScale = 0.0f;
    }

    public void Resume()
    {
        gameState = oldState;
        // TODO: move this to a separate script?
        pauseMenu.SetActive(false);
        Time.timeScale = oldTimeScale;
    }

    public void OpenInGameMenu()
    {
        // this just makes using Resume() easier
        oldTimeScale = Time.timeScale;
        gameState = GameState.InGameMenu;
    }

    #endregion

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetAxis("Cancel") > 0.0)
        {
            switch (gameState)
            {
                case GameState.Playing:
                case GameState.InGameMenu:
                case GameState.Cutscene:
                    Pause();
                    break;
                case GameState.Paused:
                    Resume();
                    break;
                default:
                    break;
            }
        }
        else if (Input.GetAxis("Quick Menu") > 0.0)
        {
            switch (gameState)
            {
                case GameState.Playing:
                    OpenInGameMenu();
                    break;
                case GameState.InGameMenu:
                    //TODO: make separate?
                    Resume();
                    break;
                case GameState.Paused:
                case GameState.Cutscene:
                case GameState.Null:
                default:
                    // nada
                    break;
            }
        }
    }

    public void Log(string msg)
    {
        Debug.Log(msg);
    }
}
