using TMPro;
using UnityEngine;

public class KillIndicator : MonoBehaviour
{
    public TextMeshProUGUI text;
    
    public int killCount = 0;
    private void Start()
    {
        text = GameObject.Find("KillCount").GetComponent<TextMeshProUGUI>();
        text.SetText(killCount.ToString());
    }

    // add one kill and updates UI
    public void IncrementKills()
    {
        killCount++;
        text.SetText(killCount.ToString());
    }
}
