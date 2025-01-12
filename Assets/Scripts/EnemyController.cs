using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IEnemy
{
    private bool isFacingRight = false;
    private BoxCollider2D enemyCollider;
    private Animator animator;
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private float moveRange = 1.0f;
    float startPositionX;
    private bool isMovingRight = true;
    private bool isHit = false;
    [SerializeField] private float killDistance = 1.5f;


    void Awake()
    {
        //rigidBody = GetComponent<Rigidbody2D>();
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
        if (isMovingRight)
        {
            if(this.transform.position.x < startPositionX + moveRange)
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
            if(this.transform.position.x > startPositionX - moveRange)
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

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
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
                enemyCollider.enabled = false;
                animator.SetBool("isDead", true);
                StartCoroutine(KillOnAnimationEnd());
                GameManager.instance.RemoveEnemy();
            }
        }
    }

    public float GetKillDistance()
    {
        return killDistance;
    }
}
