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
    public static TheManager Instance { get; private set; }

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
    private GameState priorState; // for resuming from pause
    private System.Object inGameMenuHolder; // the object holding open the current In Game Menu

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
        private set
        {
            var oldState = _gameState;
            _gameState = value;
            stateChangeListeners.Invoke(oldState, _gameState);
        }
    }

    public GameObject pauseMenu;
    private float oldTimeScale;

    public bool TryCutsceneStart()
    {
        if (gameState == GameState.Paused) return false;
        priorState = gameState;
        gameState = GameState.Cutscene;
        return true;
    }

    public void Pause()
    {
        priorState = gameState;
        gameState = GameState.Paused;
        // TODO: move this to a separate script?
        pauseMenu.SetActive(true);
        oldTimeScale = Time.timeScale;
        Time.timeScale = 0.0f;
    }

    public void Resume()
    {
        gameState = priorState;
        priorState = GameState.Null;
        // TODO: move this to a separate script?
        pauseMenu.SetActive(false);
        Time.timeScale = oldTimeScale;
    }

    public bool TryOpenInGameMenu(System.Object holder)
    {
        if (gameState == GameState.Paused) return false;
        // TODO: ingamemenu hierarchy
        inGameMenuHolder = holder;
        gameState = GameState.InGameMenu;
        return true;
    }

    public bool TryCloseInGameMenu(System.Object holder)
    {
        if (gameState == GameState.InGameMenu)
        {
            if (holder != inGameMenuHolder)
            {
                Debug.LogError("Non-menu holder tried to close menu");
                return false;
            }
            gameState = GameState.Playing;
            return true;
        }
        return false;
    }

    #endregion

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
    }

    public void Log(string msg)
    {
        Debug.Log(msg);
    }
}
