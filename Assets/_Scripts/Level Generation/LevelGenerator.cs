using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.Cinemachine;
using Unity.VisualScripting.Dependencies.NCalc;
// using UnityEditorInternal;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject player;
    public float playerOffset = 2;
    public CinemachineCamera cinemachineCamera;
    
    public RoomNode roomMap;
    private int playerCount;

    void Awake()
    {
        playerCount = PlayerPrefs.GetInt("playerCount");

        DontDestroyOnLoad(gameObject);

        spawnRoomMap();

        if (spawnStartingRoom() && spawnAllRooms(roomMap)) { 
            if(playerCount == 1)
            {
                spawnPlayer();
            }
            else
            {
                // 2 player
                UnityEngine.Debug.Log("2 PLAYERS");
            }

        } else {
            UnityEngine.Debug.LogError("Level generation error");
        }
    }

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

    bool spawnAllRooms(RoomNode root) {
        RoomNode currNode = root; 
        RoomNode newNode; 
        int randIndex = UnityEngine.Random.Range(0,2);

        while(currNode.children.Count != 0) { // not reached dead end
            if (currNode.children.Count == 1) 
            {
                newNode = currNode.children[0];
                spawnRoom(newNode);
                connectRooms(currNode, 0, newNode, 0);
                currNode = newNode;
            } else if (currNode.children.Count == 2)
            {
                spawnRoom(currNode.children[0]);
                spawnRoom(currNode.children[1]);

                connectRooms(currNode, randIndex, currNode.children[0], 0);
                connectRooms(currNode, Math.Abs(randIndex-1), currNode.children[1], 0);



                spawnAllRooms(currNode.children[0]);
                spawnAllRooms(currNode.children[1]);
                break;
            } else 
            {
                UnityEngine.Debug.LogError(gameObject.name + ": Unexpected number of exits");
                return false;
            }
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
            }
        }
        return r.roomObject;
    }
    
    bool connectRooms(RoomNode currNode, int exitIdx, RoomNode newNode, int entranceIdx) {
        // UnityEngine.Debug.Log(currNode.gameObject.name + ": " + exitIdx + ", " + entranceIdx);

        RoomManager currNodeManager = currNode.roomObject.GetComponent<RoomManager>();
        RoomManager newNodeManager = newNode.roomObject.GetComponent<RoomManager>();

        newNode.roomObject.transform.position += currNodeManager.exits[exitIdx].transform.position - newNodeManager.entrances[entranceIdx].transform.position;
        
        List<Collider2D> results = new List<Collider2D>();
        // ContactFilter2D contactFilter = new ContactFilter2D();
        // contactFilter.useTriggers = false; // Ignore triggers if necessary
        // contactFilter.SetLayerMask(LayerMask.GetMask("Room")); // Ensure the objects are on the "Room" layer
        int overlaps = 0;

        foreach (Collider2D collider in newNodeManager.colliders) {
            overlaps += Physics2D.OverlapCollider(collider, results);

            foreach (Collider2D overlappingCollider in results) {
                string colliderParentName = collider.transform.parent != null ? collider.transform.parent.name : collider.gameObject.name;
                string overlappingParentName = overlappingCollider.transform.parent != null ? overlappingCollider.transform.parent.name : overlappingCollider.gameObject.name;
                
                UnityEngine.Debug.Log($"Overlap detected: {colliderParentName} is overlapping with {overlappingParentName}");
            }
            results.Clear();
        }

        if (overlaps != 0) {
            UnityEngine.Debug.LogError("collision overlap " + newNode.gameObject.name + ": " + overlaps);
        }
        
        return true;
    }

    void spawnPlayer() {
        GameObject levelOrigin = GameObject.Find("LevelOrigin");
        Transform levelTransform;

        if (levelOrigin != null) {
            levelTransform = levelOrigin.transform;
            player = Instantiate(player, new Vector3(levelTransform.position.x, levelTransform.position.y + playerOffset, 0), levelTransform.rotation);
            
            
            cinemachineCamera.Target.TrackingTarget = player.transform;
        } else {
            UnityEngine.Debug.LogError("Level Origin not found");
        }
    }

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
