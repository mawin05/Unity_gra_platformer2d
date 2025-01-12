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
            // Przypisz pozycjê gracza do pozycji t³a z uwzglêdnieniem offsetu
            transform.position = player.position + offset;
        }
    }
}
