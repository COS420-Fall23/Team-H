using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldbergLevel : MonoBehaviour
{
    #region | Variables |

    [SerializeField] private GameObject _goalBall;
    private Rigidbody _goalBallRigidBody;
    
    public TaskHighlighterBehavior taskHighlighter;
    private Vector3 _startingBallPos;

    #endregion

    #region | Unity Methods |

    private void OnEnable()
    {
        _goalBallRigidBody = _goalBall.GetComponent<Rigidbody>();
        _startingBallPos = _goalBall.transform.position;
    }

    #endregion

    #region | Custom Methods |

    public void StartLevel()
    {
        _goalBallRigidBody.isKinematic = false;
    }

    public void ResetLevel()
    {
        _goalBallRigidBody.isKinematic = true;
        _goalBall.transform.position = _startingBallPos;
    }

    public void CompleteLevel()
    {
        // Not sure yet.
    }

    #endregion
}
