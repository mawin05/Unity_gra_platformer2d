using UnityEngine;

public class GateController : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons; // Lista przycisk�w
    [SerializeField] private Transform targetPosition; // Pozycja, do kt�rej przesuwany jest obiekt
    [SerializeField] private float moveSpeed = 2.0f; // Pr�dko�� ruchu

    private bool allButtonsPressed = false; // Czy wszystkie przyciski s� aktywne?

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
