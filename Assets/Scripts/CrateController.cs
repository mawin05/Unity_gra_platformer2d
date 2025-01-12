using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateController : MonoBehaviour
{

    private Rigidbody2D rigidbody;
    private bool isBeingMoved = false;

    [Header("Grounded")]
    [SerializeField] private Transform groundCheck;  // Punkt do sprawdzania pod³o¿a (np. pod stopami gracza)
    [SerializeField] private float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isStatic = true;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isBeingMoved && IsGrounded())
        {
            rigidbody.bodyType = RigidbodyType2D.Static;
        }
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {

            isBeingMoved = true;
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
            //isStatic = false;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            isBeingMoved = false;
            //rigidbody.bodyType = RigidbodyType2D.Static;
        }
    }
}
