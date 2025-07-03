using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gamePanel;
    public GameObject comingSoonPanel;
    public GameObject exitPanel;

    private bool isComingSoonActive = false;

    void Start()
    {
        gamePanel.SetActive(true);
        exitPanel.SetActive(false);
        comingSoonPanel.SetActive(false);
    }

    void Update()
    {
        if (comingSoonPanel.activeSelf)
            isComingSoonActive = true;
        if (Input.GetKeyDown(KeyCode.Escape)) // Android back button
            {
                if (!exitPanel.activeSelf)
                {
                    exitPanel.SetActive(true);
                    comingSoonPanel.SetActive(false);
                }
            }

        if (exitPanel.activeSelf)
        {
            Time.timeScale = 0f;
        }
    }

    public void OnExitYes()
    {
        Application.Quit();
        Debug.Log("Game closed.");
    }

    public void OnExitNo()
    {
        exitPanel.SetActive(false);
        if(isComingSoonActive)
            comingSoonPanel.SetActive(true);
        else
            comingSoonPanel.SetActive(false);
        Time.timeScale = 1f; // Resume game if exit panel is closed
    }
}
