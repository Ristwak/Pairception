// QuestionBox.cs
using UnityEngine;

public class QuestionBox : MonoBehaviour
{
    [HideInInspector] public string questionText;
    [HideInInspector] public RectTransform currentLineRect;
    [HideInInspector] public string matchedAnswerText;  // NEW
    [HideInInspector] public bool answered;
    [SerializeField] public RectTransform circleAnchor;
}
