using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using TMPro;

public enum GameState { GAME, PAUSEMENU, LEVEL_COMPLETED, OPTIONS, LEVEL_FAILED};

public class GameManager : MonoBehaviour
{
    public GameState currentGamestate = GameState.GAME;
    public Canvas inGameCanvas;
    public Canvas pauseMenuCanvas;
    public Canvas levelCompletedCanvas;
    public Canvas OptionCanvas;
    public Canvas levelFailedCanvas;
    public static GameManager instance;

    [Header("Texts - UI")]
    public TMP_Text timeLabel;
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text enemyCount;
    public TMP_Text Score;
    public TMP_Text HighScore;
    public TMP_Text QualityLabel;
    private int score = 0;
    public Image[] keysTab;
    private int keysFound = 0;
    public Image[] hearts;
    private int lives = 3;
    private float timer = 0;
    private int TotalEnemies;
    private const string keyHighScore1 = "HighScoreLevel1";
    private const string keyHighScore2 = "HighScoreLevel2";
    private const string keyHighScore3 = "HighScoreLevel3";

    private const string keyVolume = "Volume";
    [SerializeField] private Slider volumeSlider;

    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }

    public void AddKey()
    {
        keysTab[keysFound].color = Color.white;
        keysFound++;
    }

    public void AddLife()
    {
        if(lives < 3)
        {
            lives++;
            hearts[lives - 1].enabled = true;
        }
    }

    public void RemoveLife()
    {
        if (lives > 0)
        {
            lives--;
            Debug.Log(lives);
            hearts[lives].enabled = false;
        }

        if(lives == 0)
        {
            LevelFailed();
        }
    }

    public void RemoveEnemy()
    {
        TotalEnemies--;
        enemyCount.text = TotalEnemies.ToString();
    }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Duplicated Game Manager", gameObject);
        }


        scoreText.text = score.ToString();
        enemyCount.text = TotalEnemies.ToString();

        for(int i = 0; i < 3; i++)
        {
            keysTab[i].color = Color.grey;
        }

        for(int i=0; i < lives; i++)
        {
            hearts[i].enabled = true;
        }

        if (!PlayerPrefs.HasKey(keyHighScore1))
        {
            PlayerPrefs.SetInt(keyHighScore1, 0);
        }

        if (!PlayerPrefs.HasKey(keyHighScore2))
        {
            PlayerPrefs.SetInt(keyHighScore2, 0);
        }

        if (!PlayerPrefs.HasKey(keyVolume))
        {
            PlayerPrefs.SetFloat(keyVolume, 0.5f);
        }

        AudioListener.volume = PlayerPrefs.GetFloat(keyVolume);
        volumeSlider.value = PlayerPrefs.GetFloat(keyVolume);

        InGame();
    }

    // Start is called before the first frame update
    void Start()
    {
        TotalEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        enemyCount.text = TotalEnemies.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        // Oblicz minuty i sekundy.
        int minutes = Mathf.FloorToInt(timer / 60); // Ca³kowite minuty.
        int seconds = Mathf.FloorToInt(timer % 60); // Pozosta³e sekundy.

        // Sformatuj tekst i ustaw go w komponencie TMP_Text.
        timerText.text = String.Format("{0:00}:{1:00}", minutes, seconds);

        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if(currentGamestate == GameState.GAME)
            {
                PauseMenu();
            }
            else if(currentGamestate != GameState.LEVEL_FAILED && currentGamestate != GameState.LEVEL_COMPLETED)
            {
                EventSystem.current.SetSelectedGameObject(null);
                InGame();
            }
        }
    }

    void SetGameState(GameState newGameState)
    {
        currentGamestate = newGameState;
        if(currentGamestate == GameState.GAME) 
        {
            inGameCanvas.enabled = true;
        }
        else
        {
            inGameCanvas.enabled = false;
        }

        if(currentGamestate == GameState.LEVEL_COMPLETED)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            string key = "";
            if(currentScene.name == "Level1")
            {
                key = keyHighScore1;

            }else if(currentScene.name == "Level2")
            {
                key = keyHighScore2;

            }else if(currentScene.name == "Level3")
            {
                key = keyHighScore3;

            }

            int highScore = PlayerPrefs.GetInt(key);
            if (highScore < score)
            {
                highScore = score;
                PlayerPrefs.SetInt(key, highScore);
            }

            Score.text = "Score = " + score.ToString();
            HighScore.text = "High Score = " + highScore.ToString();
            timeLabel.text = "Time: " + timer.ToString();
        }


        pauseMenuCanvas.enabled = currentGamestate == GameState.PAUSEMENU ? true : false;
        levelCompletedCanvas.enabled = currentGamestate == GameState.LEVEL_COMPLETED ? true : false;
        OptionCanvas.enabled = currentGamestate == GameState.OPTIONS ? true : false;
        levelFailedCanvas.enabled = currentGamestate == GameState.LEVEL_FAILED ? true : false;
    }

    public void PauseMenu()
    {
        SetGameState(GameState.PAUSEMENU);
        Time.timeScale = 0;
    }

    public void InGame()
    {
        SetGameState(GameState.GAME);
        Time.timeScale = 1;
    }

    public void Options()
    {
        SetGameState(GameState.OPTIONS);
    }

    public void LevelCompleted()
    {
        SetGameState(GameState.LEVEL_COMPLETED);
        Time.timeScale = 0;
    }

    public void LevelFailed()
    {
        SetGameState(GameState.LEVEL_FAILED);
        Time.timeScale = 0;
    }

    public void OnResumeButtonClicked()
    {
        EventSystem.current.SetSelectedGameObject(null);
        InGame();
    }

    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnReturnToMainMenuButtonClicked()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void OnOptionButtonClicked()
    {
        Options();
        Time.timeScale = 0;
    }

    public void IncreaseQuality()
    {
        QualitySettings.IncreaseLevel(true);
        QualityLabel.text = "Quality - " + QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    public void DecreaseQuality()
    {
        QualitySettings.DecreaseLevel(true);
        QualityLabel.text = "Quality - " + QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    public void SetVolume()
    {
        AudioListener.volume = volumeSlider.value;
        PlayerPrefs.SetFloat(keyVolume, volumeSlider.value);
        volumeSlider.value = PlayerPrefs.GetFloat(keyVolume);
    }

    public int getLives()
    {
        return lives;
    }
}
