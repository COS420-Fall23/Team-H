using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialUIGuideBehavior : MonoBehaviour
{
    #region | Core Variables |

    // ## Canvas Objects
    [SerializeField] private GameObject _welcomeScreen;
    [SerializeField] private GameObject _curTaskScreen;
    [SerializeField] private TMP_Text _curTaskTopicTitle;
    [SerializeField] private TMP_Text _curTaskBodyText;
    [SerializeField] private Button _btnContinue;

    // ## Integers to track task completion.
    private int _curTutorialProgressIndex = 0;
    
    // ## Events
    public event Action activateNextTask;
    public event Action pausePlayerMovement;

    [Header("Debugging")] 
    [SerializeField] private bool _enableDebugLogs;
    [SerializeField] private Transform[] _menuTransforms;
    

    #endregion

    #region | Unity Methods |

    private void Awake()
    {
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
                    "To start we will go over how to move. There are two categories of movement:\n" +
                    "\n - Synchronized Movement \n - Remote Movement";
                break;
            case 1: // 'Synced Movement' Task
                _btnContinue.interactable = false;
                if(_enableDebugLogs) Debug.Log("Invoking ActivateNextTask");
                activateNextTask?.Invoke();
                _curTaskTopicTitle.text = "Synchronized Movement";
                _curTaskBodyText.text = "Synchronized Movement is when you are setup to share a physical space as well as your virtual space with others.\n" +
                                        "\nFor this to occur, there will be no controller-based movements. Your movement is done with your actual body.\n" +
                                        "\n[YOUR GOAL]: Physically step into the highlighted area.";
                break;
            case 2: // 'Synced Movement' Complete
                _btnContinue.interactable = true;
                gameObject.transform.position = _menuTransforms[1].position;
                _curTaskTopicTitle.text = "Synchronized Movement: Complete";
                _curTaskBodyText.text = "Great job. The idea here, is to have a classroom set up to share the physical and virtual world together in real time.\n" + 
                                        "\n Lets continue on to 'Remote Movement'";
                break;
            case 3: // 'Remote Movement' Overview
                _curTaskTopicTitle.text = "Remote Movement";
                _curTaskBodyText.text = "Remote movement is when you are joining a room over the internet from another location. This is the traditional mode of movement when using VR online.\n" + 
                                        "There are two primary modes of remote movement:\n" +
                                        "\n- Smooth Locomotion\n - Teleportation";
                break;
            case 4: // 'Smooth Move' Task
                _btnContinue.interactable = false;
                activateNextTask?.Invoke();
                _curTaskTopicTitle.text = "Smooth Movement";
                _curTaskBodyText.text = "The first mode of movement we will explore is 'Smooth Movement'. This is more immersive and simulates normal walking.\n" +
                                        "\nHowever it may feel disorienting at first, so take it slow.\n" +
                                        "On your left hand, tilt the analog stick to move forward back, and side to side.\n" +
                                        "Then, on your right hand, tilt the stick side to side to rotate.\n" +
                                        "\n[YOUR GOAL]: Practice these controls by stepping into the new highlighted area.";
                break;
            case 5: // 'Smooth movement' Complete
                _btnContinue.interactable = true;
                _curTaskTopicTitle.text = "Smooth Movement: Complete";
                _curTaskBodyText.text = "If the smooth rotation is uncomfortable, it will be possible to change it to a 'snap' rotation.\n" +
                                        "\nLets continue to Teleportation movement.";
                break;
            case 6: // 'Teleport Movement' Task
                _btnContinue.interactable = false;
                activateNextTask?.Invoke();
                _curTaskTopicTitle.text = "Teleportation";
                _curTaskBodyText.text = "Teleporting allows you to snap to a location without the discomfort of gradual motion.\n" +
                                        "To Teleport, on your right hand, tilt the analog stick down to draw the targeting cursor.\n" +
                                        "Release the analog stick back to neutral to confirm that as the location your will teleport too.\n" +
                                        "\n[YOUR GOAL]: Teleport to the new highlighted area.";
                break;
            case 7: // 'Teleport Movement' Complete
                _btnContinue.interactable = true;
                gameObject.transform.position = _menuTransforms[2].position;
                _curTaskTopicTitle.text = "Teleportation: Complete";
                _curTaskBodyText.text = "Yay, you did it. Let's continue on to interacting with the environment.";
                break;
            
            // ## Interaction Tutorials
            case 8: // 'Interaction' Overview
                _curTaskTopicTitle.text = "Interaction";
                _curTaskBodyText.text = "There are two primary way to interact with objects.\n" +
                                        "\n- Proximity Grab" +
                                        "\n- Ranged Grab";
                _btnContinue.gameObject.SetActive(true);
                break;
            case 9: // 'Proximity Grab Pt. 1' Task
                _btnContinue.interactable = false;
                activateNextTask?.Invoke();
                _curTaskTopicTitle.text = "Proximity Grab Pt. 1";
                _curTaskBodyText.text = "Let's start with proximity grab.\n This simply means that your hand has to be close to a grabbable object to hold it.\n" +
                                        "To do this, reach for that ball in the tray in front of you, hold grip on your controller to grab it.\n" +
                                        "\n[YOUR GOAL]: Try dropping the ball into the highlighted area in the cardboard box.";
                break;
            case 10: // 'Proximity Grab Pt 2'
                _curTaskTopicTitle.text = "Proximity Grab Pt. 2";
                activateNextTask?.Invoke();
                _curTaskBodyText.text = "Excellent job. Now try opening the drawer to the desk in front of you. You'll find another ball inside.\n" +
                                        "\n[YOUR GOAL]: This time, try throwing the ball into the highlighted area.";
                break;
            case 11: // 'Ranged Grab' Task
                _btnContinue.interactable = false;
                activateNextTask?.Invoke();
                _curTaskTopicTitle.text = "Ranged Grab";
                _curTaskBodyText.text = "Now we will explain how to grab things from afar.\n" +
                                        "Without joining to the dark side, extend your hand out toward the ball.\n" +
                                        "There will be a small icon letting you know when you are targeting it.\n" +
                                        "Hold the grip button as before, to have it fly into your hand.\n" +
                                        "\n[YOUR GOAL]: Drop this third ball into the box.";
                break;
            case 12: // 'Ranged Grab' Complete
                _btnContinue.interactable = true;
                _curTaskTopicTitle.text = "Ranged Grab: Complete";
                _curTaskBodyText.text = "Excellent. Now that you know how to grab things, lets move on to your backpack.";
                break;
            case 13: // 'Backpack' Task
                _btnContinue.interactable = false;
                activateNextTask?.Invoke();
                _curTaskTopicTitle.text = "Your Backpack";
                _curTaskBodyText.text = "Your backpack will contain items that are relevant to the activity you are currently interacting with.\n" +
                                        "To access your backpack, take either hand and reach over the back of your shoulder and squeeze the grip button.\n" +
                                        "You'll see that you have a ball there as well. To pull an item out of your backpack, simply proximity grab it like before.\n" +
                                        "\n[YOUR GOAL]: Drop that fourth ball into the box."; 
                break;
            case 14: // 'Backpack' Task Complete
                _btnContinue.interactable = true;
                _curTaskTopicTitle.text = "Basic Tutorial Completed";
                _curTaskBodyText.text = "Excellent job! Now you know all the basics needed to being exploring the Infinity Center.\n" +
                                        "\nEnjoy!";
                break;
            
            case 15:
                SceneManager.LoadScene("LogInScreen");
                break;
        }
        
        if(_enableDebugLogs) Debug.Log("Task Index Incremented.");
        _curTutorialProgressIndex++;
    }

    #endregion
}
