using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PauseMenuWH : MonoBehaviour
{

    public GameObject pauseMenuCanvas;
    private bool gamePaused;

    //public AudioSource pauseMenuBGMusic;
    
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gamePaused)
            {
                PauseGame(); //if the game isn't pause this will pause it
                Debug.Log("Pause Menu");
                //pauseMenuBGMusic.Play();
                //pauseMenuBGMusic.loop = true;
            }
            else
            {
                ResumeGame();
                Debug.Log("Resume Game");
                //pauseMenuBGMusic.Stop();
                //pauseMenuBGMusic.loop = false;
            }
        }
        #endregion
    }

    #region Resume & Pause

    void PauseGame()
    {
        gamePaused = true;
        //Time.timeScale = 0f; //pausing the game
        pauseMenuCanvas.SetActive(true);
    }

    public void ResumeGame()
    {
        gamePaused = false;
        //Time.timeScale = 1f; //this is resuming the game time
        pauseMenuCanvas.SetActive(false);
    }
    #endregion 
}
