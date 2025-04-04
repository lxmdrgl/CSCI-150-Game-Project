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
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

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
    public InputMenuManager inputMenu;
    
    [Header("Rooms")]
    public RoomNode roomMap;
    private int roomNumber = 1;
    private int playerCount;
    private InputDevice replaceInputDevice;
    private List<Entity> enemies = new List<Entity>();
    PlayerInputManager playerInputManager;

    void Awake()
    {
        playerCount = PlayerPrefs.GetInt("playerCount");
        playerInputManager = FindFirstObjectByType<PlayerInputManager>();

        //DontDestroyOnLoad(gameObject);

        spawnRoomMap();

        InputSystem.onDeviceChange += OnDeviceChange;
        inputMenu.OnButtonClickedEvent += index => ReplacePlayerInput(replaceInputDevice, index);

        if (spawnStartingRoom() && spawnAllRooms(roomMap)) 
        {

            // spawnAllEnemies(roomMap);

            UnityEngine.Debug.Log("player count: " + playerCount);
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
            else    
            {
                spawnPlayer(1);
                UnityEngine.Debug.Log("PLAYERCOUNT NOT SET");
            }

            InputUser.onUnpairedDeviceUsed += OnUnpairedDeviceUsed;
            // PlayerInput.all[0].user.UnpairDevices();
            // UnityEngine.Debug.Log("Unpairing player 1 input devices: " + PlayerInput.all[0].user.pairedDevices.Count);
        } 
        else 
        {
            UnityEngine.Debug.LogError("Level generation error");
        }
    }


    void OnDestroy()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
        InputUser.onUnpairedDeviceUsed -= OnUnpairedDeviceUsed;
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

    void setPlayerDependencies(PlayerInput newPlayer)
    {
        UnityEngine.Debug.Log("Setting player dependencies for player " + (newPlayer.playerIndex + 1));
        setCameras(newPlayer);
        setUserInterface(newPlayer);
        setEnemyDependencies();
    }

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
                inputMenu.player1 = newPlayer.gameObject;
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
                inputMenu.player1 = newPlayer.gameObject;
            }
            else if (newPlayer.playerIndex == 1)
            {
                HealthBar playerHealthBar2 = gameplayCanvas.playerHealthBar2.GetComponent<HealthBar>();
                playerHealthBar2.SetPlayer(newPlayer.gameObject);
                pauseMenu.player2 = newPlayer.gameObject;
                upgradeMenu.player2 = newPlayer.gameObject;
                inputMenu.player2 = newPlayer.gameObject;
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
        inputMenu.SetDependencies();
    }

    private void setEnemyDependencies()
    {
        enemies = FindObjectsByType<Entity>(FindObjectsSortMode.None).ToList();
        foreach (Entity enemy in enemies)
        {
            enemy.SetDependencies();
        }
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
                    newPlayer.transform.rotation = levelTransform.rotation;
                    
                    setPlayerDependencies(newPlayer);
                    /* setCameras(newPlayer);
                    setUserInterface(newPlayer);
                    setEnemyDependencies(); */
                }
                else
                {
                    UnityEngine.Debug.LogWarning("Waiting for second input device to join player " + (i + 1));

                    // Pause the game, UI saying wait for second player, disable player 1 input (PlayerInput.all[0])

                    StartCoroutine(WaitForUnpairedPlayer(playerInputManager, i));
                    break;
                }

                // GameObject newPlayer = Instantiate(player, new Vector3(levelTransform.position.x, levelTransform.position.y + playerOffset, 0), levelTransform.rotation);
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

                // Pause the game, UI saying second player disconnected, disable player 1 input (PlayerInput.all[0])

                StartCoroutine(WaitForUnpairedPlayer(playerInputManager, player.playerIndex));
            }
        }
        else if (change == InputDeviceChange.Reconnected)
        {
            UnityEngine.Debug.Log($"Device reconnected: {device.displayName}");
        }
        else if (change == InputDeviceChange.Added)
        {
            UnityEngine.Debug.Log($"New device detected: {device.displayName}");

            /* if (PlayerInput.all.Count == 1)
            {
                ReplacePlayerInput(device, 0);
            }
            if (PlayerInput.all.Count >= 2)
            {
                // replace either device 0 or device 1 
            } */
        }
    }

    private void OnUnpairedDeviceUsed(InputControl control, InputEventPtr eventPtr)
    {
        InputDevice newDevice = control.device;

        // Check if there are any players
        if (PlayerInput.all.Count == 0)
        {
            UnityEngine.Debug.LogWarning("No players available to assign the device.");
            return;
        }

        UnityEngine.Debug.Log($"Unpaired device used: {newDevice.displayName}");

        // Determine which player to replace
        int playerIndexToReplace = 0; // Default to Player 1

        // If there are two players, decide which one to replace based on your criteria
        if (PlayerInput.all.Count == 1)
        {
            UnityEngine.Debug.Log("Replacing input device for player 1");
            ReplacePlayerInput(newDevice, playerIndexToReplace);
        }
        if (PlayerInput.all.Count > 1)
        {
            UnityEngine.Debug.Log("Replacing input device for player " + (playerIndexToReplace + 1));
            if (newDevice != null)
            {
                UnityEngine.Debug.Log("Replacing input device: " + newDevice.displayName);
            }
            else
            {
                UnityEngine.Debug.LogError("New device is null.");
            }
            replaceInputDevice = newDevice;
            inputMenu.OpenMenu();
        }
    }

    private void ReplacePlayerInput(InputDevice newDevice, int playerIndex)
    {
        if (PlayerInput.all.Count > playerIndex)
        {
            PlayerInput player = PlayerInput.all[playerIndex];
            player.user.UnpairDevices();

            // Check if the new device is either the keyboard or mouse
            if (newDevice is Keyboard || newDevice is Mouse)
            {
                // Pair both keyboard and mouse together
                InputUser.PerformPairingWithDevice(Keyboard.current, player.user);
                InputUser.PerformPairingWithDevice(Mouse.current, player.user);

                string controlScheme = player.actions.controlSchemes
                    .FirstOrDefault(scheme => scheme.SupportsDevice(Keyboard.current) && scheme.SupportsDevice(Mouse.current)).name;

                if (!string.IsNullOrEmpty(controlScheme))
                {
                    player.SwitchCurrentControlScheme(controlScheme, Keyboard.current, Mouse.current);
                    UnityEngine.Debug.Log($"Replaced Player {playerIndex + 1} input with Mouse and Keyboard using control scheme '{controlScheme}'");
                }
                else
                {
                    UnityEngine.Debug.LogWarning($"No matching control scheme found for Mouse and Keyboard. Default control scheme will be used.");
                }
            }
            else
            {
                // Handle other devices (like gamepads) as usual
                InputUser.PerformPairingWithDevice(newDevice, player.user);

                string controlScheme = player.actions.controlSchemes
                    .FirstOrDefault(scheme => scheme.SupportsDevice(newDevice)).name;

                if (!string.IsNullOrEmpty(controlScheme))
                {
                    player.SwitchCurrentControlScheme(controlScheme, newDevice);
                    UnityEngine.Debug.Log($"Replaced Player {playerIndex + 1} input with {newDevice.displayName} using control scheme '{controlScheme}'");
                }
                else
                {
                    UnityEngine.Debug.LogWarning($"No matching control scheme found for device {newDevice.displayName}. Default control scheme will be used.");
                }
            }
            setPlayerDependencies(player);
        }
        else
        {
            UnityEngine.Debug.LogWarning($"Player {playerIndex + 1} does not exist.");
        }
    }

    private IEnumerator WaitForUnpairedPlayer(PlayerInputManager playerInputManager, int playerIndex)
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
                    newPlayer.transform.rotation = levelTransform.rotation;


                    setPlayerDependencies(newPlayer);
                    /* setCameras(newPlayer);
                    setUserInterface(newPlayer); */

                    // Second player found, resume the game

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
