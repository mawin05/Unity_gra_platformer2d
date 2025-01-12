using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // Prefab pocisku
    [SerializeField] private bool isFacingRight = true; // Kierunek, w kt�rym patrzy armata
    [SerializeField] private float bulletSpeed = 5.0f; // Pr�dko�� pocisku
    [SerializeField] private float shootDelay = 0f; // Op�nienie przed rozpocz�ciem strzelania
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        // Rozpocznij strzelanie z op�nieniem
        InvokeRepeating("Shoot", shootDelay, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        // Mo�na tutaj doda� dodatkow� logik�, je�li b�dzie potrzebna
    }

    void Shoot()
    {
        // Animacja strzelania
        anim.SetTrigger("isShooting");

        // Tworzenie pocisku
        GameObject bullet = Instantiate(bulletPrefab, this.transform.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        Transform bulletTransform = bullet.GetComponent<Transform>();

        // Kierunek strza�u
        int direction = isFacingRight ? 1 : -1;

        // Ustawienie skali pocisku w zale�no�ci od kierunku
        Vector3 theScale = bullet.transform.localScale;
        theScale.x *= -1 * direction;
        bullet.transform.localScale = theScale;

        // Nadanie pr�dko�ci pociskowi
        bulletRb.AddForce(direction * transform.right * bulletSpeed, ForceMode2D.Impulse);
    }
}