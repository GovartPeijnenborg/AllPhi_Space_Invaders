using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int Difficulty;
    public void OnPlayButton()
    {
        SceneManager.LoadScene(1);
    }
    public void OnDifficultyChanged()
    {
        Difficulty = 
    }
    public void OnQuitButton() 
    {
        Application.Quit();
    }
}
