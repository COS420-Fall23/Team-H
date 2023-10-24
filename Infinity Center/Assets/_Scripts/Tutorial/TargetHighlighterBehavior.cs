using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHighlighterBehavior : MonoBehaviour
{
    #region | Variables |

    [SerializeField] private ParticleSystem _completionConfetti;
    [SerializeField] private GameObject _arrows;
    [SerializeField] private GameObject _lightRay;
    [SerializeField] private GameObject _floorRing;

    #endregion

    #region | Unity Methods |

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    #endregion
    
    public void Activate()
    {
        _floorRing.SetActive(true);
        _lightRay.SetActive(true);
        _arrows.SetActive(true);
        gameObject.SetActive(true);
    }

    public void TriggerCompletion()
    {
        _floorRing.SetActive(false);
        _lightRay.SetActive(false);
        _arrows.SetActive(false);
        _completionConfetti.Play();
    }
}
