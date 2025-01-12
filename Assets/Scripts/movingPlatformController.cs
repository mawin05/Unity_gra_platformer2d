using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatformController : MonoBehaviour
{
    private bool isMovingRight = false;
    [Range(0.01f, 20.0f)] [SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private float moveRange = 1.0f;
    float startPositionX;

    void Awake()
    {
        startPositionX = this.transform.position.x;
    }

    void Update()
    {
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
        if (true) //prawo
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        }
    }

    void moveLeft()
    {
        if (true) //lewo
        {
            transform.Translate(-1 * moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

}
