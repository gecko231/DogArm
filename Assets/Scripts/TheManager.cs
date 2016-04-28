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
    public int panelNum = 0;
    private bool isOpen = false;

    // Use this for initialization
    void Start() {
        //Debug.Log(quickMenuPanels.Length);
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

        quickSelector.SetActive(true);

        quickSelector.transform.position = new Vector2(quickMenuPanels[panelNum].transform.position.x, quickMenuPanels[panelNum].transform.position.y);
    }

    public void QuickClose()
    {
        quickMenu.SetActive(false);

        quickSelector.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isOpen == false) {
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
                isOpen = true;
                QuickOpen();
            }
        } else {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                panelNum += 3;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                panelNum -= 3;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (panelNum == 2 || panelNum == 5 || panelNum == 8 || panelNum == 11)
                {
                    panelNum -= 2;
                }
                else
                {
                    panelNum += 1;
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (panelNum == 0 || panelNum == 3 || panelNum == 6 || panelNum == 9)
                {
                    panelNum += 2;
                }
                else
                {
                    panelNum -= 1;
                }
            }

            if (panelNum > 11)
            {
                panelNum -= 12;
            }

            if (panelNum < 0)
            {
                panelNum += 12;
            }

            //Debug.Log("Panel: " + panelNum);

            quickSelector.transform.position = new Vector2(quickMenuPanels[panelNum].transform.position.x, quickMenuPanels[panelNum].transform.position.y);

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                isOpen = false;
                QuickClose();
            }
        }
    }
}
