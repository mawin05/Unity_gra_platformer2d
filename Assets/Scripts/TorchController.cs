using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchController : MonoBehaviour
{
    private Light2D torchLight;

    // Parametry do sterowania migotaniem
    [SerializeField] private float flickerSpeed = 0.1f; // Czêstotliwoœæ zmiany œwiat³a
    [SerializeField] private float innerRadiusVariation = 0.05f; // Zakres zmian wewnêtrznego ko³a
    [SerializeField] private float outerRadiusVariation = 0.1f; // Zakres zmian zewnêtrznego ko³a

    [SerializeField] private float baseInnerRadius = 0.5f; // Podstawowy promieñ wewnêtrzny
    [SerializeField] private float baseOuterRadius = 1.0f; // Podstawowy promieñ zewnêtrzny

    private float timeCounter;

    void Start()
    {
        // Pobierz komponent Light2D
        torchLight = GetComponent<Light2D>();

        if (torchLight == null)
        {
            Debug.LogError("Brak komponentu Light2D na tym obiekcie!");
        }

        // Ustaw domyœlne wartoœci
        torchLight.pointLightInnerRadius = baseInnerRadius;
        torchLight.pointLightOuterRadius = baseOuterRadius;
    }

    void Update()
    {
        if (torchLight != null)
        {
            // Licznik czasu dla p³ynnego efektu
            timeCounter += Time.deltaTime * flickerSpeed;

            // Oblicz nowy wewnêtrzny promieñ z losow¹ zmian¹
            float newInnerRadius = baseInnerRadius + Random.Range(-innerRadiusVariation, innerRadiusVariation);

            // Oblicz nowy zewnêtrzny promieñ z losow¹ zmian¹
            float newOuterRadius = baseOuterRadius + Random.Range(-outerRadiusVariation, outerRadiusVariation);

            // Ustaw nowe wartoœci promieni
            torchLight.pointLightInnerRadius = Mathf.Clamp(newInnerRadius, 0.1f, baseInnerRadius + innerRadiusVariation);
            torchLight.pointLightOuterRadius = Mathf.Clamp(newOuterRadius, 0.1f, baseOuterRadius + outerRadiusVariation);
        }
    }
}
