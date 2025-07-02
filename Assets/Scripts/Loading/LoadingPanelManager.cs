using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingPanelManager : MonoBehaviour
{
    public GameObject loadingPanel;
    public TextMeshProUGUI loadingText;
    public TextMeshProUGUI loadingDots;

    private bool isLoading = false;

    void Start()
    {
        loadingPanel.SetActive(false);
    }

    public void ShowLoadingPanel()
    {
        loadingPanel.SetActive(true);
        isLoading = true;
        StartCoroutine(AnimateDots());
    }

    public void HideLoadingPanel()
    {
        isLoading = false;
        loadingPanel.SetActive(false);
    }

    IEnumerator AnimateDots()
    {
        string[] dotStates = { "", ".", "..", "..." };
        int index = 0;

        while (isLoading)
        {
            loadingDots.text = dotStates[index];
            index = (index + 1) % dotStates.Length;
            yield return new WaitForSeconds(0.4f);
        }

        loadingDots.text = "";
    }

    // Example: Loading a scene with this panel
    public void LoadSceneWithLoading(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    IEnumerator LoadSceneRoutine(string sceneName)
    {
        ShowLoadingPanel();
        yield return new WaitForSeconds(1f); // Optional: fake wait

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        HideLoadingPanel();
    }
}
