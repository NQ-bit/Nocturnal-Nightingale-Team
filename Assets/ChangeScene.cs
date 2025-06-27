using UnityEngine;
using UnityEngine.SceneManagement; 
public class ChangeScene : MonoBehaviour
{
    public void GoToCutscene()
    {
        SceneManager.LoadScene("Cutscene");
    }
    public void GoToMainGameScene()
    {

        SceneManager.LoadScene("Main Game");

    }
}
