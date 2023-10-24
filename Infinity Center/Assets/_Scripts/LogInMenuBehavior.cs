using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LogInMenuBehavior : MonoBehaviour
{

    #region | Variables |

    [SerializeField] private GameObject _credentialPrompt;
    [SerializeField] private GameObject _rolePrompt;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _settingMenu;

    #endregion

    #region | Unity Methods |

    private void Start()
    {
        _credentialPrompt.SetActive(true);
        _rolePrompt.SetActive(false);
        _mainMenu.SetActive(false);
        _settingMenu.SetActive(false);
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

    #endregion
}
