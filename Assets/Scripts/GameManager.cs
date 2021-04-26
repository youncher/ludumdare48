using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject playerGO;
    public GameObject gameOverDisplayGO;
    public GameObject gloomProjectileControllerGO;
    
    private PlayerController player;
    private GameOverDisplay gameOverDisplay;
    private GloomProjectileController gloomProjectileController;
    
    private void Start()
    {
        gameOverDisplay = gameOverDisplayGO.GetComponent<GameOverDisplay>();
        player = playerGO.GetComponent<PlayerController>();
        gloomProjectileController = gloomProjectileControllerGO.GetComponent<GloomProjectileController>();
    }
    
    // Parameter: string - "win" or "lose"
    public void HandleGameOver(string winStatus)
    {
        gloomProjectileController.gameOver = true;
        player.gameOver = true;
        gameOverDisplay.gameObject.SetActive(true);
        gameOverDisplay.DisplayGameOver(winStatus);
    }
    
    // TODO update "SampleScene" name to something else
    public void LoadLevel1()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("IntroScene");
    }
}
