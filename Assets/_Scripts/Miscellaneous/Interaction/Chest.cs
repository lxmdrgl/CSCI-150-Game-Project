using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator animator;
    private bool isOpened = false;
    public GameObject upgradePrefab;
    void Start()
    {
        animator = GetComponent<Animator>();
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
            Instantiate(upgradePrefab, transform.position, Quaternion.identity);
        }
        StartCoroutine(DisappearAfterAnimation());
    }

    private IEnumerator DisappearAfterAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length+2.0f);
        gameObject.SetActive(false);
    }
}
