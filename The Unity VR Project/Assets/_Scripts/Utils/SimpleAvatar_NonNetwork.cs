using FishNet;
using FishNet.Managing;
using FishNet.Transporting;
using FishyRealtime.Samples;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleAvatar_NonNetwork : MonoBehaviour
{
    [SerializeField] private Matchmaker _matchmaker;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TMP_Text _pullTrigTxt;
    [SerializeField] private TMP_Text _isServerTxt;
    [SerializeField] private TMP_Text _isClientTxt;
    [SerializeField] private TMP_Text _statusTxt;

    public InputAction goOnline;

    private void OnEnable()
    {
        goOnline.Enable();
    }

    private void Update()
    {
        if (_gameManager.enabled && goOnline.IsPressed())
        {
            _gameManager.Login();
        }
        else if (goOnline.IsPressed() && _matchmaker.IsConnectedToMaster())
        {
            _matchmaker.JoinRandom();
            Destroy(this);
        }
        
        UpdateNetworkStatusHUD();
    }

    private void UpdateNetworkStatusHUD()
    {
        // Control Input Feedback
        if (goOnline.IsPressed()) _pullTrigTxt.color = Color.green;
        else if(!goOnline.IsPressed())_pullTrigTxt.color = Color.white;

        if (InstanceFinder.IsServer)
        {
            _isServerTxt.text = "True";
            _isServerTxt.color = Color.green;
        }
        else
        {
            _isServerTxt.text = "False";
            _isServerTxt.color = Color.red;
        }
        
        if (InstanceFinder.IsClient)
        {
            _isClientTxt.text = "True";
            _isClientTxt.color = Color.green;
        }
        else
        {
            _isClientTxt.text = "False";
            _isClientTxt.color = Color.red;
        }

        if (InstanceFinder.IsOffline)
        {
            _statusTxt.text = "Offline";
            _statusTxt.color = Color.red;
        }
        else if(!InstanceFinder.IsOffline)
        {
            _statusTxt.text = "Online";
            _statusTxt.color = Color.green;
        }
    }
}