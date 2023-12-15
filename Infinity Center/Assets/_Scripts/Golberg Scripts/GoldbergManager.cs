using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoldbergManager : MonoBehaviour
{
    public static GoldbergManager instance { get; private set; }
    
    #region | Variables |

    private enum PuzzleStates
    {
        NotReady,
        Ready,
        Active
    }

    [SerializeField] private GoldbergLevel[] _levelPrefabs;
    [SerializeField] private GameObject _welcomeScreen;
    [SerializeField] private GameObject _congratsScreen;
    

    private event Action startLevel;
    private event Action resetLevel;
    private event Action allLevelsCompleted; 

    [Header("Debugging")]
    [SerializeField] private int _curLevelIndex = 0;
    [SerializeField] private List<CreatableItemBehavior> _createdItems;
    [SerializeField] private PuzzleStates _curPuzzleState;
    #endregion

    #region | Custom Methods |

    // Scene Starting State: All Levels Off.
    private void Awake()
    {
        // Singleton Structure
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        
        // Puzzle Setting
        _curPuzzleState = PuzzleStates.NotReady;
        foreach (var level in _levelPrefabs)
        {
            level.gameObject.SetActive(false);
        }
        ReadyCurrentLevel();
    }

    #endregion

    #region | Custom Methods |

    // State machine for Button Press based on Puzzle's State
    public void OnPuzzleButtonPress()
    {
        switch (_curPuzzleState)
        {
            case PuzzleStates.NotReady:
                ReadyCurrentLevel();
                break;
            case PuzzleStates.Ready:
                StartLevel();
                break;
            case PuzzleStates.Active:
                ResetLevel();
                break;
        }
    }

    // Enables Level & Subscribes Methods to Events
    private void ReadyCurrentLevel()
    {
        _levelPrefabs[_curLevelIndex].gameObject.SetActive(true);
        startLevel += _levelPrefabs[_curLevelIndex].StartLevel;
        resetLevel += _levelPrefabs[_curLevelIndex].ResetLevel;
        _levelPrefabs[_curLevelIndex].taskHighlighter.OnEnter += OnCompleteLevel;
        _curPuzzleState = PuzzleStates.Ready;
    }

    private void StartLevel()
    {
        _curPuzzleState = PuzzleStates.Active;
        startLevel?.Invoke();
    }

    public void ResetLevel()
    {
        _curPuzzleState = PuzzleStates.Ready;
        resetLevel?.Invoke();
    }

    private void OnCompleteLevel(TaskHighlighterBehavior taskHighlighterBehavior, float particalDuration)
    {
        StartCoroutine(taskHighlighterBehavior.CompleteObjective());
        
        // Remove & Destroy Created Items
        foreach (var item in _createdItems)
        {
            Destroy(item.gameObject);
        }
        _createdItems.Clear();
        
        // Unsubscribe from Events 
        startLevel -= _levelPrefabs[_curLevelIndex].StartLevel;
        resetLevel -= _levelPrefabs[_curLevelIndex].ResetLevel;
        _levelPrefabs[_curLevelIndex].taskHighlighter.OnEnter -= OnCompleteLevel;
        
        // Conclude Level
        _levelPrefabs[_curLevelIndex].CompleteLevel();
        _levelPrefabs[_curLevelIndex].gameObject.SetActive(false);
        
        // Iterate To Next
        _curLevelIndex++;
        if (_curLevelIndex < _levelPrefabs.Length)
        {
            ReadyCurrentLevel();
        }
        else
        {
            AllLevelsCompleted();
        }
    }

    private void AllLevelsCompleted()
    {
        _welcomeScreen.SetActive(false);
        _congratsScreen.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent(out CreatableItemBehavior newObj))
        {
            _createdItems.Add(newObj);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out CreatableItemBehavior newObj))
        {
            _createdItems.Remove(newObj);
        }
    }

    #endregion
}
