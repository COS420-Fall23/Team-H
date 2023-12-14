using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class VelocityArrow : MonoBehaviour
{
    #region | Variables |

    private GameObject _parentObj;
    private Rigidbody _parentRigidbody;
    private GameObject _arrowInstance;

    [SerializeField] private GameObject _momentumArrowPrefab;
    [SerializeField] private float _velocityDeadZone;

    #endregion
    
    #region | Unity Methods |

    private void Awake()
    {
        Transform parentTransform = GetComponentInParent<Transform>();
        _parentObj = parentTransform.gameObject;
        _parentRigidbody = _parentObj.GetComponent<Rigidbody>();
        _arrowInstance = Instantiate(_momentumArrowPrefab, parentTransform.position, _parentObj.transform.rotation, parentTransform);
        _arrowInstance.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (IsVelocitBeyondDeadzone(GetAbsoluteVelocityValue(_parentRigidbody.velocity), _velocityDeadZone))
        {
            DrawMomentumArrow();
        }
        else if (_arrowInstance.activeSelf) _arrowInstance.SetActive(false);
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
            if(!_arrowInstance.activeSelf) _arrowInstance.SetActive(true);
            _arrowInstance.transform.rotation = Quaternion.LookRotation(_parentRigidbody.velocity);
    }
    
    #endregion
    
}
