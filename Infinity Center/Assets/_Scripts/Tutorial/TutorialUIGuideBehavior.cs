using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUIGuideBehavior : MonoBehaviour
{
    #region | Core Variables |

    // # Canvas Objects
    [SerializeField] private GameObject _welcomeScreen;
    [SerializeField] private GameObject _curTaskScreen;
    [SerializeField] private TMP_Text _curTaskTopicTitle;
    [SerializeField] private TMP_Text _curTaskBodyText;
    [SerializeField] private Button _btnContinue;
    [SerializeField] private GameObject _btnsMovementType;
    [SerializeField] private GameObject _userSettings; // This is a place holder. Need to find proper component to set movement type.

    // Integers to track task completion.
    private int _curTutorialProgressIndex = 0;
    private int _curTaskIndex = 0;
    
    // ## World Space Objects
    [SerializeField] private GameObject _menuPositionsObj;
    [SerializeField] private GameObject _taskHighlightersObj;
    

    [Header("Debugging")] 
    [SerializeField] private bool _enableDebugLogs;
    [SerializeField] private Transform[] _menuTransforms;
    [SerializeField] private TargetHighlighterBehavior[] _taskHighlighters;

    #endregion

    #region | Unity Methods |

    private void Awake()
    {
        _menuTransforms = _menuPositionsObj.GetComponentsInChildren<Transform>();
        _taskHighlighters = _taskHighlightersObj.GetComponentsInChildren<TargetHighlighterBehavior>();
        foreach (var highlighter in _taskHighlighters)
        {
            highlighter.gameObject.SetActive(false);
        }
        gameObject.transform.position = _menuTransforms[0].position;
        gameObject.SetActive(true);
        _welcomeScreen.SetActive(true);
        _curTaskScreen.SetActive(false);
    }

    #endregion

    #region | Custom Methods |
    
    public void StartSetupProcess()
    {
        if(_enableDebugLogs) Debug.Log("TO DO: This will begin the setup process.");
    }

    public void StartTutorial()
    {
        if(_enableDebugLogs) Debug.Log("Starting Tutorial");
        _welcomeScreen.SetActive(false);
        _curTaskScreen.SetActive(true);
        UpdateTutorial();
    }

    public void SelectMovement(int choiceIndex)
    {
        if (choiceIndex == 0)
        {
            if(_enableDebugLogs) Debug.Log("Smooth Locomotion Was Selected");
        }
        else if (choiceIndex == 1)
        {
            if(_enableDebugLogs) Debug.Log("Teleport movement was selected.");
        }
    }

    public void UpdateTutorial()
    {
        if(_enableDebugLogs) Debug.Log("Continue Tutorial - Current Task Index: " + _curTutorialProgressIndex);
        switch (_curTutorialProgressIndex)
        {
            // ## Movement Tutorials
            case 0: // Movement Overview
                _curTaskTopicTitle.text = "Movement Overview";
                _curTaskBodyText.text =
                    "To start we will go over how to move. There are two categories of movement in the Infinity Center:\n" +
                    "\n - Synchronized Movement \n - Remote Movement";
                break;
            case 1: // 'Synced Movement' Task
                _btnContinue.interactable = false;
                _taskHighlighters[0].Activate();
                _curTaskTopicTitle.text = "Synchronized Movement";
                _curTaskBodyText.text = "Synchronized Movement is when you are set to be sharing the physical space as well as your virtual space with others.\n" +
                                        "\nFor this to occur, there will be no controller based movements. Your only move as you do with your actual body.\n" +
                                        "\nYOUR GOAL: Physically step into the highlighted area to demonstrate the you understand how Synchronized Movement.";
                break;
            case 2: // 'Synced Movement' Complete
                _btnContinue.interactable = true;
                _taskHighlighters[0].Activate();
                gameObject.transform.position = _menuTransforms[1].position;
                _curTaskTopicTitle.text = "Synchronized Movement: Complete";
                _curTaskBodyText.text = "Great job. The idea here, is to have a classroom set up with this use where you share the physical and virtual world together in real time.\n" + 
                                        "\n Lets continue on to 'Remote Movement'";
                break;
            case 3: // 'Remote Movement' Overview
                _curTaskTopicTitle.text = "Remote Movement";
                _curTaskBodyText.text = "Remote movement is when you are joining a room over the internet from another location. This is the traditional mode of movement when using VR online.\n" + 
                                        "There are two primary modes of remote movement:\n" +
                                        " - Smooth Locomotion\n - Teleportation";
                break;
            case 4: // 'Smooth Move' Task
                _btnContinue.interactable = false;
                _taskHighlighters[0].gameObject.SetActive(true);
                _curTaskTopicTitle.text = "Smooth Movement";
                _curTaskBodyText.text = "The first mode of movement we will explore is 'Smooth Movement'. This is a simulated way of ";
                break;
            case 5: // 'Smooth movement' Complete
                _btnContinue.interactable = true;
                _curTaskTopicTitle.text = "Smooth Movement: Complete";
                _curTaskBodyText.text = "More text on smooth movement";
                break;
            case 6: // 'Teleport Movement' Task
                _btnContinue.interactable = false;
                _curTaskTopicTitle.text = "Teleportation";
                _curTaskBodyText.text = "Introduce teleportation.";
                break;
            case 7: // 'Teleport Movement' Complete
                _btnContinue.interactable = true;
                gameObject.transform.position = _menuTransforms[2].position;
                _curTaskTopicTitle.text = "Teleportation: Complete";
                _curTaskBodyText.text = "Yay, you did it. Please choose which way you prefer.";
                _btnContinue.gameObject.SetActive(false);
                _btnsMovementType.SetActive(true);
                break;
            
            // ## Interaction Tutorials
            case 8: // 'Interaction' Overview
                _curTaskTopicTitle.text = "Interaction";
                _curTaskBodyText.text = "Task";
                _btnContinue.gameObject.SetActive(true);
                _btnsMovementType.SetActive(false);
                break;
            case 9: // 'Proximity Grab' Task
                _btnContinue.interactable = false;
                _curTaskTopicTitle.text = "Proximity Grab";
                _curTaskBodyText.text = "Task";
                break;
            case 10: // 'Proximity Grab' Complete
                _btnContinue.interactable = true;
                _curTaskTopicTitle.text = "Proximity Grab: Complete";
                _curTaskBodyText.text = "Yay";
                break;
            case 11: // 'Ranged Grab' Task
                _btnContinue.interactable = false;
                _curTaskTopicTitle.text = "Ranged Grab";
                _curTaskBodyText.text = "Task";
                break;
            case 12: // 'Ranged Grab' Complete
                _btnContinue.interactable = true;
                _curTaskTopicTitle.text = "Ranged Grab: Complete";
                _curTaskBodyText.text = "Yay";
                break;
            case 13: // 'Other Grab Points' Task
                _btnContinue.interactable = false;
                _curTaskTopicTitle.text = "Other Grab Points";
                _curTaskBodyText.text = "Task";
                // Should have a color mapped to "You can Grab This"
                // i.e. Bright Orange = "Grab Here" 
                break;
            case 14: // 'Other Grab Points' Task Complete
                _btnContinue.interactable = true;
                _curTaskTopicTitle.text = "Other Grab Points: Complete";
                _curTaskBodyText.text = "Yay";
                break;
            
            // ## Inventory Tutorials
            // Will wait for previous tutorials to be done before assuming Task Index.
            
        }
        
        if(_enableDebugLogs) Debug.Log("Task Index Incremented.");
        _curTutorialProgressIndex++;
    }

    #endregion
}
