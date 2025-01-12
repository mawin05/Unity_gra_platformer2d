using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpossumControler : MonoBehaviour, IEnemy
{
    private bool isFacingRight = false;
    private Rigidbody2D rigidBody;
    private BoxCollider2D enemyCollider;
    private Animator animator;
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private float moveRange = 1.0f;
    float startPositionX;
    private bool isMovingRight = true;
    private bool isHit = false;
    public LayerMask groundLayer;
    [SerializeField] private float killDistance = 0.5f;

    [Header("Falling")]
    [SerializeField] private Transform groundCheck;  // Punkt do sprawdzania pod³o¿a (np. pod stopami gracza)
    [SerializeField] private float groundCheckRadius = 0.2f;


    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<BoxCollider2D>();
        startPositionX = this.transform.position.x;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGrounded())
        {
            rigidBody.gravityScale = 0;
            rigidBody.velocity = Vector2.zero;
        }
        else
        {
            rigidBody.gravityScale = 3;
        }

        if (isMovingRight)
        {
            if (this.transform.position.x < startPositionX + moveRange)
            {
                moveRight();
            }
            else
            {
                isMovingRight = false;
                moveLeft();
            }
        }
        else
        {
            if (this.transform.position.x > startPositionX - moveRange)
            {
                moveLeft();
            }
            else
            {
                isMovingRight = true;
                moveRight();
            }
        }
    }

    void moveRight()
    {
        if (!isHit) //prawo
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            if (!isFacingRight)
            {
                Flip();
            }
        }
    }

    void moveLeft()
    {
        if (!isHit) //lewo
        {
            transform.Translate(-1 * moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            if (isFacingRight)
            {
                Flip();
            }
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    IEnumerator KillOnAnimationEnd()
    {
        //isHit = true;
        //yield return new WaitForSeconds(0.5f);
        //this.gameObject.SetActive(false);
        isHit = true;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer == null)
        {
            Debug.LogError($"SpriteRenderer is missing on {gameObject.name}!");
            yield break; // Przerywa dzia³anie funkcji, jeœli renderer nie istnieje.
        }
        Color startColor = renderer.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        float elapsedTime = 0.0f;
        float fadeDuration = 0.5f;

        while (elapsedTime < fadeDuration)
        {
            renderer.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D cal)
    {
        if (cal.CompareTag("Player"))
        {
            if (this.transform.position.y + killDistance < cal.gameObject.transform.position.y)
            {
                Debug.Log("collision");
                enemyCollider.enabled = false;
                animator.SetBool("isDead", true);
                StartCoroutine(KillOnAnimationEnd());
                GameManager.instance.RemoveEnemy();
            }
        }
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    public float GetKillDistance()
    {
        return killDistance;
    }
}
