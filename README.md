# 🧠 Pairception

**Pairception** is a fast-paced, interactive matching game built in Unity where players must logically pair questions and answers across key domains like AI, machine learning, and logic. Inspired by cognitive training and educational tools, it challenges players to think critically under pressure.

![Pairception Banner](./banner.png) <!-- Optional: Replace with your actual image path -->

---

## 🎮 Game Description

In each round, players are shown a set of questions and a shuffled set of possible answers. The goal is to drag and match each question to its correct answer.

After matching all pairs, players submit their selections. The game then visually indicates which matches were correct or incorrect and loads the next round.

---

## 🧩 Features

- 🎯 **Drag-to-match** UI with visual feedback using LineRenderer.
- 📦 Loads data dynamically from JSON files.
- ✅ Submit-and-reveal system: feedback only appears after all matches are placed.
- 🧠 Built for reasoning, matching, and fast association.
- 📉 Tracks round-by-round accuracy (scoring system optional).
- 🛑 Shows “Coming Soon” panel when levels run out.

---

## 🛠️ Tech Stack

- **Engine:** Unity 2022+  
- **Language:** C#  
- **UI:** Unity UI (TextMeshPro, Panels, Buttons)  
- **Visuals:** LineRenderer for dynamic linking  
- **Data Format:** JSON (StreamingAssets)  

---

## 📂 Folder Structure

Assets/
├── Scripts/
│ ├── QAMatchGameManager.cs
│ ├── QuestionBox.cs
│ ├── AnswerBox.cs
│ ├── DragLineAnchored.cs
│ └── AboutGameManager.cs
├── StreamingAssets/
│ └── unicode_converted_questions.json
├── Prefabs/
│ └── LineRendererPrefab.prefab
├── UI/
│ └── Panels, Buttons, Text


---

## 🚀 How to Play

1. **Start the game.**
2. **Drag** each question to the answer you think is correct.
3. Once all are matched, click **Submit**.
4. **Green line = correct**, **Red line = incorrect**.
5. After a short delay, the next round loads.
6. When the questions run out, a **Coming Soon** screen appears.

---

## 🧪 Developer Setup

To run or modify the game:

1. Clone the repo:  
   ```bash
   git clone https://github.com/your-username/pairception.git
