using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatableItemBehavior : MonoBehaviour
{
    public GoldbergManager gbmInstance;
    public GameObject thisObj;

    private void Awake()
    {
        gbmInstance = GoldbergManager.instance;
        thisObj = gameObject;
    }

    public void AddSelfToPuzzle()
    {
    }
}
