using UnityEngine;
using UnityEngine.Events;

public class PuzzleCompletion : MonoBehaviour
{
    public UnityEvent onPuzzleComplete; // Event that can be set in the inspector
    private PuzzleManager puzzleManager;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the tag "TaskItem"
        if (other.CompareTag("TaskItem"))
        {
            Debug.Log("Puzzle Completed!");
            puzzleManager.LoadNextLevel();
        }
    }
}
