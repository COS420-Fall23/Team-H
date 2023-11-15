using System;
using System.Collections;
using System.Collections.Generic;
using HurricaneVR.Framework.Core.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LogInMenuBehavior : MonoBehaviour
{

    #region | Variables |

    [SerializeField] private GameObject _credentialPrompt;
    [SerializeField] private GameObject _rolePrompt;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _settingMenu;
    [SerializeField] private GameObject _txtExhibitComingSoon;

    [SerializeField] private HVRPlayerController _playerController;

    #endregion

    #region | Unity Methods |

    private void Start()
    {
        _credentialPrompt.SetActive(true);
        _rolePrompt.SetActive(false);
        _mainMenu.SetActive(false);
        _settingMenu.SetActive(false);
        _txtExhibitComingSoon.SetActive(false);

        _playerController.MoveSpeed = 0;
        _playerController.RunSpeed = 0;
    }

    #endregion
    
    #region | Custom Methods |

    public void OnLogInPress()
    {
        _credentialPrompt.SetActive(false);
        _rolePrompt.SetActive(true);
    }

    public void BackToLogIn()
    {
        _credentialPrompt.SetActive(true);
        _rolePrompt.SetActive(false);
        _mainMenu.SetActive(false);
        _settingMenu.SetActive(false);
    }

    public void LoginAsTeacher()
    {
        _rolePrompt.SetActive(false);
        _mainMenu.SetActive(true);
    }

    public void LogInAsStudent()
    {
        _rolePrompt.SetActive(false);
        _mainMenu.SetActive(true);
    }

    public void ToggleSettings()
    {
        if (_settingMenu.activeInHierarchy)
        {
            _settingMenu.SetActive(false);
            _mainMenu.SetActive(true);
        }
        else
        {
            _settingMenu.SetActive(true);
            _mainMenu.SetActive(false);
        }
    }

    public void StartExhibit()
    {
        _txtExhibitComingSoon.SetActive(true);
    }

    public void StartTutorial()
    {
        SceneManager.LoadScene("BasicTutorial");
    }

    #endregion
}
