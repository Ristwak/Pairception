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

public class QAMatchGameManager : MonoBehaviour {
    public static QAMatchGameManager Instance { get; private set; }

    [Header("JSON Settings")] [SerializeField]
    private string jsonFileName = "updated_question_answer_500.json";

    [Header("UI References")]
    public TMP_Text[] questionLabels;
    public TMP_Text[] answerLabels;
    public RectTransform[] questionBoxes;
    public RectTransform[] answerBoxes;

    [Header("Controls")]
    public Button nextRoundButton;

    private List<QAPair> _allPairs;
    private Dictionary<string, string> _currentPairs;

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        nextRoundButton?.onClick.AddListener(SetupNextRound);
    }

    private void Start() { StartCoroutine(LoadQuestions()); }
    private IEnumerator LoadQuestions() {
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
        else { Debug.LogError("JSON not found: " + path); yield break; }
#endif
        try { _allPairs = JsonConvert.DeserializeObject<List<QAPair>>(json); }
        catch (Exception ex) { Debug.LogError("JSON parse error: " + ex.Message); yield break; }
        SetupNextRound();
    }

    public void SetupNextRound() {
        foreach (var qb in FindObjectsOfType<QuestionBox>()) {
            qb.answered = false;
            if (qb.currentLineRect != null) Destroy(qb.currentLineRect.gameObject);
            qb.currentLineRect = null;
        }

        var chosen = _allPairs.OrderBy(_ => UnityEngine.Random.value)
                              .Take(questionLabels.Length)
                              .ToList();
        _currentPairs = chosen.ToDictionary(p => p.question, p => p.answer);

        for (int i = 0; i < questionLabels.Length; i++) {
            questionLabels[i].text = chosen[i].question;
            questionBoxes[i].GetComponent<QuestionBox>().questionText = chosen[i].question;
        }
        var shuffled = chosen.Select(p => p.answer)
                             .OrderBy(_ => UnityEngine.Random.value)
                             .ToList();
        for (int i = 0; i < answerLabels.Length; i++) {
            answerLabels[i].text = shuffled[i];
            answerBoxes[i].GetComponent<AnswerBox>().answerText = shuffled[i];
        }
    }

    public bool EvaluateMatch(string q, string a) {
        return _currentPairs.TryGetValue(q, out var c) && c == a;
    }
}
