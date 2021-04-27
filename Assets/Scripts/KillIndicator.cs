using TMPro;
using UnityEngine;

public class KillIndicator : MonoBehaviour
{
    public TextMeshProUGUI text;

    public GameManager gameManager;
    public int KillCount = 0;
    public int ToWin = 3;
    
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        text = GameObject.Find("KillCount").GetComponent<TextMeshProUGUI>();
        text.SetText(KillCount.ToString());
    }

    void Update()
    {
        if (KillCount >= ToWin)
        {
            gameManager.AdvanceNextLevel();
        }
    }
    
    // add one kill and updates UI
    public void IncrementKills()
    {
        KillCount++;
        text.SetText(KillCount.ToString());
    }
}
