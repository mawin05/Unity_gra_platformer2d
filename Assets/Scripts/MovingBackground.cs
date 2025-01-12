using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackground : MonoBehaviour
{
    private float length, startpos, ypos;
    public GameObject cam;
    public float parallaxEffect;
    public float backgroundScrollSpeed = 2f; // Szybko�� niezale�nego przewijania t�a

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
        // Ruch t�a w oparciu o ruch kamery (paralaksa)
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x);
        float ydist = (cam.transform.position.y * parallaxEffect);

        // Dodanie sta�ego przewijania t�a niezale�nie od kamery
        startpos -= backgroundScrollSpeed * Time.deltaTime;

        // Ustawienie pozycji t�a
        transform.position = new Vector3(startpos + dist, ypos + ydist, transform.position.z);

        // Zawijanie t�a, aby nie znika�o z widoku
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
