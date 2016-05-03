using UnityEngine;
using System.Collections.Generic;

public enum QuickPanelState
{
    Closed,
    Open,
}

public class QuickPanelController : MonoBehaviour {
    public GameObject quickMenu;
    public GameObject quickSelector;
    public GameObject[] quickMenuPanels;
    [Range(1, int.MaxValue)]
    public int width;
    [Range(1, int.MaxValue)]
    public int height;
    public GridAddress posn;
    private bool positionDirty = true;
    public QuickPanelState state;

    // checks if the visual container is active. DOES NOT
    // check our state
    private bool isOpen { get { return quickMenu.activeInHierarchy; } }

    void Close()
    {
        quickMenu.SetActive(false);
        state = QuickPanelState.Closed;
        this.enabled = true;
    }

    void CloseSilenced()
    {
        quickMenu.SetActive(false);
        this.enabled = false;
    }

    void Open()
    {
        quickMenu.SetActive(true);
        state = QuickPanelState.Open;
        this.enabled = true;
    }

    void Start()
    {
        posn = new GridAddress(width, height);
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
                    CloseSilenced();
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

    void Validate()
    {
        if (quickMenu == null) Debug.LogError("No quick menu found for QuickMenuController", this);

        if (quickMenuPanels == null || quickMenuPanels.Length == 0) Debug.LogError("No quick menu panels found for QuickMenuController", this);

        if (width == 0 || height == 0) Debug.LogError("0 width or height for QuickMenuController", this);
    }

    // state transition strategy: change our state first, then change global state
    // so we know who the request came from
    void Update()
    {
        switch (state)
        {
            case QuickPanelState.Open:
                if (Input.GetButtonDown("Quick Menu"))
                {
                    Close();
                    TheManager.Instance.Resume();
                    break;
                }

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
                break;
            case QuickPanelState.Closed:
                if (Input.GetButtonDown("Quick Menu"))
                {
                    Open();
                    TheManager.Instance.OpenInGameMenu();
                }
                break;
        }
    }

    public void OnGameStateChange(GameState oldState, GameState newState)
    {
        switch (newState)
        {
            case GameState.Paused:
            case GameState.Cutscene:
                CloseSilenced();
                break;
            case GameState.InGameMenu:
                // if it's not open, then we didn't request it.
                if (!isOpen) {
                    CloseSilenced();
                }
                break;
            case GameState.Playing:
                // BACK TO LISTENING
                Close();
                break;
        }
    }

    private void UpdatePosition()
    {
        if (!positionDirty) return;
        Debug.Log("position " + posn.Index);
        quickSelector.transform.localPosition = quickMenuPanels[posn.Index].transform.localPosition;
        positionDirty = false;
    }
}
