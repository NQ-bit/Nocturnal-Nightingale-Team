//using System.Collections;
//using System.Collections.Generic;
//^ can remove later after testing or merge to the main menu script
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempMenuScript : MonoBehaviour
{
    #region Plays menu muscic
    ///// <summary>
    ///// commenting out for now but it works just need it to work across all
    ///// scenes will adjust this later on
    ///// </summary>
    //private void Start()
    //{
    //    AllMusicManager.Instance.PlayMusic("Main Menu");
    //}
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        SceneManager.LoadScene("Beach");
        Time.timeScale = 1f; //this is resuming the game time
        //AllMusicManager.Instance.PlayMusic("Alcove Music");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("TitleScreen");
        Time.timeScale = 1f; //this is resuming the game time
        //AllMusicManager.Instance.PlayMusic("Main Menu");
    }

    public void ToCredits()
    {
        SceneManager.LoadScene("Credits");
        Time.timeScale = 1f; //this is resuming the game time
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
            Debug.Log("Game quit!");
    }

}
