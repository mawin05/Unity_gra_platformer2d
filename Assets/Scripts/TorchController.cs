using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchController : MonoBehaviour
{
    private Light2D torchLight;

    // Parametry do sterowania migotaniem
    [SerializeField] private float flickerSpeed = 0.1f; // Cz�stotliwo�� zmiany �wiat�a
    [SerializeField] private float innerRadiusVariation = 0.05f; // Zakres zmian wewn�trznego ko�a
    [SerializeField] private float outerRadiusVariation = 0.1f; // Zakres zmian zewn�trznego ko�a

    [SerializeField] private float baseInnerRadius = 0.5f; // Podstawowy promie� wewn�trzny
    [SerializeField] private float baseOuterRadius = 1.0f; // Podstawowy promie� zewn�trzny

    private float timeCounter;

    void Start()
    {
        // Pobierz komponent Light2D
        torchLight = GetComponent<Light2D>();

        if (torchLight == null)
        {
            Debug.LogError("Brak komponentu Light2D na tym obiekcie!");
        }

        // Ustaw domy�lne warto�ci
        torchLight.pointLightInnerRadius = baseInnerRadius;
        torchLight.pointLightOuterRadius = baseOuterRadius;
    }

    void Update()
    {
        if (torchLight != null)
        {
            // Licznik czasu dla p�ynnego efektu
            timeCounter += Time.deltaTime * flickerSpeed;

            // Oblicz nowy wewn�trzny promie� z losow� zmian�
            float newInnerRadius = baseInnerRadius + Random.Range(-innerRadiusVariation, innerRadiusVariation);

            // Oblicz nowy zewn�trzny promie� z losow� zmian�
            float newOuterRadius = baseOuterRadius + Random.Range(-outerRadiusVariation, outerRadiusVariation);

            // Ustaw nowe warto�ci promieni
            torchLight.pointLightInnerRadius = Mathf.Clamp(newInnerRadius, 0.1f, baseInnerRadius + innerRadiusVariation);
            torchLight.pointLightOuterRadius = Mathf.Clamp(newOuterRadius, 0.1f, baseOuterRadius + outerRadiusVariation);
        }
    }
}
