using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Serialization;

public class TaskHighlighterBehavior : MonoBehaviour
{
    #region | Variables |
    
    // ## Targeted Game Objects
    [SerializeField] private ParticleSystem _completionConfetti;
    [SerializeField] private GameObject _arrows;
    [SerializeField] private GameObject _lightRay;
    [SerializeField] private GameObject _floorRing;
    
    private CapsuleCollider _capsuleCollider;
    private BoxCollider _boxCollider;
    
    // ## Enum Target Type
    private enum TaskTargetTypes{ Player, Item }
    [SerializeField] private TaskTargetTypes taskTargetType;
    private string curTaskTargetString;
    
    // ## Event
    public event Action<TaskHighlighterBehavior, float> OnEnter;
    
    [Header("Debugging")] 
    [SerializeField] private bool _enableDebugLogs;
    #endregion

    #region | Unity Methods |

    private void OnEnable()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _boxCollider = GetComponent<BoxCollider>();
        switch (taskTargetType)
        {
            case TaskTargetTypes.Player:
                curTaskTargetString = "Player";
                _capsuleCollider.enabled = true;
                _boxCollider.enabled = false;
                break;
            case TaskTargetTypes.Item:
                curTaskTargetString = "TaskItem";
                _capsuleCollider.enabled = false;
                _boxCollider.enabled = true;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_enableDebugLogs)
        {
            Debug.Log("OnTriggerEnter Triggered");
            Debug.Log("Collide Tag: " + other.tag);
            Debug.Log("Event: " + OnEnter);
        }
        
        if (other.CompareTag(curTaskTargetString)) OnEnter?.Invoke(this, _completionConfetti.main.duration);
    }

    #endregion

    #region | Custom Methods |

    public IEnumerator CompleteObjective()
    {
        if(_enableDebugLogs)Debug.Log("TaskHighlighter Task Complete");
        _floorRing.SetActive(false);
        _lightRay.SetActive(false);
        _arrows.SetActive(false);
        _capsuleCollider.enabled = false;
        _boxCollider.enabled = false;
        _completionConfetti.Play();
        
        yield return new WaitForSeconds(_completionConfetti.main.duration);
        _completionConfetti.gameObject.SetActive(false);
    }

    public void PrintInvocationList_Debugging()
    {
        Delegate[] invokeList = OnEnter?.GetInvocationList();
        foreach (var item in invokeList)
        {
            Debug.Log("Invocation Lists: " + item.Method);
        }
    }

    #endregion

    
}
