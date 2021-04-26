using System;
using TMPro;
using UnityEngine;

public class HealthIndicator : MonoBehaviour
{
    private const int MAX_LIFE = 3;

    private GameManager gameManager;
    public int currentLife = MAX_LIFE;
    public TextMeshProUGUI text;
    
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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

        if (currentLife <= 0)
        {
            gameManager.HandleGameOver("lose");
        }
    }
}
