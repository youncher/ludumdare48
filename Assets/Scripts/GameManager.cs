using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject gameOverDisplayGO;
    private GameOverDisplay gameOverDisplay;
    
    private void Start()
    {
        gameOverDisplay = gameOverDisplayGO.GetComponent<GameOverDisplay>();
    }
    
    // Parameter: string - "win" or "lose"
    public void HandleGameOver(string winStatus)
    {
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
