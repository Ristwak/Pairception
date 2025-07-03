// QAMatchGameManager.cs
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
public class QAPair { public string question; public string answer; }

public class QAMatchGameManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject comingSoonPanel; // Assign in inspector

    public static QAMatchGameManager Instance { get; private set; }

    [Header("JSON Settings")]
    [SerializeField]
    private string jsonFileName = "unicode_converted_questions.json";

    [Header("UI References")]
    public TMP_Text[] questionLabels;
    public TMP_Text[] answerLabels;
    public RectTransform[] questionBoxes;
    public RectTransform[] answerBoxes;

    [Header("Controls")]
    public Button nextRoundButton;

    private List<QAPair> _allPairs;
    private Dictionary<string, string> _currentPairs;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        nextRoundButton.onClick.AddListener(EvaluateRound);
    }

    private void Start() { StartCoroutine(LoadQuestions()); }
    private IEnumerator LoadQuestions()
    {
        string path = Path.Combine(Application.streamingAssetsPath, jsonFileName);
        string json = string.Empty;

#if UNITY_ANDROID && !UNITY_EDITOR
    using var www = UnityWebRequest.Get(path);
    yield return www.SendWebRequest();
    if (www.result != UnityWebRequest.Result.Success) {
        Debug.LogError("Failed to load JSON: " + www.error);
        yield break;
    }
    json = www.downloadHandler.text;
#else
        if (File.Exists(path)) json = File.ReadAllText(path);
        else
        {
            Debug.LogError("JSON not found: " + path);
            yield break;
        }
#endif

        try
        {
            _allPairs = JsonConvert.DeserializeObject<List<QAPair>>(json);
        }
        catch (Exception ex)
        {
            Debug.LogError("JSON parse error: " + ex.Message);
            yield break;
        }

        // Check pair count
        if (_allPairs == null || _allPairs.Count < 4)
        {
            Debug.LogWarning("Not enough pairs in JSON. Showing Coming Soon panel.");
            comingSoonPanel.SetActive(true);
            yield break; // Don't start game
        }

        SetupNextRound();
    }

    public void SetupNextRound()
    {
        if (_allPairs == null || _allPairs.Count < 4)
        {
            Debug.LogWarning("Not enough pairs to continue. Showing Coming Soon panel.");
            comingSoonPanel.SetActive(true);
            return;
        }

        foreach (var qb in FindObjectsOfType<QuestionBox>())
        {
            qb.answered = false;
            if (qb.currentLineRect != null) Destroy(qb.currentLineRect.gameObject);
            qb.currentLineRect = null;
        }

        var chosen = _allPairs.OrderBy(_ => UnityEngine.Random.value)
                              .Take(questionLabels.Length)
                              .ToList();

        // Remove used pairs to prevent repetition
        foreach (var pair in chosen)
            _allPairs.Remove(pair);

        _currentPairs = chosen.ToDictionary(p => p.question, p => p.answer);

        for (int i = 0; i < questionLabels.Length; i++)
        {
            questionLabels[i].text = chosen[i].question;
            questionBoxes[i].GetComponent<QuestionBox>().questionText = chosen[i].question;
        }

        var shuffled = chosen.Select(p => p.answer)
                             .OrderBy(_ => UnityEngine.Random.value)
                             .ToList();

        for (int i = 0; i < answerLabels.Length; i++)
        {
            answerLabels[i].text = shuffled[i];
            answerBoxes[i].GetComponent<AnswerBox>().answerText = shuffled[i];
        }
    }

    public void EvaluateRound()
    {
        StartCoroutine(EvaluateAndWaitCoroutine());
    }

    private IEnumerator EvaluateAndWaitCoroutine()
    {
        nextRoundButton.interactable = false;

        foreach (var qb in questionBoxes.Select(q => q.GetComponent<QuestionBox>()))
        {
            if (qb.currentLineRect == null) continue;

            var img = qb.currentLineRect.GetComponent<Image>();
            bool isCorrect = _currentPairs.TryGetValue(qb.questionText, out string correctAns)
                             && correctAns == qb.matchedAnswerText;

            img.color = isCorrect ? Color.green : Color.red;
        }

        yield return new WaitForSeconds(3f);

        if (_allPairs.Count < 4)
        {
            comingSoonPanel.SetActive(true);
        }
        else
        {
            SetupNextRound();
        }

        nextRoundButton.interactable = true;
    }
}
