using UnityEngine;

public class GameOverDisplay : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject losePanel;

    // Display Game over panel
    public void DisplayGameOver(string winStatus)
    {
        // Display win or lose text
        if (string.Equals(winStatus, "win"))
        {
            winPanel.SetActive(true);
        }
        else if (string.Equals(winStatus, "lose"))
        {
            losePanel.SetActive(true);
        }
        // Nothing else should be accepted
    }
}
