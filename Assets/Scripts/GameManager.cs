using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public string currentSceneName;

    [Header("Scene Names")]
    public string mainMenuSceneName = "TitleScreen";
    public string cutSceneName = "IntroScene";
    public string firstGameSceneName = "Beach";
    public string secondGameSceneName = "TownSquare";
    public string thirdGameSceneName = "Warehouse";
    public string goodEndingSceneName = "GoodEnding";
    public string badEndingSceneName = "BadEnding";

    [Header("Fade Settings")]
    [SerializeField] private float delayBeforeLoad = 1f; // Extra delay after fade

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        currentSceneName = SceneManager.GetActiveScene().name;
    }

    public void LoadCutScene()
    {
        StartCoroutine(LoadSceneWithDelay(cutSceneName));
    }

    public void LoadFirstGameScene()
    {
        StartCoroutine(LoadSceneWithDelay(firstGameSceneName));
    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(LoadSceneWithDelay(mainMenuSceneName));
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    IEnumerator LoadSceneWithDelay(string sceneName)
    {
        yield return new WaitForSeconds(delayBeforeLoad);
        SceneManager.LoadScene(sceneName);
    }
}