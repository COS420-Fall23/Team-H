using System;
using UnityEngine;

[Serializable]
public class puzzleStatus : MonoBehaviour
{
    [SerializeField] public GameObject pieces;
    public List <GameObject> objectList;
    public List <Transform> transformBase;

    [SerializeField] public List<GameObject> startingState;

    private void Awake(List targetList)
    {
        SavePuzzleState(targetList);
    }

    public void SavePuzzleState(List <GameObject> origin, List <Transform> target)
    {

        foreach(GameObject currentObj in origin)
        {
            target.add(currentObj.transform);
        }
    }

    public void LoadPuzzleState(objectList)
    {
        for (int i = 0; int i < range(objectList); i++)
        {
            Instantiate(objectList[int i]);
        }
        Debug.Log("Puzzle state loaded.");
        objectList.Clear();
        Debug.Log("List cleared.");
    }
}
