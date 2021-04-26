using TMPro;
using UnityEngine;

public class HealthIndicator : MonoBehaviour
{
    private const int MAX_LIFE = 3;
    
    public int currentLife = MAX_LIFE;
    public TextMeshProUGUI text;
    
    private void Start()
    {
        text = GameObject.Find("RemainingLife").GetComponent<TextMeshProUGUI>();
        text.SetText(currentLife.ToString());
    }

    // Subtract one life and updates UI
    public void ReduceLife()
    {
        if (currentLife > 0)
        {
            currentLife--;
            text.SetText(currentLife.ToString());
        }
    }
}
