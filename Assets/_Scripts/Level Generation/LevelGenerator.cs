using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Game.CoreSystem;
using Unity.Cinemachine;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;

// using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelGenerator : MonoBehaviour
{
    public GameObject player;
    public float playerOffset = 2;

    [Header("Cameras")]
    public MainCamera mainCamera;
    public CinemachineCamera singleplayerCinemachineCamera;
    public CinemachineCamera multiplayerCinemachineCamera;
    public CinemachineTargetGroup multiplayerTargetGroup;

    [Header("User Interfaces")]
    public GameplayCanvas gameplayCanvas;
    public DeathScreenManager deathScreen;
    public MenuManager pauseMenu;
    public UpgradeMenuManager upgradeMenu;
    
    [Header("Rooms")]
    public RoomNode roomMap;
    private int roomNumber = 1;
    private int playerCount;

    PlayerInputManager playerInputManager;

    void Awake()
    {
        playerCount = PlayerPrefs.GetInt("playerCount");
        playerInputManager = FindFirstObjectByType<PlayerInputManager>();

        spawnRoomMap();

        InputSystem.onDeviceChange += OnDeviceChange;

        if (spawnStartingRoom() && spawnAllRooms(roomMap)) 
        {
            spawnAllEnemies(roomMap);

            if(playerCount == 1)
            {
                spawnPlayer(1);
                UnityEngine.Debug.Log("1 PLAYER");
            }
            else if(playerCount == 2)
            {
                spawnPlayer(2);
                UnityEngine.Debug.Log("2 PLAYERS");
            }
            else    // FOR TESTING
            {
                // spawnPlayer();
            }


        } 
        else 
        {
            UnityEngine.Debug.LogError("Level generation error");
        }
    }


    void OnDestroy()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    #region SpawnRooms
    void spawnRoomMap() {
        if (roomMap != null) {
            roomMap = Instantiate(roomMap);
        } else {
            UnityEngine.Debug.LogError(gameObject.name + ": Room Map field not filled");
        }
    }

    bool spawnStartingRoom() {
        // find origin location for level generator
        GameObject levelOrigin = GameObject.Find("LevelOrigin");
        Transform levelTransform;

        // create starting room and find room origin location
        GameObject newRoom = spawnRoom(roomMap);
        GameObject roomOrigin = GameObject.Find("RoomOrigin");
        Transform roomTransform;

        if (levelOrigin != null && roomOrigin != null && newRoom != null) {
            levelTransform = levelOrigin.transform;
            roomTransform = roomOrigin.transform;

            // change room location to difference in origins -> origins overlap
            newRoom.transform.position += levelTransform.position - roomTransform.position;
        } else {
            UnityEngine.Debug.LogError("Level Origin, Room Origin, or Room not found");
            return false;
        }

        return true;
    }

    bool spawnAllRooms(RoomNode root) { // supports rooms with one entrance, 0-2 exits
        RoomNode currNode = root; 
        int randIndex = UnityEngine.Random.Range(0,2);

        if (currNode.children.Count == 0) {
            return true;
        } else if (currNode.children.Count == 1) {
            RoomManager newRoomManager = spawnRoom(currNode.children[0]).GetComponent<RoomManager>();

            connectRooms(currNode, 0, currNode.children[0], 0);

            spawnAllRooms(currNode.children[0]); // recursively call child node
        } else if (currNode.children.Count == 2) {
            RoomManager roomManager1 = spawnRoom(currNode.children[0]).GetComponent<RoomManager>();
            RoomManager roomManager2 = spawnRoom(currNode.children[1]).GetComponent<RoomManager>();

            connectRooms(currNode, randIndex, currNode.children[0], 0);
            connectRooms(currNode, Math.Abs(randIndex-1), currNode.children[1], 0);

            spawnAllRooms(currNode.children[0]); // recursively call on both children
            spawnAllRooms(currNode.children[1]);
        } else {
            UnityEngine.Debug.LogError(gameObject.name + ": Unexpected number of exits");
            return false;
        }

        return true;
    }


    GameObject spawnRoom(RoomNode r) {
        // Instantiate a random resource of roomType
        List<RoomManager> roomList = new List<RoomManager>();
        
        if (r.roomType == "") {
            UnityEngine.Debug.LogError(r.name + ": roomType left empty");
        } else {
            roomList = Resources.LoadAll<RoomManager>("Rooms/" + r.roomType).ToList();
            if (roomList.Count == 0) {
                UnityEngine.Debug.LogError("Failed to load files from Resources/Rooms/" + r.roomType);
            } else {
                r.roomObject = Instantiate(roomList[UnityEngine.Random.Range(0, roomList.Count)].gameObj, new Vector3(0, 0, 0), transform.rotation); 
                r.roomObject.name = "Room_" + roomNumber++;
            }
        }
        return r.roomObject;
    }
    
    bool connectRooms(RoomNode currNode, int exitIdx, RoomNode newNode, int entranceIdx) {
        // UnityEngine.Debug.Log(currNode.gameObject.name + ": " + exitIdx + ", " + entranceIdx);

        RoomManager currNodeManager = currNode.roomObject.GetComponent<RoomManager>();
        RoomManager newNodeManager = newNode.roomObject.GetComponent<RoomManager>();

        newNode.roomObject.transform.position += currNodeManager.exits[exitIdx].transform.position - newNodeManager.entrances[entranceIdx].transform.position;
        
        foreach (BoxCollider2D collider in newNodeManager.colliders) {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Room")) {
                RoomCollider roomCollider = collider.GetComponent<RoomCollider>();
                roomCollider.enableCollider();
                roomCollider.tryCollider();
                if (roomCollider.hasCollision) {                
                    // UnityEngine.Debug.Log($"Collision detected: {collider.transform.parent}");
                }    
            }
        }

        // List<Collider2D> results = new List<Collider2D>();
        // ContactFilter2D contactFilter = new ContactFilter2D();
        // contactFilter.useTriggers = false; // Ignore triggers if necessary
        // contactFilter.SetLayerMask(LayerMask.GetMask("Room")); // Ensure the objects are on the "Room" layer
        // int overlaps = 0;

        // foreach (Collider2D collider in newNodeManager.colliders) {
        //     overlaps += Physics2D.OverlapCollider(collider, results);

        //     foreach (Collider2D overlappingCollider in results) {
        //         string colliderParentName = collider.transform.parent != null ? collider.transform.parent.name : collider.gameObject.name;
        //         string overlappingParentName = overlappingCollider.transform.parent != null ? overlappingCollider.transform.parent.name : overlappingCollider.gameObject.name;
                
        //         UnityEngine.Debug.Log($"Overlap detected: {colliderParentName} is overlapping with {overlappingParentName}");
        //     }
        //     results.Clear();
        // }

        // if (overlaps != 0) {
        //     UnityEngine.Debug.LogError("collision overlap " + newNode.gameObject.name + ": " + overlaps);
        // }
        
        return true;
    }
    #endregion SpawnRooms

    bool spawnAllEnemies(RoomNode root) { 
        RoomNode currNode = root; 
        int randIndex = UnityEngine.Random.Range(0,2);

        if (currNode.children.Count == 0) {
            return true;
        } 

        foreach (RoomNode child in currNode.children) {
            RoomManager childRoomManager = child.roomObject.GetComponent<RoomManager>();
            childRoomManager.SpawnEnemies(); // Spawn enemies in the new room
            spawnAllEnemies(child);
        } 

        return true;
    }

    #region SpawnPlayer

    /* void spawnPlayer() {
        GameObject levelOrigin = GameObject.Find("LevelOrigin");
        Transform levelTransform;

        if (levelOrigin != null) {
            levelTransform = levelOrigin.transform;
            player = Instantiate(player, new Vector3(levelTransform.position.x, levelTransform.position.y + playerOffset, 0), levelTransform.rotation);
            
            
            cinemachineCamera.Target.TrackingTarget = player.transform;
        } else {
            UnityEngine.Debug.LogError("Level Origin not found");
        }
    } */
    void setCameras(PlayerInput newPlayer)
    {
        int activePlayerCount = PlayerInput.all.Count;
        if (activePlayerCount == 1)
        {
            singleplayerCinemachineCamera.enabled = true;
            multiplayerCinemachineCamera.enabled = false;
            if (newPlayer.playerIndex == 0)
            {
                singleplayerCinemachineCamera.Target.TrackingTarget = newPlayer.transform;
                UnityEngine.Debug.Log("Singleplayer camera set to player 1");
            }

            mainCamera.player1 = newPlayer.gameObject;
            mainCamera.player2 = null;
        }
        else if (activePlayerCount == 2)
        {
            singleplayerCinemachineCamera.enabled = false;
            multiplayerCinemachineCamera.enabled = true;
            // multiplayerTargetGroup.AddMember(newPlayer.transform, 1f, 13.33f);
            multiplayerTargetGroup.AddMember(PlayerInput.all[0].transform, 2f, 0f);
            multiplayerTargetGroup.AddMember(PlayerInput.all[1].transform, 1f, 0f);

            mainCamera.player1 = PlayerInput.all[0].gameObject;
            mainCamera.player2 = PlayerInput.all[1].gameObject;
        }
        else
        {
            UnityEngine.Debug.LogError("Invalid player count for camera setup.");
        }
    }

    void setUserInterface(PlayerInput newPlayer)
    {
        int activePlayerCount = PlayerInput.all.Count;
        UnityEngine.Debug.Log("Setting UI for player " + (newPlayer.playerIndex + 1) + " with " + activePlayerCount + " active players.");
        
        /* GameplayCanvas gameplayCanvasScript = gameplayCanvas.GetComponent<GameplayCanvas>();
        MenuManager pauseMenuManager = pauseMenu.GetComponent<MenuManager>();
        UpgradeMenuManager upgradeMenuManager = upgradeMenu.GetComponent<UpgradeMenuManager>(); */

        if (activePlayerCount == 1)
        {
            UnityEngine.Debug.Log("Singleplayer UI set for player 1");      
            gameplayCanvas.playerHealthBar1.SetActive(true);
            gameplayCanvas.playerHealthBar2.SetActive(false);

            if (newPlayer.playerIndex == 0)
            {
                HealthBar playerHealthBar1 = gameplayCanvas.playerHealthBar1.GetComponent<HealthBar>();
                playerHealthBar1.SetPlayer(newPlayer.gameObject);
                // Does nothing, but needed for some reason
                HealthBar playerHealthBar2 = gameplayCanvas.playerHealthBar2.GetComponent<HealthBar>();
                playerHealthBar2.SetPlayer(newPlayer.gameObject);
                pauseMenu.player1 = newPlayer.gameObject;
                upgradeMenu.player1 = newPlayer.gameObject;
            }   
            else
            {
                UnityEngine.Debug.LogError("Player index mismatch for singleplayer UI setup.");
            }
        }   
        else if (activePlayerCount == 2)
        {
            UnityEngine.Debug.Log("Multiplayer UI set for players 1 and 2");
            gameplayCanvas.playerHealthBar1.SetActive(true);
            gameplayCanvas.playerHealthBar2.SetActive(true);

            if (newPlayer.playerIndex == 0)
            {
                HealthBar playerHealthBar1 = gameplayCanvas.playerHealthBar1.GetComponent<HealthBar>();
                playerHealthBar1.SetPlayer(newPlayer.gameObject);
                pauseMenu.player1 = newPlayer.gameObject;
                upgradeMenu.player1 = newPlayer.gameObject;
            }
            else if (newPlayer.playerIndex == 1)
            {
                HealthBar playerHealthBar2 = gameplayCanvas.playerHealthBar2.GetComponent<HealthBar>();
                playerHealthBar2.SetPlayer(newPlayer.gameObject);
                pauseMenu.player2 = newPlayer.gameObject;
                upgradeMenu.player2 = newPlayer.gameObject;
            }
            else
            {
                UnityEngine.Debug.LogError("Player index mismatch for multiplayer UI setup.");
            }
        }
        else
        {
            UnityEngine.Debug.LogError("Invalid player count for UI setup.");
        }

        gameplayCanvas.SetDependencies();
        pauseMenu.SetDependencies();
        upgradeMenu.SetDependencies();
    }

    void spawnPlayer(int count)
    {
        GameObject levelOrigin = GameObject.Find("LevelOrigin");
        Transform levelTransform;

        if (levelOrigin != null)
        {
            levelTransform = levelOrigin.transform;

            if (playerInputManager == null)
            {
                UnityEngine.Debug.LogError("PlayerInputManager not found in the scene.");
                return;
            }

            for (int i = 0; i < count; i++)
            {
                PlayerInput newPlayer = playerInputManager.JoinPlayer(i);
                if (newPlayer != null)
                {
                    UnityEngine.Debug.Log("Player " + (i + 1) + " joined successfully!");
                    newPlayer.transform.position = new Vector3(levelTransform.position.x, levelTransform.position.y + playerOffset, 0);
                    
                    setCameras(newPlayer);
                    setUserInterface(newPlayer);
                }
                else
                {
                    UnityEngine.Debug.LogWarning("Waiting for second input device to join player " + (i + 1));
                    StartCoroutine(WaitForSecondPlayer(playerInputManager, i));
                    break;
                }
            }
        }
        else
        {
            UnityEngine.Debug.LogError("Level Origin not found");
        }
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (change == InputDeviceChange.Disconnected)
        {
            UnityEngine.Debug.LogWarning($"Device disconnected: {device.displayName}");

            // Check if the device belonged to a player and handle disconnection
            var player = PlayerInput.all.FirstOrDefault(p => p.devices.Contains(device));
            if (player != null)
            {
                UnityEngine.Debug.LogWarning($"Player {player.playerIndex + 1} disconnected. Waiting for reconnection...");
                StartCoroutine(WaitForSecondPlayer(playerInputManager, player.playerIndex));
            }
        }
        else if (change == InputDeviceChange.Reconnected)
        {
            UnityEngine.Debug.Log($"Device reconnected: {device.displayName}");
        }
    }

    private IEnumerator WaitForSecondPlayer(PlayerInputManager playerInputManager, int playerIndex)
    {
        GameObject levelOrigin = GameObject.Find("LevelOrigin");
        Transform levelTransform;
        levelTransform = levelOrigin.transform;
        while (true)
        {
            if (InputSystem.devices.Count > 1) // Check if there are multiple input devices
            {
                PlayerInput newPlayer = playerInputManager.JoinPlayer(playerIndex);
                if (newPlayer != null)
                {
                    UnityEngine.Debug.Log("Second player joined successfully!");
                    newPlayer.transform.position = new Vector3(levelTransform.position.x, levelTransform.position.y + playerOffset, 0);
                    setCameras(newPlayer);
                    setUserInterface(newPlayer);
                    yield break;
                }
            }

            yield return new WaitForSeconds(0.5f); // Check every half second
        }
    }

    #endregion SpawnPlayer

    // bool generateLevel_1() {
    //     // startRoom -> hallway -> hallway -> endRoom
    //     GameObject currRoom;
    //     RoomManager currRoomMan;
    //     GameObject newRoom;
    //     RoomManager newRoomMan;
    //     int numHallways = Random.Range(3, 5);
    //     int i = 0;

    //     // Instantiate multiple hallways
    //     for (i = 0; i < numHallways; i++) {
    //         currRoom = roomObjQueue[i];
    //         currRoomMan = currRoom.GetComponent<RoomManager>();

    //         newRoom = Instantiate(hallways[Random.Range(0, hallways.Count)].gameObj, new Vector3(0, 0, 0), transform.rotation);
    //         newRoomMan = newRoom.GetComponent<RoomManager>();
    //         roomObjQueue.Add(newRoom);

    //         newRoom.transform.position += currRoomMan.exits[0].transform.position - newRoomMan.entrances[0].transform.position;
    //     }

    //     // Instantiate end room
    //     currRoom = roomObjQueue[i];
    //     currRoomMan = currRoom.GetComponent<RoomManager>();

    //     newRoom = Instantiate(endRoom.gameObj, new Vector3(0, 0, 0), transform.rotation);
    //     newRoomMan = newRoom.GetComponent<RoomManager>();
    //     roomObjQueue.Add(newRoom);

    //     newRoom.transform.position += currRoomMan.exits[0].transform.position - newRoomMan.entrances[0].transform.position;

    //     return true;
    // }

    // void spawnStartingRoom() {
    //     // find origin location for level generator
    //     GameObject levelOrigin = GameObject.Find("LevelOrigin");
    //     Transform levelTransform;

    //     // create starting room and find room origin location
    //     GameObject newRoom = Instantiate(startRoom.gameObj, new Vector3(0, 0, 0), transform.rotation);   
    //     roomObjQueue.Add(newRoom);

    //     GameObject roomOrigin = GameObject.Find("RoomOrigin");
    //     Transform roomTransform;

    //     if (levelOrigin != null && roomOrigin != null) {
    //         levelTransform = levelOrigin.transform;
    //         roomTransform = roomOrigin.transform;

    //         // change room location to difference in origins -> origins overlap
    //         newRoom.transform.position += levelTransform.position - roomTransform.position;
    //     } else {
    //         Debug.LogError("Level or Room Origin not found");
    //     }
    // }
}
