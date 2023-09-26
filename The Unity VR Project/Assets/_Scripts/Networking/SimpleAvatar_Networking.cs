using FishNet.Object;
using UnityEngine;
using UnityEngine.SpatialTracking;

public class SimpleAvatar_Networking : NetworkBehaviour
{
    [SerializeField] private GameObject _gridPrefab;

    [Header("Components to disable when not owner:")] [SerializeField]
    private TrackedPoseDriver _headDriver;

    [SerializeField] private TrackedPoseDriver _leftHandDriver;
    [SerializeField] private TrackedPoseDriver _rightHandDriver;
    [SerializeField] private AudioListener _audioListener;
    [SerializeField] private Camera _camera;


    [Header("Debugging")] [SerializeField] private bool _enableDebugLogs = true;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (_enableDebugLogs)
        {
            Debug.Log("SimpleAvatar_Networking (SAN) OnStartClient");
        }

        gameObject.name += OwnerId;


        if (!IsOwner)
        {
            if (_enableDebugLogs)
            {
                Debug.Log("SAN - Not Owned By Self");
            }

            _headDriver.enabled = false;
            _rightHandDriver.enabled = false;
            _leftHandDriver.enabled = false;
            _audioListener.enabled = false;
            _camera.enabled = false;
            return;
        }

        if (_enableDebugLogs)
        {
            Debug.Log("SAN: Is Owned By You");
        }
    }
}