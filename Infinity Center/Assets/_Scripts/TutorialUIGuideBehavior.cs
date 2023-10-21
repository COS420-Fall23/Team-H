using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialUIGuideBehavior : MonoBehaviour
{
    #region | Core Variables |

    // Canvas Objects
    [SerializeField] private GameObject _taskUICanvas;
    [SerializeField] private GameObject _welcomeScreen;
    [SerializeField] private GameObject _curTaskScreen;
    [SerializeField] private TMP_Text _curTaskTopicTitle;
    [SerializeField] private TMP_Text _curTaskBodyText;
    [SerializeField] private GameObject _smoothOrTeleportPrompt;
    [SerializeField] private GameObject _userSettings; // This is a place holder. Need to find proper component to set movement type.

    // Integers to track task completion.
    private int _curTaskProgressIndex = 0;

    [Header("Debugging")] 
    [SerializeField] private bool _enableDebugLogs;

    #endregion

    #region | Unity Methods |
    

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
        ContinueTutorial();
    }

    public void ContinueTutorial()
    {
        if(_enableDebugLogs) Debug.Log("Continue Tutorial - Current Task Index: " + _curTaskProgressIndex);
        switch (_curTaskProgressIndex)
        {
            // ## Movement Tutorials
            case 0: // Movement Overview
                _curTaskTopicTitle.text = "Movement Overview";
                _curTaskBodyText.text =
                    "To start we will go over how to move. There are two categories of movement in the Infinity Center:\n" +
                    "\n - Synchronized Movement \n - Remote Movement";
                break;
            case 1: // 'Synced Movement' Summary & Task
                _curTaskTopicTitle.text = "Synchronized Movement";
                _curTaskBodyText.text = "Synchronized Movement is when you are set to be sharing the physical space as well as your virtual space with others.\n" +
                                        "\nFor this to occur, there will be no controller based movements. Your only move as you do with your actual body.\n" +
                                        "\nYOUR GOAL: Physically step into the highlighted area to demonstrate the you understand how Synchronized Movement.";
                break;
            case 2: // 'Synced Movement' Complete
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
                _curTaskTopicTitle.text = "Smooth Movement";
                _curTaskBodyText.text = "The first mode of movement we will explore is 'Smooth Movement'. This is a simulated way of ";
                break;
            case 5: // 'Smooth movement' Complete
                _curTaskTopicTitle.text = "Smooth Movement: Complete";
                _curTaskBodyText.text = "More text on smooth movement";
                break;
            case 6: // 'Teleport Movement' Task
                _curTaskTopicTitle.text = "Teleportation";
                _curTaskBodyText.text = "Introduce teleportation.";
                break;
            case 7: // 'Teleport Movement' Complete
                _curTaskTopicTitle.text = "Teleportation: Complete";
                _curTaskBodyText.text = "Yay, you did it.";
                _smoothOrTeleportPrompt.SetActive(true);
                break;
            
            // ## Interaction Tutorials
            case 8: // 'Interaction' Overview
                _curTaskTopicTitle.text = "";
                _curTaskBodyText.text = "";
                break;
            case 9: // 'Proximity Grab' Task
                _curTaskTopicTitle.text = "";
                _curTaskBodyText.text = "";
                break;
            case 10: // 'Proximity Grab' Complete
                _curTaskTopicTitle.text = "";
                _curTaskBodyText.text = "";
                break;
            case 11: // 'Ranged Grab' Task
                _curTaskTopicTitle.text = "";
                _curTaskBodyText.text = "";
                break;
            case 12: // 'Ranged Grab' Complete
                _curTaskTopicTitle.text = "";
                _curTaskBodyText.text = "";
                break;
            case 13: // 'Other Grab Points' Task
                _curTaskTopicTitle.text = "";
                _curTaskBodyText.text = "";
                // Should have a color mapped to "You can Grab This"
                // i.e. Bright Orange = "Grab Here" 
                break;
            case 14: // 'Other Grab Points' Task Complete
                _curTaskTopicTitle.text = "";
                _curTaskBodyText.text = "";
                break;
            
            // ## Inventory Tutorials
            // Will wait for previous tutorials to be done before assuming Task Index.
            
        }
        _curTaskProgressIndex++;
        if(_enableDebugLogs) Debug.Log("Task Index Incremented.");
    }

    #endregion
}
