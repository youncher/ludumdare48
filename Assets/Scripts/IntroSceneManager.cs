using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour
{
    public void HandleStartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
