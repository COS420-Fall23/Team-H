using System;
using UnityEngine;
using FishyRealtime;
using Random = UnityEngine.Random;

public class Matchmaker : MonoBehaviour
{
    public static Matchmaker Instance;

    //[SerializeField] private GameObject _connectionHUD;
    [SerializeField] private GameObject _connectingScreen;
    // [SerializeField] private GameObject _joiningRoomScreen;
    // [SerializeField] private GameObject _creatingRoomScreen;

    [SerializeField] private string _roomName = "LatticeLand_IMRE";

    private bool _isConnectedToMaster;

    [SerializeField] private FishyRealtime.FishyRealtime _fishyRealtime;

    private Room _labRoom;

    private void Awake()
    {
        _labRoom = new Room()
        {
            isPublic = true,
            open = true,
            name = _roomName
        };
    }

    private void Start()
    {
        if (Instance != null) Destroy(Instance);
        Instance = this;
        
        _connectingScreen.SetActive(true);
    #if UNITY_ANDROID
        _fishyRealtime.ConnectedToMaster += FishyRealtime_ConnectedToMaster;
    #else
        
    #endif
    }

    private void FishyRealtime_ConnectedToMaster(object sender, EventArgs e)
    {
        Debug.Log("ConnectedToServer Event Triggered");
        _isConnectedToMaster = true;
        _connectingScreen.SetActive(false);
        SetUsername("User_" + Random.Range(0, 999));
    }

    public void SearchForRooms()
    {
        /*Room[] rooms = FishyRealtime.FishyRealtime.Instance.GetRoomList();
        if (rooms.Length > 0)
        {
            // Join the first room available
            if (!FishyRealtime.FishyRealtime.isConnectedToMaster)
            {
                Debug.Log("Attempt to Join Fail: Not Connected to Master- Trying Again");
                SearchForRooms();
                return;
            }
            
            Debug.Log($"Joining Room {rooms[0].name}: Connected to Mater");
            _joiningRoomScreen.SetActive(true);
            FishyRealtime.FishyRealtime.Instance.JoinRoom(_labRoom);
        }
        else
        {
            // Create a new room.
            if (!FishyRealtime.FishyRealtime.isConnectedToMaster)
            {
                Debug.Log("Attempt to Create Fail: Not Connected To Master- Trying Again");
                SearchForRooms();
                return;
            }
            
            Debug.Log($"Creating Room {_labRoom.name} : Connected to Mater");
            _creatingRoomScreen.SetActive(true);
            FishyRealtime.FishyRealtime.Instance.CreateRoom(_labRoom);
            SearchForRooms();
        }*/
    }

    private void OnDestroy()
    {
        FishyRealtime.FishyRealtime.Instance.ConnectedToMaster -= FishyRealtime_ConnectedToMaster;
    }

    public void SetUsername(string username)
    {
        FishyRealtime.FishyRealtime.Instance.playerName = username;
    }

    public void JoinRandom()
    {
        if (!_isConnectedToMaster) return;
        FishyRealtime.FishyRealtime.Instance.JoinRandomRoom(true);
    }

    public bool IsConnectedToMaster()
    {
        return _isConnectedToMaster;
    }
}


