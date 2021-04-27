using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int killsNeededToWin { get; set; }
    
    public GameObject playerGO;
    public GameObject gameOverDisplayGO;
    public GameObject gloomProjectileControllerGO;
    public GameObject targetCounterTextGO;
    
    private const int MAX_LEVEL_TO_WIN = 1;
    private PlayerController player;
    private GameOverDisplay gameOverDisplay;
    private GloomProjectileController gloomProjectileController;
    private TextMeshProUGUI targetCounterText;
    
    private int currentLevel = 1;
    private bool gameOver = false;

    void Start()
    {
        gameOverDisplay = gameOverDisplayGO.GetComponent<GameOverDisplay>();
        player = playerGO.GetComponent<PlayerController>();
        gloomProjectileController = gloomProjectileControllerGO.GetComponent<GloomProjectileController>();
        targetCounterText = targetCounterTextGO.GetComponent<TextMeshProUGUI>();

        if (string.Equals(SceneManager.GetActiveScene().name, "Level1Scene"))
        {
            Debug.Log("I'M IN SCENE 1!!!");
        }

        // Display number of kills to win level
        killsNeededToWin = 3;
        targetCounterText.SetText($"{killsNeededToWin}");
    }
    
    // Parameter: string - "win" or "lose"
    public void HandleGameOver(string winStatus)
    {
        if (!gameOver){
            gameOver = true;
            gloomProjectileController.gameOver = true;
            player.gameOver = true;
            gameOverDisplay.gameObject.SetActive(true);
            gameOverDisplay.DisplayGameOver(winStatus);
        }
    }
    
    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1Scene");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("IntroScene");
    }

    // Advance user to next level
    public void AdvanceNextLevel()
    {
        if (currentLevel >= MAX_LEVEL_TO_WIN)
        {
            Debug.Log("current level:" + currentLevel);
            Debug.Log("MAX_LEVEL_TO_WIN:" + MAX_LEVEL_TO_WIN);
            Debug.Log("You win!");
            HandleGameOver("win");
        }
        //
        // int nextLevel = currentLevel + 1;
        //
        // if (nextLevel == 2)
        // {
        //     Debug.Log("Advancing to Level 2");
        //     killsNeededToWin = 4;
        // } else if (nextLevel == 3)
        // {
        //     Debug.Log("Advancing to Level 3");
        //     killsNeededToWin = 5;
        // }
        //
        // currentLevel = nextLevel;
        
        // TODO advance to next level
    }
}
