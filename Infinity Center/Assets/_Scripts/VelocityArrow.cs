using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Serialization;

public class VelocityArrow : MonoBehaviour
{
    #region | Variables |

    private GameObject _parentObj;
    private Rigidbody _parentRigidbody;

    [SerializeField] private GameObject _velocityArrowMesh;
    [SerializeField] private float _velocityDeadZone;

    #endregion
    
    #region | Unity Methods |

    private void Awake()
    {
        Transform parentTransform = GetComponentInParent<Transform>();
        _parentObj = parentTransform.gameObject;
        _parentRigidbody = _parentObj.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (IsVelocitBeyondDeadzone(GetAbsoluteVelocityValue(_parentRigidbody.velocity), _velocityDeadZone))
        {
            DrawMomentumArrow();
        }
        else if (_velocityArrowMesh.activeSelf) _velocityArrowMesh.SetActive(false);
    }

    #endregion
    
    #region | Custom Methods |

    private Vector3 GetAbsoluteVelocityValue(Vector3 objVelocity)
    {
        return new Vector3(Mathf.Abs(objVelocity.x),
            Mathf.Abs(objVelocity.y), Mathf.Abs(objVelocity.z));
    }

    private bool IsVelocitBeyondDeadzone(Vector3 absoluteVelocity, float deadZone)
    {
        if (absoluteVelocity.x > deadZone || absoluteVelocity.y > deadZone || absoluteVelocity.z > deadZone)
        {
            return true;
        }
        return false;
    }

    private void DrawMomentumArrow()
    {
            if(!_velocityArrowMesh.activeSelf) _velocityArrowMesh.SetActive(true);
            _velocityArrowMesh.transform.rotation = Quaternion.LookRotation(_parentRigidbody.velocity);
    }
    
    #endregion
    
}
