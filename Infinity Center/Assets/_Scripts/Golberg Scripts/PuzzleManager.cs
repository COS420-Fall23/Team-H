using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private PuzzleStatus puzzleStatus;
    [SerializeField] private GameObject[] levelPrefabs; // Array of level prefabs

    public int currentLevelIndex = 0;
    private GameObject currentLevelInstance;

    private void Start()
    {
        LoadLevel(currentLevelIndex);
    }

    // Call this to load a specific level
    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levelPrefabs.Length)
        {
            Debug.LogError("Level index out of range");
            return;
        }

        // Destroy current level if it exists
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
        }

        // Instantiate the new level
        currentLevelInstance = Instantiate(levelPrefabs[levelIndex]);
        currentLevelIndex = levelIndex;

        // Reset puzzle status for the new level
        puzzleStatus.ResetPuzzleObjects();
        puzzleStatus.SavePuzzleState();
    }

    // Call this to load the next level
    public void LoadNextLevel()
    {
        int nextLevelIndex = (currentLevelIndex + 1) % levelPrefabs.Length;
        LoadLevel(nextLevelIndex);
    }

    // Call this to retry the current level
    public void RetryCurrentLevel()
    {
        LoadLevel(currentLevelIndex);
    }
}
