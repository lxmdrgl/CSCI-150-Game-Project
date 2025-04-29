using System.Collections;
using UnityEngine;

public class SpikeEffect : MonoBehaviour
{
    public float riseHeight = 1f;
    public float riseSpeed = 4f;
    public float fallSpeed = 4f;
    public float fadeSpeed = 2f;

    private SpriteRenderer sr;
    private Vector3 initialPosition;
    private bool rising = true;
    private bool falling = false;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        initialPosition = transform.position;
        Color c = sr.color;
        c.a = 0f;
        sr.color = c;
        StartCoroutine(SpikeRoutine());
    }

    private IEnumerator SpikeRoutine()
    {
        // Fade In
        while (sr.color.a < 1f)
        {
            Color c = sr.color;
            c.a += Time.deltaTime * fadeSpeed;
            sr.color = c;
            yield return null;
        }

        // Rise
        while (transform.position.y < initialPosition.y + riseHeight)
        {
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.1f); // Pause at top

        // Fall
        while (transform.position.y > initialPosition.y)
        {
            transform.position -= Vector3.up * fallSpeed * Time.deltaTime;
            yield return null;
        }

        // Fade Out
        while (sr.color.a > 0f)
        {
            Color c = sr.color;
            c.a -= Time.deltaTime * fadeSpeed;
            sr.color = c;
            yield return null;
        }

        Destroy(gameObject);
    }
}
