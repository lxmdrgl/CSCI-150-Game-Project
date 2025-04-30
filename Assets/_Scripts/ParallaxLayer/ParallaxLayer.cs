using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor = 1f;
    public float globalParallaxMultiplier = 2.0f; 
    private Transform cam;
    private Vector3 lastCamPos;

    void Start()
    {
        cam = Camera.main.transform;
        lastCamPos = cam.position;
    }

    void Update()
    {
        Vector3 delta = cam.position - lastCamPos;
        transform.position += new Vector3(delta.x * parallaxFactor * globalParallaxMultiplier, 0f, 0f);
        lastCamPos = cam.position;
        transform.position = new Vector3(transform.position.x, cam.transform.position.y, 0f);
    }

}
