using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogInMenuBehavior : MonoBehaviour
{
    [SerializeField] private GameObject _CredentialPrompt;
    [SerializeField] private GameObject _RolePrompt;

    public void OnLogInPress()
    {
        _CredentialPrompt.SetActive(false);
        _RolePrompt.SetActive(true);
    }

    public void BackToLogIn()
    {
        _CredentialPrompt.SetActive(true);
        _RolePrompt.SetActive(false);
    }
}
