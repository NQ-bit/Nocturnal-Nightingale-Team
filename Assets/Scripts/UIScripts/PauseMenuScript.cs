using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{

    //public GameObject pauseMenuCanvas;
    //public static bool gamePaused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //pauseMenuCanvas.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
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


    // Update is called once per frame
    void Update()
    {
        #region Esc Key
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (gamePaused)
        //    {
        //        ResumeGame();
        //    }
        //    else
        //    {
        //        PauseGame();
        //        Debug.Log("Pause Menu open!");
        //    }
        //}
        #endregion
    }

    #region Resume & Pause
    //public void ResumeGame()
    //{
    //    pauseMenuCanvas.SetActive(false);
    //    Time.timeScale = 1f; //this is resuming the game time
    //    gamePaused = false;
    //}

    //void PauseGame()
    //{
    //    pauseMenuCanvas.SetActive(true);
    //    Time.timeScale = 0f; //pausing the game
    //    gamePaused = true;
    //}
    #endregion 
}
