using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting.Dependencies.NCalc;
// using UnityEditorInternal;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject player;
    public float playerOffset = 2;
    public CinemachineCamera cinemachineCamera;


    public RoomManager startRoom;
    public List<RoomManager> hallways;
    public List<RoomManager> rooms;
    public RoomManager endRoom;


    public List<GameObject> roomObjQueue = new List<GameObject>();

    void Awake()
    {
         DontDestroyOnLoad(gameObject);

        spawnStartingRoom();

        if (generateLevel_1()) { 
            spawnPlayer();
        } else {
            Debug.LogError("Level generation error");
        }
    }

    bool generateLevel_1() {
        // startRoom -> hallway -> hallway -> endRoom
        GameObject currRoom;
        RoomManager currRoomMan;
        GameObject newRoom;
        RoomManager newRoomMan;
        int numHallways = Random.Range(3, 5);
        int i = 0;

        // Instantiate multiple hallways
        for (i = 0; i < numHallways; i++) {
            currRoom = roomObjQueue[i];
            currRoomMan = currRoom.GetComponent<RoomManager>();

            newRoom = Instantiate(hallways[Random.Range(0, hallways.Count)].gameObj, new Vector3(0, 0, 0), transform.rotation);
            newRoomMan = newRoom.GetComponent<RoomManager>();
            roomObjQueue.Add(newRoom);

            newRoom.transform.position += currRoomMan.exits[0].transform.position - newRoomMan.entrances[0].transform.position;
        }

        // Instantiate end room
        currRoom = roomObjQueue[i];
        currRoomMan = currRoom.GetComponent<RoomManager>();

        newRoom = Instantiate(endRoom.gameObj, new Vector3(0, 0, 0), transform.rotation);
        newRoomMan = newRoom.GetComponent<RoomManager>();
        roomObjQueue.Add(newRoom);

        newRoom.transform.position += currRoomMan.exits[0].transform.position - newRoomMan.entrances[0].transform.position;

        return true;
    }

    void spawnStartingRoom() {
        // find origin location for level generator
        GameObject levelOrigin = GameObject.Find("LevelOrigin");
        Transform levelTransform;

        // create starting room and find room origin location
        GameObject newRoom = Instantiate(startRoom.gameObj, new Vector3(0, 0, 0), transform.rotation);   
        roomObjQueue.Add(newRoom);

        GameObject roomOrigin = GameObject.Find("RoomOrigin");
        Transform roomTransform;

        if (levelOrigin != null && roomOrigin != null) {
            levelTransform = levelOrigin.transform;
            roomTransform = roomOrigin.transform;

            // change room location to difference in origins -> origins overlap
            newRoom.transform.position += levelTransform.position - roomTransform.position;
        } else {
            Debug.LogError("Level or Room Origin not found");
        }
    }

    void spawnPlayer() {
        GameObject levelOrigin = GameObject.Find("LevelOrigin");
        Transform levelTransform;

        if (levelOrigin != null) {
            levelTransform = levelOrigin.transform;
            player = Instantiate(player, new Vector3(levelTransform.position.x, levelTransform.position.y + playerOffset, 0), levelTransform.rotation);
            cinemachineCamera.Target.TrackingTarget = player.transform;
        } else {
            Debug.LogError("Level Origin not found");
        }
    }
}
