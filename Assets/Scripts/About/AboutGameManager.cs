using UnityEngine;
using TMPro;

/// <summary>
/// Populates the About panel with detailed game info for Pairception.
/// </summary>
public class AboutGameManager : MonoBehaviour
{
    [Header("UI Reference")]
    [Tooltip("Assign the TMP_Text component for the About section.")]
    public TMP_Text aboutText;

    void Start()
    {
        if (aboutText == null)
        {
            Debug.LogError("AboutGameManager: 'aboutText' reference is missing.");
            return;
        }

        aboutText.text =
            "<b>About Pairception</b>\n" +
            "Pairception is a fast-paced matching game designed to challenge and enhance your understanding of AI and Machine Learning concepts. Match key terms with their correct definitions — before the clock runs out!\n\n" +

            "<b>What’s the Game About?</b>\n" +
            "Each round presents a 4x4 match-the-columns grid. Your task is to pair each AI/ML term with its correct explanation.\n" +
            "From classic algorithms to model behaviors, Pairception is built to sharpen both recall and reasoning.\n\n" +

            "<b>Why Pairception?</b>\n" +
            "It’s more than memory — it's perception.\n" +
            "Train your ability to recognize and understand foundational ideas in AI and ML through quick, intuitive matching.\n\n" +

            "<b>Core Gameplay</b>\n" +
            "• Match terms like 'Overfitting', 'PCA', 'GANs', and more\n" +
            "• Get instant feedback with visual lines (correct = green, incorrect = red)\n" +
            "• Learn by playing — one concept at a time\n\n" +

            "<b>What You'll Learn</b>\n" +
            "• Key concepts in Machine Learning and AI\n" +
            "• Relationships between techniques, models, and outcomes\n" +
            "• Faster recognition of complex terminology\n\n" +

            "<b>Designed for Students</b>\n" +
            "Pairception is ideal for PG learners, especially those diving into:\n" +
            "• Machine Learning foundations\n" +
            "• Algorithm behavior and terminology\n" +
            "• Concept reinforcement through active play\n\n" +

            "<b>Crack the Code of Intelligence</b>\n" +
            "Boost your AI/ML IQ — one match at a time.\n" +
            "Master the terms. Connect the concepts.\n" +
            "Welcome to Pairception.";
    }
}
