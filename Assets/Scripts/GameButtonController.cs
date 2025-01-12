using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameButtonController : MonoBehaviour
{
    private BoxCollider2D buttonCollider;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private Sprite unpressedSprite;
    private SpriteRenderer renderer;
    private bool isPressedByPlayer = false;
    private bool isPressedByCrate = false;
    private bool isPressed = false;
    [SerializeField] private GameObject gate;
    private GateController gateController;


    void Awake()
    {
        buttonCollider = GetComponent<BoxCollider2D>();
        renderer = GetComponent<SpriteRenderer>();
        gateController = gate.GetComponent<GateController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsPressed()
    {
        return isPressed;
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.CompareTag("Player"))
        {
            if (!isPressedByPlayer)
            {
                renderer.sprite = pressedSprite;
                isPressedByPlayer = true;
                isPressed = true;
                gateController.CheckButtons();
            }
        }

        if (col.CompareTag("CrateBottom"))
        {
            if (!isPressedByCrate)
            {
                renderer.sprite = pressedSprite;
                isPressedByCrate = true;
                isPressed = true;
                gateController.CheckButtons();
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (isPressedByPlayer)
            {
                isPressedByPlayer = false;
            }


            if (!isPressedByPlayer && !isPressedByCrate)
            {
                renderer.sprite = unpressedSprite;
                isPressed = false;
            }
        }

        if (col.CompareTag("CrateBottom"))
        {
            if (isPressedByCrate)
            {
                isPressedByCrate = false;
            }

            if(!isPressedByPlayer && !isPressedByCrate) {
                renderer.sprite = unpressedSprite;
                isPressed = false;
            }
        }
    }
}
