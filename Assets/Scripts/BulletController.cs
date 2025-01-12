using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rigidBody;
    private CapsuleCollider2D colider;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        colider = GetComponent<CapsuleCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Cannon") && !col.CompareTag("Heart") && !col.CompareTag("Key") && !col.CompareTag("Bonus"))
        {
            anim.SetBool("wasHit", true);
            colider.enabled = false;
            rigidBody.velocity = new Vector2(0, 0); // resetowanie prêdkoœci w osi y
            StartCoroutine(DisableAfterDelay(0.5f)); // Rozpocznij korutynê z opóŸnieniem 0.5 sekundy
        }
    }

    private IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Odczekaj podany czas
        this.gameObject.SetActive(false);      // Wy³¹cz obiekt
        Destroy(this.gameObject);
    }
}
