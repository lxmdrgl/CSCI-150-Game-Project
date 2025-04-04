using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator animator;
    private bool isOpened = false;
    public GameObject upgradePrefab;
    private SpriteRenderer spriteRenderer; // For fading

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer for fading
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        if (!isOpened)
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        isOpened = true;
        animator.SetTrigger("Open");

        if (upgradePrefab != null)
        {
            // Spawn the upgrade and start its pop-out effect
            GameObject upgrade = Instantiate(upgradePrefab, transform.position, Quaternion.identity);
            StartCoroutine(UpgradePopOut(upgrade));
        }

        // Start fading out the chest after the animation
        StartCoroutine(FadeOutAfterAnimation());
    }

    private IEnumerator FadeOutAfterAnimation()
    {
        // Wait for the animation to finish (plus a small buffer)
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + 0.5f);

        // Fade out over 1 second
        float fadeDuration = 1.5f;
        float elapsedTime = 0f;
        Color startColor = spriteRenderer.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        // Deactivate the chest after fading
        gameObject.SetActive(false);
        spriteRenderer.color = startColor; // Reset alpha for potential reuse
    }

    private IEnumerator UpgradePopOut(GameObject upgrade)
    {
        // Initial scale (start small for punch effect)
        upgrade.transform.localScale = Vector3.zero;
        Vector3 targetScale = Vector3.one; // Normal size
        Vector3 startPos = upgrade.transform.position;
        startPos.x -= 0.5f;
        startPos.y -= 0.5f;
        Vector3 targetPos = startPos;
        targetPos.x -= 1.0f;
        targetPos.y += 1.0f;
        float duration = 1.0f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            
            // Smoothly interpolate position and scale
            upgrade.transform.position = Vector3.Lerp(startPos, targetPos, t);
            upgrade.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);
            yield return null;
        }

        // Ensure final values are exact
        upgrade.transform.position = targetPos;
        upgrade.transform.localScale = targetScale;
    }
}