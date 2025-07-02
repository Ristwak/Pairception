using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject aboutPanel;
    public GameObject loadingPanel;

    void Start()
    {
        menuPanel.SetActive(true);
        loadingPanel.SetActive(false);
        aboutPanel.SetActive(false);
    }

    public void OnStartGame()
    {
        // Load first level or scene
        Debug.Log("Starting Game");
        SceneManager.LoadScene("GameScene");
        menuPanel.SetActive(false);
        loadingPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void OnBack()
    {
        // Return to main menu
        aboutPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void OnAbout()
    {
        // Open AI history info
        menuPanel.SetActive(false);
        aboutPanel.SetActive(true);
        Debug.Log("About AI opened.");
    }

    public void OnExit()
    {
        // Open AI history info
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
