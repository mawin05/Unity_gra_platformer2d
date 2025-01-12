using UnityEngine;

public class GateController : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons; // Lista przycisków
    [SerializeField] private Transform targetPosition; // Pozycja, do której przesuwany jest obiekt
    [SerializeField] private float moveSpeed = 2.0f; // Prêdkoœæ ruchu

    private bool allButtonsPressed = false; // Czy wszystkie przyciski s¹ aktywne?

    private void Awake()
    {

    }

    public void CheckButtons()
    {
        allButtonsPressed = true;

        foreach (GameObject button in buttons)
        {
            GameButtonController buttonController = button.GetComponent<GameButtonController>();
            if (!buttonController.IsPressed())
            {
                allButtonsPressed = false;
                break;
            }
        }

    }

    private void Update()
    {
        if (allButtonsPressed)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);
        }
    }
}
