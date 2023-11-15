using System;
using System.Collections;
using System.Collections.Generic;
using HurricaneVR.Framework.Core.Player;
using UnityEngine;
using UnityEngine.Events;

public class TutorialEventHandler : MonoBehaviour
{
    #region | Psudocode |
    /*
     * I want to limit the scope of the classes.
     * I want to have the UI only worry about manipulating itself.
     * I want the highlighters to only manipulate itself.
     *
     * This event handler is the tool to bridge the communication gap between the two.
     *
     * The highlighter has two major behaviors:
     *  - Activate when needed.
     *  - Trigger confetti and deactivate when completed.
     *  - This trigger is connected to the UI to proceed.
     *
     * The UI itterates through the progress.
     *  - Will show next txt.
     *  - 'Activates highlighter' when prompting user.
     *      - Deactivates "continue" button when this occurs.
     *  - Moves to next location when completed and continues.
     *
     * Thus Needed Events:
     *  - UI -> Highlight : Activate
     *  - Highlight -> UI : Completed
     *  - Tutorial Complete when final task is accomplished.
     *
     * How will data be passed?
     *  - Highlight -> UI : Complete
     *      - Highlight invokes event OnTriggerEnter(), referencing itself.
     *      - OnInvoke, this tells UI to continue.
     *
     *  - UI -> Highlight : Activate
     *      - On correct button press, Invoke() activate.
     *      - OnInvoke, this track the next task, and activates it.
     *
     *
     */
    #endregion
    
    #region | Variables |

    [SerializeField] private TutorialUIGuideBehavior _tutorialUI;
    [SerializeField] private TaskHighlighterBehavior[] _highlighters;
    
    [Header("Player Inputs")] [SerializeField]
    private HVRPlayerController _playerController;
    [SerializeField] private float _defaultMoveSpeed = 1.5f;
    [SerializeField] private float _defaultRunSpeed = 3.5f;
    
    
    [Header("Debugging")] 
    [SerializeField] private bool _enableDebugLogs;
    [SerializeField] private int _curHighlightIndex;

    #endregion

    #region | Unity Methods |

    private void OnEnable()
    {
        foreach (var highlight in _highlighters)
        {
            highlight.gameObject.SetActive(false);
            highlight.OnEnter += OnEnterMarker;
            highlight.PrintInvocationList_Debugging();
        }
        _tutorialUI.activateNextTask += OnActivateNextTask;
    }

    private void Start()
    {
        
    }

    #endregion

    #region | Custom Methods |

    private void OnPauseMovement()
    {
        if(_enableDebugLogs) Debug.Log("OnPauseMovement Invoked");
        _playerController.MoveSpeed = 0;
        _playerController.RunSpeed = 0;
        _playerController.CanJump = false;
        _playerController.CanCrouch = false;
        
    }

    private void OnEnterMarker(TaskHighlighterBehavior taskHighlighterBehavior, float particalDuration)
    {
        if(_enableDebugLogs) Debug.Log("OnEnterMarker Invoked");
        
        StartCoroutine(taskHighlighterBehavior.CompleteObjective());
        _tutorialUI.UpdateTutorial();
    }

    private void OnActivateNextTask()
    {
        if(_enableDebugLogs) Debug.Log("OnActivateNextTask Triggered");
        _highlighters[_curHighlightIndex].gameObject.SetActive(true);
        _curHighlightIndex++;
    }

    #endregion


}