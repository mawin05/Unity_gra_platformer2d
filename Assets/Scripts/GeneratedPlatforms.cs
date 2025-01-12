using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedPlatforms : MonoBehaviour
{
    [SerializeField] GameObject platformPrefab;
    [SerializeField] float rotationSpeed = 10f; // Szybko�� obrotu platform
    const int PLATFORMS_NUM = 4;
    GameObject[] platforms;
    Vector3[] positions; // U�ycie Vector3 zamiast Vector2
    float angle = 0f; // K�t obrotu platform

    private void Awake()
    {
        platforms = new GameObject[PLATFORMS_NUM];
        positions = new Vector3[PLATFORMS_NUM];
        float rad = 0f;
        float pi = Mathf.PI;
        float deltaAngle = 2 * pi / PLATFORMS_NUM;
        float radius = 4.0f;

        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            // Oblicz pozycj� pocz�tkow� platformy
            positions[i] = new Vector3(
                transform.position.x + Mathf.Sin(rad) * radius,
                transform.position.y + Mathf.Cos(rad) * radius,
                0 // Pozycja Z dla 2D
            );

            // Tworzenie platformy
            platforms[i] = Instantiate(platformPrefab, positions[i], Quaternion.identity);
            rad += deltaAngle;
        }
    }


    void Update()
    {
        angle += rotationSpeed * Time.deltaTime; // Aktualizacja k�ta obrotu
        float rad = angle; // Rozpoczynanie obrotu od zmiennej `angle`
        float pi = Mathf.PI;
        float deltaAngle = 2 * pi / PLATFORMS_NUM;
        float radius = 4.0f;

        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            // Oblicz now� pozycj� platformy
            positions[i].x = transform.position.x + Mathf.Sin(rad) * radius;
            positions[i].y = transform.position.y + Mathf.Cos(rad) * radius;

            // Przypisz now� pozycj� do istniej�cej platformy
            platforms[i].transform.position = positions[i];

            rad += deltaAngle; // Zwi�ksz k�t dla kolejnej platformy
        }
    }
}
