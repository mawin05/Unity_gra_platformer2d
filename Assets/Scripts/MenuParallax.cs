using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuParallax : MonoBehaviour
{
    private float length, startpos, ypos;
    public GameObject cam;
    public float parallaxEffect;
    [SerializeField] private float moveSpeed = 10;

    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position.x;
        ypos = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x / 3;
    }

    // Update is called once per frame
    void Update()
    {
        // moving the camera
        cam.transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);

        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);
        float ydist = (cam.transform.position.y * parallaxEffect);

        transform.position = new Vector3(startpos + dist, ypos + ydist, transform.position.z);
        if (temp > startpos + length)
        {
            startpos += length;
        }
        else if (temp < startpos - length)
        {
            startpos -= length;
        }
    }
}
