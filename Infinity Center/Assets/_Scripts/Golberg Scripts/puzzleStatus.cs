using System.Collections.Generic;
using UnityEngine;

public class PuzzleStatus : MonoBehaviour
{
    private List<GameObject> puzzleObjects = new List<GameObject>();
    private Dictionary<GameObject, TransformData> savedTransforms = new Dictionary<GameObject, TransformData>();

    // Call this method to add a new object to the puzzle
    public void AddPuzzleObject(GameObject obj)
    {
        if (!puzzleObjects.Contains(obj))
        {
            puzzleObjects.Add(obj);
        }
    }

    // Call this method to remove an object from the puzzle
    public void RemovePuzzleObject(GameObject obj)
    {
        if (puzzleObjects.Contains(obj))
        {
            puzzleObjects.Remove(obj);
        }
    }

    // Call this method to save the current state of the puzzle
    public void SavePuzzleState()
    {
        savedTransforms.Clear();
        foreach (GameObject obj in puzzleObjects)
        {
            if (obj != null)
            {
                savedTransforms[obj] = new TransformData(obj.transform.position, obj.transform.rotation);
            }
        }
    }

    // Call this method to restore the puzzle to the saved state
    public void LoadPuzzleState()
    {
        foreach (var pair in savedTransforms)
        {
            if (pair.Key != null)
            {
                pair.Key.transform.position = pair.Value.Position;
                pair.Key.transform.rotation = pair.Value.Rotation;
            }
        }
    }

    // New method to reset puzzle objects when switching levels
    public void ResetPuzzleObjects()
    {
        puzzleObjects.Clear();
        savedTransforms.Clear();
    }

    private struct TransformData
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public TransformData(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}
