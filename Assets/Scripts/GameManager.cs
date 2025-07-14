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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!exitPanel.activeSelf)
            {
                exitPanel.SetActive(true);
                gamePanel.SetActive(false);
                comingSoonPanel.SetActive(false);

                OnExitPanelShown(); // clear lines
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
        if (isComingSoonActive)
            SceneManager.LoadScene("MenuScene");
        else
        {
            comingSoonPanel.SetActive(false);
            gamePanel.SetActive(true);
        }
        Time.timeScale = 1f;
    }

    // Clears all drawn lines
    private void OnExitPanelShown()
    {
        GameObject[] lines = GameObject.FindGameObjectsWithTag("LineUI");
        foreach (GameObject line in lines)
        {
            Destroy(line);
        }
    }
}
