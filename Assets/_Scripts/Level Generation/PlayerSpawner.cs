using Unity.Cinemachine;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject player;

    public float y_offset = 1;

    [SerializeField] private CinemachineCamera cinemachineCamera;

    void Awake()
    {
        player = Instantiate(player, new Vector3(transform.position.x, y_offset, 0), transform.rotation);
        cinemachineCamera.Target.TrackingTarget = player.transform;
    }
    
    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
