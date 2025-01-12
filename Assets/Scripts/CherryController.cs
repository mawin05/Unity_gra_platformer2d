using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    private Animator animator;
    private CircleCollider2D collider;

    void Awake()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<CircleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D cal)
    {
        if (cal.CompareTag("Player"))
        {
            collider.enabled = false;
            animator.SetBool("wasPicked", true);
            StartCoroutine(DisableOnAnimationEnd());
        }
    }

    IEnumerator DisableOnAnimationEnd()
    {
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }
}
