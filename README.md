# 🧠 Maze Escape AI – Best First Search in Action

**Maze Escape AI** is a 2D grid-based game inspired by Pac-Man. The project showcases how a non-player character (NPC) can chase the player using the **Best First Search** pathfinding algorithm.

> 🎓 Created as a university project for the course **"Artificial Intelligence in Video Games"**.

---

## 🕹️ Gameplay

- The player controls a character navigating a procedurally generated maze.
- An AI-controlled enemy chases the player using **Best First Search**.
- Each level introduces different tile types:
  - 🟫 **Walls** – impassable tiles
  - 🟩 **Floor** – regular walkable tiles
  - 🟫 **Swamp** – slows the player or NPC (starting from level 2)
  - 🟦 **Speed Boost** – increases movement speed (added in level 3)

---

## 🧠 Algorithm: Best First Search

- The enemy uses a **greedy Best First Search** algorithm to find the shortest path.
- It prioritizes tiles based on **Manhattan distance** (`|x₁ - x₂| + |y₁ - y₂|`) to the target.
- Unlike A*, BFS does not account for the cost already traveled — it focuses only on proximity to the goal.
- The path is reconstructed by backtracking from the goal using a dictionary of visited nodes.

---

## 🛠️ Implementation

Built with **Unity (C#)** using clean OOP architecture:

- `MazeGenerator.cs` – generates the maze grid and assigns tile types
- `BestFirstSearchPathfinder.cs` – core pathfinding logic
- `GridManager.cs` – handles maze visualization and entity placement
- `EnemyAI.cs` – NPC logic using the BFS algorithm
- `PlayerMovement.cs` – controls player movement across tiles

---

## 📦 Getting Started

1. Clone the repository.
2. Open the project in **Unity (2023.2.7f1 or newer)**.
3. Launch the scene called `SampleScene` and press Play.

---

## ⚖️ License

This project is licensed under the [MIT License](LICENSE) — feel free to use, study, and expand it for personal or educational purposes.

---

## 🙋‍♂️ Author

Developed with ❤️ as a showcase of AI algorithms in games.

https://github.com/Vitalik0007

---

## ✨ Future Ideas

- Add **A\*** pathfinding to compare with Best First Search
- Implement multiple enemies with different AI behaviors
- UI for win/lose states
- Maze themes or visual variations per level

---
