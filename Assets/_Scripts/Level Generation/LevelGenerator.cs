using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

using Debug=UnityEngine.Debug;
using Random=UnityEngine.Random;

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
        Debug.Log("Subscribing to InputUser.onUnpairedDeviceUsed");
        InputSystem.onDeviceChange += OnDeviceChange;
        inputMenu.OnButtonClickedEvent += index => ReplacePlayerInput(replaceInputDevice, index);
        InputUser.onUnpairedDeviceUsed += OnUnpairedDeviceUsed;
    }

    async void Start()
    {
        InputUser.listenForUnpairedDeviceActivity = 1;
        InputUser.onUnpairedDeviceUsed += OnUnpairedDeviceUsed;
        Debug.Log("Starting level gen");
        await InitializeLevel();
    }

    private async Task InitializeLevel()
    {
        playerCount = PlayerPrefs.GetInt("playerCount");
        playerInputManager = FindFirstObjectByType<PlayerInputManager>();

        // PlayerPrefs.SetInt("player1Kills", PlayerPrefs.GetInt("player1Kills") + 1);

        spawnRoomMap();

        if (spawnStartingRoom() && await spawnAllRooms(roomMap)) 
        {

            spawnAllEnemies(roomMap);

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
            // PlayerInput.all[0].user.UnpairDevices();
            UnityEngine.Debug.Log("Unpairing player 1 input devices: " + PlayerInput.all[0].user.pairedDevices.Count);
        } 
        else 
        {
            UnityEngine.Debug.LogError("Level generation error");
        }
    }

    /* async Task Awake()
    {
        playerCount = PlayerPrefs.GetInt("playerCount");
        playerInputManager = FindFirstObjectByType<PlayerInputManager>();

        // InputSystem.onDeviceChange += OnDeviceChange;
        // inputMenu.OnButtonClickedEvent += index => ReplacePlayerInput(replaceInputDevice, index);
        // InputUser.onUnpairedDeviceUsed += OnUnpairedDeviceUsed;

        // PlayerPrefs.SetInt("player1Kills", PlayerPrefs.GetInt("player1Kills") + 1);

        spawnRoomMap();

        if (spawnStartingRoom() && await spawnAllRooms(roomMap)) 
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
                spawnPlayer(1);
                UnityEngine.Debug.Log("2 PLAYERS");
            }
            else    
            {
                spawnPlayer(1);
                UnityEngine.Debug.Log("PLAYERCOUNT NOT SET");
            }
            PlayerInput.all[0].user.UnpairDevices();
            UnityEngine.Debug.Log("Unpairing player 1 input devices: " + PlayerInput.all[0].user.pairedDevices.Count);

        } 
        else 
        {
            UnityEngine.Debug.LogError("Level generation error");
        }
    } */

    /* void OnEnable()
    {
        Debug.Log("Subscribing to InputUser.onUnpairedDeviceUsed");
        InputSystem.onDeviceChange += OnDeviceChange;
        inputMenu.OnButtonClickedEvent += index => ReplacePlayerInput(replaceInputDevice, index);
        InputUser.onUnpairedDeviceUsed += OnUnpairedDeviceUsed;
    } */

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

        // load random starting room
        List<RoomManager> roomList = loadRoomList(roomMap.roomType);
        RoomManager randRoom = roomList[UnityEngine.Random.Range(0, roomList.Count)];

        // create starting room and find room origin location
        GameObject newRoom = spawnRoom(roomMap, randRoom);
        GameObject roomOrigin = GameObject.Find("RoomOrigin");
        Transform roomTransform;

        if (levelOrigin != null && roomOrigin != null && newRoom != null) {
            levelTransform = levelOrigin.transform;
            roomTransform = roomOrigin.transform;

            // change room location to difference in origins -> origins overlap
            newRoom.transform.position += levelTransform.position - roomTransform.position;
        } else {
            UnityEngine.Debug.LogError("SpawnStartingRoom Error");
            return false;
        }

        return true;
    }

    async Task<bool> spawnAllRooms(RoomNode root) { // supports rooms with one entrance, 0-2 exits
        RoomNode currNode = root; 
        int randIndex = UnityEngine.Random.Range(0,2);

        if (currNode.children.Count == 0) 
        {
            return true;
        } 
        else if (currNode.children.Count == 1) 
        {
            // try next room, if fails (all rooms have collision error)
            if (!await TryNextRoom(currNode, 0, 0)) 
            {
                if (root.parent == null) // end case reached start room
                {
                    UnityEngine.Debug.LogError("No permutation of map possible for: " + roomMap.name);
                    return false;
                } 
                else // retry parent
                {
                    UnityEngine.Debug.Log("Retrying parent, deleting " + root.name);
                    DestroyPath(root);
                    // DestroyImmediate(.roomObject);
                    // roomNumber--;
                    await spawnAllRooms(root.parent);
                }
            } 
            else 
            {
                await spawnAllRooms(currNode.children[0]);
            }
        } 
        else if (currNode.children.Count == 2) 
        {
            
            // fork case
            // if either path fails, swap paths and try again

            int exitIndex = Random.Range(0,2);
            int numTrials = 1;
            int maxTrials = 4;

            while (numTrials <= maxTrials) { 
                if (await TryNextRoom(currNode, 0, exitIndex) && 
                    await TryNextRoom(currNode, 1, 1 - exitIndex)) // success
                { 
                    break;
                }
                else // swap exits and try again
                {
                    exitIndex = 1 - exitIndex;
                    numTrials++;
                    UnityEngine.Debug.LogError("Destroying children of fork and flipping paths: " + roomMap.name);
                    DestroyPath(currNode.children[0]);
                    DestroyPath(currNode.children[1]);
                }
            }
                
            if (numTrials > maxTrials) { // fork is a failure need to retry parent
                root.listTriedRooms.Clear(); // chatgpt logical error

                if (root.parent == null) // end case reached start room
                {
                    UnityEngine.Debug.LogError("No permutation of map possible for: " + roomMap.name);
                    return false;
                } 
                else // retry parent
                {
                    UnityEngine.Debug.LogError("Retrying fork, deleting " + root.name);   
                    DestroyPath(root);

                    return await spawnAllRooms(root.parent);
                }
            }
            
            await spawnAllRooms(currNode.children[0]);
            await spawnAllRooms(currNode.children[1]);
        } 
        else 
        {
            UnityEngine.Debug.LogError(gameObject.name + ": Unexpected number of exits");
            return false;
        }
        return true;
    }

    async Task<bool> TryNextRoom(RoomNode currNode, int childIndex, int exitIndex)
    {
        RoomNode nextNode = currNode.children[childIndex];
        List<RoomManager> roomList = loadRoomList(nextNode.roomType);

        // exclude previously tried rooms if necessary
        roomList = roomList.Except(nextNode.listTriedRooms).ToList<RoomManager>();

        while (true)
        {
            String s = "";
            foreach (RoomManager room in roomList) {
                s += room.name + "|";
            }
            UnityEngine.Debug.Log("Possible rooms for " + nextNode.transform.name + ": " + s);

            // No valid room found
            if (roomList.Count == 0)
            {
                // clear next's triedRoomList
                // retry from parent while not repeating previously tried rooms
                nextNode.listTriedRooms.Clear();

                UnityEngine.Debug.Log("Clear tried of " + nextNode.transform.name + ", No valid rooms available");
                return false;
            }

            // Random query without replacement
            int randIndex = UnityEngine.Random.Range(0, roomList.Count);
            RoomManager randRoom = roomList[randIndex];
            roomList.RemoveAt(randIndex);

            // add selected room to next's listTriedRooms
            nextNode.listTriedRooms.Add(randRoom);

            // Spawn the room
            GameObject spawnObj = spawnRoom(nextNode, randRoom);

            UnityEngine.Debug.Log("Spawned and trying to connect: " + nextNode.name);

            // Ensure ConnectRooms fully completes before continuing
            bool isValid = await ConnectRooms(currNode, exitIndex, nextNode, 0);

            if (isValid)
            {
                UnityEngine.Debug.Log("Valid room: " + nextNode.name);
                return true; // Exit loop on first valid room
            }
            else
            {
                UnityEngine.Debug.Log("Invalid room, deleting: " + nextNode.name);
                DestroyImmediate(spawnObj);
                roomNumber -= 1; // Adjust room count
                // UnityEngine.Debug.Log("Retrying...");
            }

            // Small delay to avoid instant looping issues
            await Task.Yield();
        }
    }

    void DestroyPath(RoomNode root) // deletes all room objects in the subtree of root
    {
        if (root == null) 
        {
            return;
        } 
        
        // Recursively delete child rooms first
        foreach(RoomNode childNode in root.children) {
            DestroyPath(childNode);
        }

        // Decrease room count when deleting a room
        if (root.roomObject != null) 
        {
            UnityEngine.Debug.LogError("Deleting: " + root.name);
            DestroyImmediate(root.roomObject);
            roomNumber--;
            DeleteFreeRooms();
        }
    }

    void DeleteFreeRooms() {
        List<GameObject> roomManagers = new List<GameObject>();

        foreach (RoomManager room in FindObjectsOfType<RoomManager>()) {
            if (!IsFreeRoom(room, roomMap)) {
                roomManagers.Add(room.gameObject);
            }
        }

        // Delete all rooms that are not part of the tree
        foreach (GameObject room in roomManagers) {
            // UnityEngine.Debug.LogError("Found free room: " + room.name);
            UnityEngine.Debug.LogError("Deleting free room: " + room.name);
            Destroy(room);
        }
    }


    bool IsFreeRoom(RoomManager room, RoomNode root) {
        if (root.roomObject == room.gameObj) {
            return true;
        } 

        foreach (RoomNode childNode in root.children) {
            if (IsFreeRoom(room, childNode)) {
                return true;
            }
        }

        return false; // Room is not part of the hierarchy
    }


    GameObject spawnRoom(RoomNode r, RoomManager randRoom) {
        r.roomObject = Instantiate(randRoom.gameObj, new Vector3(0, 0, 0), transform.rotation); 
        
        r.roomObject.name = "Room_" + roomNumber++;
        r.roomObject.GetComponent<RoomManager>().roomCollider.enabled = true;
        
        return r.roomObject;
    }
    
    List<RoomManager> loadRoomList(String roomType) {
        List<RoomManager> roomList = new List<RoomManager>();
        
        roomList = Resources.LoadAll<RoomManager>("Rooms/" + roomType).ToList();
        if (roomList.Count == 0) {
            UnityEngine.Debug.LogError("Failed to load files from Resources/Rooms/" + roomType);
        }

        return roomList;
    }

    async Task<bool> ConnectRooms(RoomNode currNode, int exitIdx, RoomNode newNode, int entranceIdx)
    {

        
        RoomManager currNodeManager = currNode.roomObject.GetComponent<RoomManager>();
        RoomManager newNodeManager = newNode.roomObject.GetComponent<RoomManager>();

        if (currNodeManager.exits.Count() == 0) {
            UnityEngine.Debug.LogError("End room found prematurely in: " + roomMap.name);
            return false;
        }

        newNode.roomObject.transform.position += currNodeManager.exits[exitIdx].transform.position 
                                                 - newNodeManager.entrances[entranceIdx].transform.position;

        newNodeManager.hasCollision = false;
        await DelayedCollisionCheck(newNode, newNodeManager); // Await coroutine

        return !newNodeManager.hasCollision;
    }

    async Task<bool> DelayedCollisionCheck(RoomNode newNode, RoomManager newNodeManager)
    {
        var tcs = new TaskCompletionSource<bool>();

        IEnumerator coroutine = DelayedCollisionCoroutine(newNode, newNodeManager, tcs);
        if (newNodeManager != null && newNodeManager.gameObject.activeInHierarchy)
        {
            newNodeManager.StartCoroutine(coroutine); // Ensure it runs on an active GameObject
        }
        else
        {
            UnityEngine.Debug.LogError("Failed to start coroutine: GameObject is inactive");
            return false; // Fail early if object is disabled
        }

        try
        {
            return await tcs.Task; // Await coroutine completion
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Exception in DelayedCollisionCheck: " + e.Message);
            return false;
        }
    }

    IEnumerator DelayedCollisionCoroutine(RoomNode newNode, RoomManager newNodeManager, TaskCompletionSource<bool> tcs)
    {
        yield return new WaitForFixedUpdate(); // Wait for physics update

        newNodeManager.roomCollider.enabled = true;

        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask("Room"));
        filter.useTriggers = false;

        Collider2D[] results = new Collider2D[10];
        int collisionCount = newNodeManager.roomCollider.Overlap(filter, results);

        if (collisionCount > 0)
        {
            newNodeManager.hasCollision = true;
            string s = "";
            foreach (Collider2D collider in results)
            {
                if (collider != null)
                {
                    s += collider.transform.parent.name + " | ";
                }
            }
            // UnityEngine.Debug.Log(newNode.roomObject.name + " has collisions with: " + s);
        }

        tcs.SetResult(true); // Mark the coroutine as complete
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
        UnityEngine.Debug.Log("OnUnpairedDeviceUsed");
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
}
