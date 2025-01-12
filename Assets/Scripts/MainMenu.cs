using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLevel1ButtonPressed()
    {
        SceneManager.LoadSceneAsync("Level1");
    }

    public void OnLevel2ButtonPressed()
    {
        SceneManager.LoadSceneAsync("Level2");
    }

    public void OnLevel3ButtonPressed()
    {
        SceneManager.LoadSceneAsync("Level3");
    }

    public void OnExitToDesktopButtonPressed()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
