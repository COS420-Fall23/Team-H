using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public void LoadClassroomScene()
    {
        SceneManager.LoadScene("TheClassroom");
    }

    public void LoadLatticeLand()
    {
        SceneManager.LoadScene("LatticeLand");
    }

    public void LoadLoginScreen()
    {
        SceneManager.LoadScene("LogInScreen");
    }

    public void LoadGoldbergScene()
    {
        SceneManager.LoadScene("GoldbergLab");
    }

    public void LoadBasicTutorial()
    {
        SceneManager.LoadScene("BasicTutorial");
    }

    public void LoadInfinityCenter()
    {
        SceneManager.LoadScene("InfinityCenter");
    }

}
