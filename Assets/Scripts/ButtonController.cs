using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private AudioSource audioSource; // Lokalny AudioSource
    [SerializeField] public AudioClip buttonClickSound; // DŸwiêk klikniêcia (ustawiany w prefabie)

    void Awake()
    {
        // Pobieramy AudioSource z obiektu
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClickSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound, AudioListener.volume);
        }
    }
}
