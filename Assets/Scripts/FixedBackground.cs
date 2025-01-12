using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedBackground : MonoBehaviour
{

    [SerializeField] private Transform player; // Referencja do gracza
    [SerializeField] private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Przypisz pozycj� gracza do pozycji t�a z uwzgl�dnieniem offsetu
            transform.position = player.position + offset;
        }
    }
}
