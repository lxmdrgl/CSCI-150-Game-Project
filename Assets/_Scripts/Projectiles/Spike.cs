using System.Collections;
using UnityEngine;
using Game.Combat.Damage;
using Game.Utilities;

public class SpikeEffect : MonoBehaviour
{
    public float damage = 10f;
    public float riseHeight = 1f;
    public float riseSpeed = 4f;
    public float fadeSpeed = 2f;
    private SpriteRenderer sr;
    private Vector3 initialPosition;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        initialPosition = transform.position;        

        // Start fully transparent
        Color c = sr.color;
        c.a = 0f;
        sr.color = c;

        StartCoroutine(SpikeRoutine());
    }

    private IEnumerator SpikeRoutine()
    {
        // Fade In while rising
        while (transform.position.y < initialPosition.y + riseHeight)
        {
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;

            Color c = sr.color;
            c.a = Mathf.Min(1f, c.a + Time.deltaTime * fadeSpeed);
            sr.color = c;

            yield return null;
        }

        // Optional: brief pause at the top
        yield return new WaitForSeconds(0.1f);

        // Fade Out at peak
        while (sr.color.a > 0f)
        {
            Color c = sr.color;
            c.a -= Time.deltaTime * fadeSpeed;
            sr.color = c;

            yield return null;
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        Collider2D[] colliders = new Collider2D[1] { collision };
        if(collision.CompareTag("Player"))
        {
            //CombatDamageUtilities.TryDamage(colliders, new DamageData(damage, gameObject), out _);
        }
    }
}
