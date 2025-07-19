using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public string currentSceneName;
    private int _score;

    [Header("Score Settings")]
    [Tooltip("Score threshold for triggering the ending scene")]
    public int endingThreshold = 20; // need to be discussed with team

    [Header("Scene Names")]
    public string mainMenuSceneName = "TitleScreen";
    public string cutSceneName = "Cutscene";
    public string tutorialGameSceneName = "Tutorial";
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
        _score = 0; // Initialize score
    }

    public void LoadCutScene()
    {
        StartCoroutine(LoadSceneWithDelay(cutSceneName));
    }
    
    public void LoadMainGame()
    {
        StartCoroutine(LoadSceneWithDelay(tutorialGameSceneName));
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

    public void AddScore(int amount)
    {
        _score += amount;
        Debug.Log($"[GameManager] Score updated to {_score}");
    }

    public void TriggerEnding()
    {
        if (_score > endingThreshold) {
                SceneManager.LoadScene(badEndingSceneName);
        } else {
            SceneManager.LoadScene(goodEndingSceneName);
        }
    }
}