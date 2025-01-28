using Unity.Cinemachine;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject startingRoom;
    public GameObject player;
    public float playerOffset = 1;
    public CinemachineCamera cinemachineCamera;

    void Awake()
    {
        spawnStartingRoom();
        spawnPlayer();
    }

    void Start()
    {
        
    }
    void Update()
    {
        
    }

    void spawnStartingRoom() {
        // find origin location for level generator
        GameObject levelOrigin = GameObject.Find("LevelOrigin");
        Transform levelTransform;

        // create starting room and find room origin location

        GameObject newRoom = Instantiate(startingRoom, new Vector3(0, 0, 0), transform.rotation);        
        GameObject roomOrigin = GameObject.Find("RoomOrigin");
        Transform roomTransform;

        if (levelOrigin != null && roomOrigin != null) {
            levelTransform = levelOrigin.transform;
            roomTransform = roomOrigin.transform;

            // change room location to difference in origins -> origins overlap
            newRoom.transform.position += levelTransform.position - roomTransform.position;
        } else {
            Debug.Log("Level or Room Origin not found");
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
            Debug.Log("Level Origin not found");
        }
    }
}
