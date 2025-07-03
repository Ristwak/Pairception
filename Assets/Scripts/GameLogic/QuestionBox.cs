// QuestionBox.cs
using UnityEngine;

public class QuestionBox : MonoBehaviour {
    [HideInInspector] public string questionText;
    [HideInInspector] public RectTransform currentLineRect;
    [HideInInspector] public bool answered;
}