using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFall : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private Transform spriteTransform;
    private Vector3 spriteStartPosition;
    private BoxCollider2D platformCollider;
    [SerializeField] public float shakeTime;
    [SerializeField]public float shakeMagnitude;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteTransform = transform.GetChild(0);
        spriteStartPosition = spriteTransform.localPosition;
        platformCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D cal)
    {
        if (cal.CompareTag("Player") && cal.transform.position.y > this.transform.position.y)
        {
            StartCoroutine(ShakePlatform());
        }
    }

    IEnumerator ShakePlatform()
    {
        float elapsedTime=0.0f;

        while (elapsedTime < shakeTime) {
            spriteTransform.localPosition = spriteStartPosition + new Vector3(Random.Range(-shakeMagnitude, shakeMagnitude),Random.Range(-shakeMagnitude, shakeMagnitude),0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteTransform.localPosition = spriteStartPosition;

        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();
        foreach (BoxCollider2D collider in colliders)
        {
            collider.enabled = false;
        }
        platformCollider.enabled = false;
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        //Destroy(gameObject, 1.0f);
        StartCoroutine(FadeAndDestroy());
    }

    IEnumerator FadeAndDestroy()
    {
        SpriteRenderer renderer = spriteTransform.GetComponent<SpriteRenderer>();
        Color startColor = renderer.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        float elapsedTime = 0.0f;
        float fadeDuration = 1.0f;

        while (elapsedTime < fadeDuration)
        {
            renderer.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
