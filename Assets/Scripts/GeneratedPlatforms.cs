using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedPlatforms : MonoBehaviour
{
    [SerializeField] GameObject platformPrefab;
    [SerializeField] float rotationSpeed = 10f; // Szybkoœæ obrotu platform
    const int PLATFORMS_NUM = 4;
    GameObject[] platforms;
    Vector3[] positions; // U¿ycie Vector3 zamiast Vector2
    float angle = 0f; // K¹t obrotu platform

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
            // Oblicz pozycjê pocz¹tkow¹ platformy
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
        angle += rotationSpeed * Time.deltaTime; // Aktualizacja k¹ta obrotu
        float rad = angle; // Rozpoczynanie obrotu od zmiennej `angle`
        float pi = Mathf.PI;
        float deltaAngle = 2 * pi / PLATFORMS_NUM;
        float radius = 4.0f;

        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            // Oblicz now¹ pozycjê platformy
            positions[i].x = transform.position.x + Mathf.Sin(rad) * radius;
            positions[i].y = transform.position.y + Mathf.Cos(rad) * radius;

            // Przypisz now¹ pozycjê do istniej¹cej platformy
            platforms[i].transform.position = positions[i];

            rad += deltaAngle; // Zwiêksz k¹t dla kolejnej platformy
        }
    }
}
