using UnityEngine;
using System.Collections.Generic;

public class QuickPanelController : MonoBehaviour {
    public GameObject quickMenu;
    public GameObject quickSelector;
    public GameObject[] quickMenuPanels;
    public int width;
    public int height;
    public GridAddress posn;
    private bool isOpen = false;

    // Use this for initialization
    void Start()
    {
        posn = new GridAddress(width, height);
        TheManager.Instance.stateChangeListeners.AddListener(this.OnGameStateChange);
        SetupRefs();
    }

    public void SetupRefs()
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

    void OnGameStateChange(GameState oldState, GameState newState)
    {
        if (newState == GameState.InGameMenu)
        {
            QuickOpen();
        }
        else if (oldState == GameState.InGameMenu)
        {
            QuickClose();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) posn.Right();
            if (Input.GetKeyDown(KeyCode.LeftArrow)) posn.Left();
            if (Input.GetKeyDown(KeyCode.DownArrow)) posn.Down();
            if (Input.GetKeyDown(KeyCode.UpArrow)) posn.Up();

            UpdatePosition();
        }
    }

    private void UpdatePosition()
    {
        Debug.Log("position " + posn.Index);
        quickSelector.transform.localPosition = quickMenuPanels[posn.Index].transform.localPosition;
    }

    public void QuickOpen()
    {
        isOpen = true;
        quickMenu.SetActive(true);

        UpdatePosition();
    }

    public void QuickClose()
    {
        isOpen = false;
        quickMenu.SetActive(false);
    }
}
