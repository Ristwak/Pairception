using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
public class QAPair
{
    public string question;
    public string answer;
}

public class QAMatchGameManager : MonoBehaviour
{
    [Header("JSON Settings")]
    [SerializeField] private string jsonFileName = "updated_question_answer_500.json";

    [Header("UI References")]
    public TMP_Text[] questionLabels;      // UI Texts for 4 questions
    public TMP_Text[] answerLabels;        // UI Texts for 4 answers
    public RectTransform[] questionBoxes; // RectTransforms for visual line start
    public RectTransform[] answerBoxes;   // RectTransforms for visual line end

    [Header("Line Drawing")]
    public GameObject linePrefab;       // Assign a prefab with LineRenderer
    public Transform lineContainer;     // Optional parent for keeping scene clean

    [Header("Controls")]
    public Button nextRoundButton;

    private List<QAPair> _allPairs;
    private Dictionary<string, string> _currentPairs;
    private List<LineRenderer> _activeLines = new();

    private int? selectedQuestionIndex = null;

    private void Awake()
    {
        if (nextRoundButton != null)
            nextRoundButton.onClick.AddListener(SetupNextRound);
    }

    private void Start()
    {
        StartCoroutine(LoadQuestionsFromStreamingAssets());
    }

    private IEnumerator LoadQuestionsFromStreamingAssets()
    {
        string path = Path.Combine(Application.streamingAssetsPath, jsonFileName);
        string json = "";

#if UNITY_ANDROID && !UNITY_EDITOR
        using (UnityWebRequest www = UnityWebRequest.Get(path))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to load JSON on Android: {www.error}");
                yield break;
            }
            json = www.downloadHandler.text;
        }
#else
        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
        }
        else
        {
            Debug.LogError($"JSON file not found at: {path}");
            yield break;
        }
#endif

        try
        {
            _allPairs = JsonConvert.DeserializeObject<List<QAPair>>(json);
        }
        catch (Exception ex)
        {
            Debug.LogError($"JSON parse error: {ex.Message}");
            yield break;
        }

        if (_allPairs == null || _allPairs.Count < 4)
        {
            Debug.LogError("Loaded JSON is empty or has fewer than 4 pairs.");
            yield break;
        }

        SetupNextRound();
    }

    public void SetupNextRound()
    {
        // Clear lines from previous round
        foreach (var line in _activeLines)
            Destroy(line.gameObject);
        _activeLines.Clear();

        // Pick 4 random pairs
        var chosen = _allPairs.OrderBy(_ => UnityEngine.Random.value).Take(4).ToList();
        _currentPairs = chosen.ToDictionary(p => p.question, p => p.answer);

        // Set question texts
        for (int i = 0; i < questionLabels.Length; i++)
            questionLabels[i].text = i < chosen.Count ? chosen[i].question : "";

        // Set answer texts (shuffled)
        var shuffledAnswers = chosen.Select(p => p.answer).OrderBy(_ => UnityEngine.Random.value).ToList();
        for (int i = 0; i < answerLabels.Length; i++)
            answerLabels[i].text = i < shuffledAnswers.Count ? shuffledAnswers[i] : "";

        selectedQuestionIndex = null;
    }

    // Called when a question is tapped
    public void OnQuestionSelected(int index)
    {
        selectedQuestionIndex = index;
    }

    // Called when an answer is tapped
    public void OnAnswerSelected(int answerIndex)
    {
        if (selectedQuestionIndex == null)
        {
            Debug.Log("Select a question first.");
            return;
        }

        int qIndex = selectedQuestionIndex.Value;
        string selectedQuestion = questionLabels[qIndex].text;
        string selectedAnswer = answerLabels[answerIndex].text;

        bool isCorrect = IsCorrectMatch(selectedQuestion, selectedAnswer);

        // Draw line
        DrawUILine(questionBoxes[qIndex], answerBoxes[answerIndex], isCorrect ? Color.green : Color.red);

        selectedQuestionIndex = null;
    }

    private bool IsCorrectMatch(string question, string answer)
    {
        return _currentPairs != null &&
               _currentPairs.TryGetValue(question, out var correct) &&
               correct == answer;
    }

    private void DrawUILine(RectTransform from, RectTransform to, Color color)
    {
        GameObject lineObj = Instantiate(linePrefab, lineContainer);
        LineRenderer lr = lineObj.GetComponent<LineRenderer>();

        Vector3 start = from.position;
        Vector3 end = to.position;

        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        lr.startColor = lr.endColor = color;
        _activeLines.Add(lr);
    }
}
