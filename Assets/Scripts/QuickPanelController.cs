using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// The QuickPanelController controls the appearance and use of
/// the Quick Menu.
/// </summary>
public class QuickPanelController : MonoBehaviour {
    public GameObject quickMenu;
    public GameObject quickSelector;
    public GameObject[] quickMenuPanels;

    // 0 isn't actually valid but we want warnings about the defaults
    // for some reason, using int.MaxValue causes it to go negative in unity,
    // and using uint makes it not move at all.
    [Range(0, 100)]
    public uint width = 0;
    [Range(0, 100)]
    public uint height = 0;
    public GridAddress posn;
    private bool positionDirty = true;

    /// <summary>
    /// checks if the visual container is active.
    /// </summary>
    private bool isOpen { get { return quickMenu.activeInHierarchy; } }

    void Close()
    {
        quickMenu.SetActive(false);
    }

    void Open()
    {
        quickMenu.SetActive(true);
    }

    void Start()
    {
        posn = new GridAddress((uint)width, (uint)height);
        TheManager.Instance.stateChangeListeners.AddListener(this.OnGameStateChange);
        SetupRefs();
        Validate();

        if (isOpen)
        {
            Open();
        }
        else
        {
            switch (TheManager.Instance.gameState)
            {
                case GameState.Paused:
                case GameState.InGameMenu:
                case GameState.Cutscene:
                    this.enabled = false;
                    Close();
                    break;
                case GameState.Playing:
                    Close();
                    break;
            }
        }
    }

    [ContextMenu("Find QuickMenu and Panels")]
    void SetupRefs()
    {
        if (quickMenu == null)
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.name.StartsWith("QuickMenu"))
                {
                    Debug.Log("Found quick menu");
                    quickMenu = child.gameObject;
                    break;
                }
            }
        }

        if (quickMenuPanels.Length == 0)
        {
            List<GameObject> panels = new List<GameObject>();
            // TODO: if we go to another more nested structure, this will have to change
            foreach (Transform child in quickMenu.transform)
            {
                if (child.gameObject.name.StartsWith("Panel"))
                {
                    Debug.Log("Found panel");
                    panels.Add(child.gameObject);
                }
                else if (quickSelector == null && child.gameObject.name.StartsWith("Selector"))
                {
                    Debug.Log("Found selector");
                    quickSelector = child.gameObject;
                }
            }
            quickMenuPanels = panels.ToArray();
        }
    }

    [ContextMenu("Validate")]
    void Validate()
    {
        if (quickMenu == null)
            Debug.LogError("No quick menu found for QuickMenuController", this);

        if (quickMenuPanels == null || quickMenuPanels.Length == 0)
            Debug.LogError("No quick menu panels found for QuickMenuController", this);

        if (width == 0 || height == 0)
            Debug.LogError("0 width or height for QuickMenuController", this);
    }
    void OnValidate() { Validate(); }

    void ProcessMovementInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            posn.Right();
            positionDirty = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            posn.Left();
            positionDirty = true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            posn.Down();
            positionDirty = true;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            posn.Up();
            positionDirty = true;
        }

        UpdatePosition();
    }

    // state transition strategy: change our state first, then change global state
    // so we know who the request came from
    void Update()
    {
        switch (TheManager.Instance.gameState)
        {
            case GameState.Playing:
                if (Input.GetButtonDown("Quick Menu"))
                {
                    if (TheManager.Instance.TryOpenInGameMenu(this))
                    {
                        Open();
                    }
                }
                break;
            case GameState.InGameMenu:
                if (isOpen)
                {
                    if (Input.GetButtonDown("Quick Menu"))
                    {
                        if (TheManager.Instance.TryCloseInGameMenu(this))
                        {
                            Close();
                            return;
                        }
                    }
                    ProcessMovementInput();
                }
                break;
            case GameState.Null:
            case GameState.Paused:
            case GameState.Cutscene:
            default:
                break;
        }
    }

    public void OnGameStateChange(GameState oldState, GameState newState)
    {
        switch (newState)
        {
            case GameState.Paused:
                // stop listening for input, but remain open for when we go back.
                this.enabled = false;
                break;
            case GameState.Cutscene:
                Close();
                this.enabled = false;
                break;
            case GameState.InGameMenu:
                // checks to see if we're in this because of this object so we coo
            case GameState.Playing:
                this.enabled = true;
                break;
        }
    }

    private void UpdatePosition()
    {
        if (!positionDirty) return;
        Debug.Log("position " + posn.Index + ", " + Time.frameCount);
        quickSelector.transform.localPosition = quickMenuPanels[posn.Index].transform.localPosition;
        positionDirty = false;
    }
}
