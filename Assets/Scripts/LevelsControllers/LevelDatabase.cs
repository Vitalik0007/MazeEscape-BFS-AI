using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "Game/LevelDatabase")]
public class LevelDatabase : ScriptableObject
{
    public List<LevelConfig> levels;

    public LevelConfig GetLevel(int levelIndex)
    {
        levelIndex = Mathf.Clamp(levelIndex, 0, levels.Count - 1);
        return levels[levelIndex];
    }
}