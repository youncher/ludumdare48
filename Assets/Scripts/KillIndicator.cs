using TMPro;
using UnityEngine;

public class KillIndicator : MonoBehaviour
{
    public TextMeshProUGUI text;
    
    public int KillCount = 0;
    public int ToWin = 3;
    private void Start()
    {
        text = GameObject.Find("KillCount").GetComponent<TextMeshProUGUI>();
        text.SetText(KillCount.ToString());
    }

    // add one kill and updates UI
    public void IncrementKills()
    {
        KillCount++;
        text.SetText(KillCount.ToString());
    }
}
