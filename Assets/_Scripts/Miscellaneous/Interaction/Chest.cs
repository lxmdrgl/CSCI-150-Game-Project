using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator animator;
    private bool isOpened = false;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isOpened && other.CompareTag("Player"))
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        isOpened = true;
        animator.SetTrigger("Open");

        Transform upgrade = transform.GetChild(0);
        if (upgrade != null)
        {
            upgrade.gameObject.SetActive(true);
        }
        StartCoroutine(DisappearAfterAnimation());
    }

    private IEnumerator DisappearAfterAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length+2.0f);
        gameObject.SetActive(false);
    }
}
