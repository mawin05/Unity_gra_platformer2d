using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private float pushingSpeed = 0.1f;
    [SerializeField] private float walkingSpeed = 0.1f;
    [SerializeField] private float jumpForce = 6.0f;
    [SerializeField] private float rayOffset = 0.5f;
    [SerializeField] private float fallGravity = 1.5f;
    [SerializeField] private AudioClip bSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip keySound;
    [SerializeField] private AudioClip heartSound;
    [SerializeField] private AudioClip crushingSound;
    [SerializeField] private AudioClip levelCompleted;
    [SerializeField] private AudioClip ladderClimbing;
    [SerializeField] private AudioClip dashSound;
    private AudioSource source;
    private Rigidbody2D rigidBody;
    private BoxCollider2D playerCollider;
    private TrailRenderer trail;
    public LayerMask groundLayer;
    const float rayLength = 0.5f;
    private Animator animator;
    private bool isWalking = false;
    private bool isFacingRight = true;
    private bool isLadder = false;
    private bool isClimbing = false;
    private bool isJumping = false;
    private bool isBouncing = false;
    private float vertical;
    private Vector2 startPosition;
    private int keysFound = 0;
    private const int keysNumber = 3;
    private int lives = 3;
    bool grounded = true;
    private float jumpCooldown = 0.35f;
    private float jumpStartTime = 0.0f;
    Vector2 leftRayOrigin;
    Vector2 rightRayOrigin;
    bool firstJump = false;
    public ParticleSystem dust;
    private bool isHurt = false;

    public float shakeDuration = 0.3f; // Czas trwania wstrz¹su
    public float shakeMagnitude = 0.15f; // Intensywnoœæ wstrz¹su
    private Transform cam;
    [SerializeField] private CanvasGroup flashCanvasGroup;

    [SerializeField] private Transform groundCheck;  // Punkt do sprawdzania pod³o¿a (np. pod stopami gracza)
    [SerializeField] private float groundCheckRadius = 0.2f;

    [SerializeField] private Transform rightWallCheck;  // Punkt do sprawdzania pod³o¿a (np. pod stopami gracza)
    [SerializeField] private float rightWallCheckRadius = 0.01f;
    private bool isNextToRightWall = false;

    [SerializeField] private Transform leftWallCheck;  // Punkt do sprawdzania pod³o¿a (np. pod stopami gracza)
    private bool isNextToLeftWall = false;

    [Header("Dashing")]
    [SerializeField] private float dashingVelocity;
    [SerializeField] private float dashingTime;
    private bool isDashing = false;
    private Vector2 dashDirection;
    private bool canDash = true;
    private bool isPushing = false;


    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void Jump()
    {
        if (IsGrounded() && !isClimbing)
        {
            CreateDust();
            source.pitch = 1.0f;
            source.PlayOneShot(jumpSound, AudioListener.volume);
            isJumping = true;
            firstJump = true;
            jumpStartTime = Time.time;
            animator.SetBool("isJumping", isJumping);
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }else if (firstJump && !isClimbing)
        {
            CreateDust();
            source.pitch = 1.3f;
            source.PlayOneShot(jumpSound, AudioListener.volume);
            isJumping = true;
            firstJump = false;
            jumpStartTime = Time.time;
            animator.SetBool("isJumping", isJumping);
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0); // resetowanie prêdkoœci w osi y
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void Dash()
    {
        if (!isDashing && canDash)
        {
            animator.SetBool("isDashing", true);
            source.PlayOneShot(dashSound, AudioListener.volume);
            trail.emitting = true;
            canDash = false;
            isDashing = true;
            dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if(dashDirection == Vector2.zero)
            {
                dashDirection = new Vector2(transform.localScale.x, 0);
            }
            rigidBody.gravityScale = 0;
            rigidBody.velocity = dashDirection.normalized * dashingVelocity;
            //rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
            StartCoroutine(StopDashing());
        }
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        rigidBody.gravityScale = 1;
        rigidBody.velocity = new Vector2(0, 0);
        trail.emitting = false;
        animator.SetBool("isDashing", false);
    }


    void Awake()
    {
        startPosition = this.transform.position;
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        trail = GetComponent<TrailRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {

        if(Physics2D.OverlapCircle(rightWallCheck.position, rightWallCheckRadius, groundLayer))
        {
            if (isFacingRight)
            {
                isNextToRightWall = true;
            }
            else
            {
                isNextToLeftWall = true;
            }
            StopDashing();
        }
        else
        {

            isNextToRightWall = false;
            isNextToLeftWall = false;
        }


        isWalking = false;
        if (GameManager.instance.currentGamestate == GameState.GAME)
        {
            if (!isHurt)
            {
                if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && !isDashing && !isNextToRightWall) //prawo
                {
                    transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);

                    isWalking = true;
                    if (!isFacingRight)
                    {
                        Flip();
                    }
                }

                if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && !isDashing && !isNextToLeftWall) //lewo
                {
                    transform.Translate(-1 * moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                    isWalking = true;
                    if (isFacingRight)
                    {
                        Flip();
                    }
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                }

                if (Input.GetKeyUp(KeyCode.Space) && !isBouncing)
                {
                    rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y / 2);
                    isJumping = false;
                    animator.SetBool("isJumping", false);
                }

                if (Input.GetKeyDown(KeyCode.V))
                {
                    Dash();
                }
            }
        }

        //Debug.DrawRay(transform.position, rayLength * Vector3.down, Color.white, 1, false);

        Debug.DrawRay(leftRayOrigin, Vector2.down * rayLength, Color.red);
        Debug.DrawRay(rightRayOrigin, Vector2.down * rayLength, Color.red);

        grounded = IsGrounded();

        if (grounded && !isDashing)
        {
            canDash = true;
        }

        animator.SetBool("isGrounded", grounded);

        if (isJumping && (Time.time > jumpStartTime + jumpCooldown))
        {
            isJumping = false;
            isBouncing = false;
            animator.SetBool("isJumping", false);
        }

        if((Time.time > jumpStartTime + 0.1f) && !isLadder)
        {
            source.pitch = 1.0f;
        }

        animator.SetBool("isWalking", isWalking);

        vertical = Input.GetAxis("Vertical");

        if(isLadder && vertical > 0 && !isClimbing)
        {
            isClimbing = true;
            source.clip = ladderClimbing;
            source.loop = true;
            source.pitch = 1.5f;
            source.Play();
        }

        animator.SetBool("isClimbing", isClimbing);

        if (Input.GetAxis("Horizontal") != 0 && isPushing)
        {
            animator.SetBool("isPushing", true);
        }
        else
        {
            animator.SetBool("isPushing", false);
        }

    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            rigidBody.gravityScale = 0;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, vertical * moveSpeed);
        }else if (isDashing)
        {
            rigidBody.gravityScale = 0;
        }
        else
        {
            rigidBody.gravityScale = 3;
        }

        if(grounded==false && isJumping == false)
        {
            rigidBody.gravityScale += fallGravity;
        }
        else
        {
            rigidBody.gravityScale = 3;
        }
        
    }

    private IEnumerator PlayHurtAnimation()
    {
        rigidBody.velocity = Vector2.zero; 
        rigidBody.isKinematic = true; 
        playerCollider.enabled = false;

        animator.SetBool("isDead", true); 
        isHurt = true;

        yield return new WaitForSeconds(1f);

        animator.SetBool("isDead", false); 

        this.transform.position = startPosition;

        rigidBody.isKinematic = false; 
        playerCollider.enabled = true; 
        isHurt = false;


        lives--;

        if (lives == 0)
        {
            this.gameObject.SetActive(false);
        }
    }



    void OnTriggerEnter2D(Collider2D cal)
    {
        if (cal.CompareTag("FallLevel"))
        {
            GameManager.instance.RemoveLife();
            transform.position = startPosition;
            lives--;
            if(lives == 0)
            {
                this.gameObject.SetActive(false); 
            }
        }

        if (cal.CompareTag("Bonus"))
        {
            GameManager.instance.AddPoints(10);
            source.PlayOneShot(bSound, AudioListener.volume);
        }

        if (cal.CompareTag("Ladder"))
        {
            isLadder = true;
        }

        if (cal.CompareTag("Spike"))
        {
            ShakeCamera();
            source.PlayOneShot(hurtSound, AudioListener.volume);
            GameManager.instance.RemoveLife();
            StartCoroutine(PlayHurtAnimation());
        }

        if(cal.CompareTag("Enemy"))
        {
            IEnemy enemy = cal.GetComponent<IEnemy>();
            float difference = enemy.GetKillDistance();

            if (this.transform.position.y - difference > cal.gameObject.transform.position.y) // player over the enemy
            {

                isJumping = true;
                isBouncing = true;
                jumpStartTime = Time.time;
                animator.SetBool("isJumping", isJumping);
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0); // resetowanie prêdkoœci w osi y

                rigidBody.AddForce(Vector2.up * jumpForce * 0.7f, ForceMode2D.Impulse);
                firstJump = true;
                canDash = true;
                source.PlayOneShot(crushingSound, AudioListener.volume);
            }
            else // player below te enemy
            {
                ShakeCamera();
                source.PlayOneShot(hurtSound, AudioListener.volume);
                GameManager.instance.RemoveLife();
                StartCoroutine(PlayHurtAnimation());
            }
        }

        if (cal.CompareTag("Key"))
        {
            GameManager.instance.AddKey();
            source.PlayOneShot(keySound, AudioListener.volume);
            keysFound++;
        }

        if (cal.CompareTag("Heart"))
        {
            /*  GameManager.instance.AddLife();
              source.PlayOneShot(heartSound, AudioListener.volume);
              if(lives<3)
                  lives++;*/
            if (lives != 3)
            {
                //GameManager.instance.AddLife();
                source.PlayOneShot(heartSound, AudioListener.volume);
                lives++;
            }
        }

        if (cal.CompareTag("Finish"))
        {
            if (keysFound == 3)
            {
                GameManager.instance.AddPoints(30 * lives);
                source.PlayOneShot(levelCompleted, AudioListener.volume);
                GameManager.instance.LevelCompleted();
            }
            else
            {
                string endlog = "Brakuje ci ";
                endlog += (char)(keysNumber - keysFound + 48);
                endlog += " kluczy";
                Debug.Log(endlog);
            }
        }

        if (cal.CompareTag("movingPlatform"))
        {
            this.transform.SetParent(cal.transform);
        }

        if (cal.CompareTag("rotatingPlatform"))
        {
            this.transform.SetParent(cal.transform);
        }

        if (cal.CompareTag("CraneSide"))
        {
            moveSpeed = walkingSpeed;
            Transform parentTransform = cal.transform.parent;
            Rigidbody2D parentRigidbody = parentTransform.GetComponent<Rigidbody2D>();
            parentRigidbody.bodyType = RigidbodyType2D.Dynamic;
            moveSpeed = pushingSpeed;
        }

        if (cal.CompareTag("Crate"))
        {
            StopDashing();
            canDash = false;
            isPushing = true;
        }

        if (cal.CompareTag("Bullet"))
        {
            ShakeCamera();
            source.PlayOneShot(hurtSound, AudioListener.volume);
            GameManager.instance.RemoveLife();
            StartCoroutine(PlayHurtAnimation());
        }

    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Ladder"))
        {
            isLadder = false;
            if (isClimbing)
            {
                isClimbing = false;
                source.loop = false;
                source.Stop();
                source.pitch = 1.0f;
            }
        }

        if (col.CompareTag("movingPlatform"))
        {
            this.transform.SetParent(null);
        }

        if (col.CompareTag("movingPlatform"))
        {
            this.transform.SetParent(null);
        }

        if (col.CompareTag("CraneSide"))
        {
            moveSpeed = walkingSpeed;
            Transform parentTransform = col.transform.parent;
            Rigidbody2D parentRigidbody = parentTransform.GetComponent<Rigidbody2D>();
            parentRigidbody.bodyType = RigidbodyType2D.Static;
        }

        if (col.CompareTag("Crate"))
        {
            canDash = true;
            isPushing = false;
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void CreateDust()
    {
        dust.Play();
    }

    private void ShakeCamera()
    {
        StartCoroutine(FlashScreen());
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        Debug.Log("Shake");
        float elapsed = 0;

        Vector3 ogPosition = cam.position;

        while (elapsed < shakeDuration)
        {
            // Generowanie losowego przesuniêcia pozycji kamery
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

            // Ustawienie nowej pozycji kamery w oparciu o jej bie¿¹c¹ pozycjê
            cam.position = new Vector3(ogPosition.x + offsetX, ogPosition.y + offsetY, ogPosition.z);

            // Zwiêkszanie up³ywu czasu
            elapsed += Time.unscaledDeltaTime;

            yield return null; // Poczekaj na nastêpny frame
        }

        cam.position = ogPosition;

    }

    private IEnumerator FlashScreen()
    {
        float flashDuration = 0.15f; // Czas trwania ca³ego rozb³ysku
        float maxAlpha = 0.5f;        // Maksymalna przezroczystoœæ
        float elapsed = 0f;

        // W³¹cz bia³y ekran
        flashCanvasGroup.gameObject.SetActive(true);

        // Faza rozb³ysku (zwiêkszanie przezroczystoœci)
        while (elapsed < flashDuration / 2f)
        {
            elapsed += Time.unscaledDeltaTime; // Niezale¿ne od Time.timeScale
            float alpha = Mathf.Lerp(0, maxAlpha, elapsed / (flashDuration / 2f));
            flashCanvasGroup.alpha = alpha;
            yield return null;
        }

        // Faza zanikania (zmniejszanie przezroczystoœci)
        elapsed = 0f;
        while (elapsed < flashDuration / 2f)
        {
            elapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(maxAlpha, 0, elapsed / (flashDuration / 2f));
            flashCanvasGroup.alpha = alpha;
            yield return null;
        }

        // Wy³¹cz bia³y ekran
        flashCanvasGroup.gameObject.SetActive(false);
    }

    public int getLives()
    {
        return lives;
    }

}
