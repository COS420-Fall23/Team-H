// using System;
// using UnityEngine;

// [Serializable]
// public class puzzleStatus : MonoBehaviour
// {
//     [SerializeField] public GameObject pieces;
//     public List <GameObject> objectList;
//     public List <Transform> transformBase;

//     [SerializeField] public List<GameObject> startingState;

//     private void Awake(List targetList)
//     {
//         SavePuzzleState(targetList);
//     }

//     public void SavePuzzleState(List <GameObject> origin, List <Transform> target)
//     {

//         foreach(GameObject currentObj in origin)
//         {
//             target.add(currentObj.transform);
//         }
//     }

//     public void LoadPuzzleState(objectList)
//     {
//         for (int i = 0; int i < range(objectList); i++)
//         {
//             Instantiate(objectList[int i]);
//         }
//         Debug.Log("Puzzle state loaded.");
//         objectList.Clear();
//         Debug.Log("List cleared.");
//     }
// }

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

