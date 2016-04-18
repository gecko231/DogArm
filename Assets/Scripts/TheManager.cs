using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum GameState
{
    Playing,
    Paused
}

public class TheManager : MonoBehaviour {

    public GameState gameState;
    public GameObject pauseMenu;
    public GameObject quickMenu;
    public GameObject quickSelector;
    public Image[] quickMenuPanels;
    private bool isOpen = false;

    // Use this for initialization
    void Start() {
        quickMenuPanels = GetComponents<Image>();
        Debug.Log(quickMenuPanels.Length);
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

    public void QuickOpen()
    {
        quickMenu.SetActive(true);

        for (var i = 0; i < quickMenuPanels.Length; i++)
        {
            Debug.Log("Panel Number " + i + " is named " + quickMenuPanels[i].transform.position.x);
        }
        quickSelector.SetActive(true);

        quickSelector.transform.position = new Vector2(quickMenuPanels[0].transform.position.x, quickMenuPanels[0].transform.position.y);
    }

    public void QuickClose()
    {
        quickMenu.SetActive(false);
        for (var i = 0; i < quickMenuPanels.Length; i++)
        {
        
        }
        quickSelector.SetActive(false);
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

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpen = !isOpen;
            if (isOpen == false)
            {
                QuickClose();
            } else
            {
                QuickOpen();
            }
        }

        // I realize this isn't optimal
        if (isOpen == true)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && (quickSelector.transform.position != quickMenuPanels[9].transform.position ||
                quickSelector.transform.position != quickMenuPanels[10].transform.position || quickSelector.transform.position != quickMenuPanels[11].transform.position))
            {
                quickSelector.transform.position = Vector2.right;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) && (quickSelector.transform.position != quickMenuPanels[0].transform.position ||
                quickSelector.transform.position != quickMenuPanels[1].transform.position || quickSelector.transform.position != quickMenuPanels[2].transform.position))
            {
                quickSelector.transform.position = Vector2.left;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) && (quickSelector.transform.position != quickMenuPanels[0].transform.position ||
                quickSelector.transform.position != quickMenuPanels[3].transform.position || 
                quickSelector.transform.position != quickMenuPanels[6].transform.position || quickSelector.transform.position != quickMenuPanels[9].transform.position))
            {
                quickSelector.transform.position = Vector2.up;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) && (quickSelector.transform.position != quickMenuPanels[2].transform.position ||
                quickSelector.transform.position != quickMenuPanels[5].transform.position ||
                quickSelector.transform.position != quickMenuPanels[8].transform.position || quickSelector.transform.position != quickMenuPanels[11].transform.position))
            {
                quickSelector.transform.position = Vector2.down;
            }
        }
    }
}
