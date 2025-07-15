using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public string currentSceneName;
    private int _score;

    [Header("Ending Threshold")]
    [Tooltip("Score above this loads Bad Ending; otherwise Good Ending")] 
    public int endingThreshold = 20; // need to be discussed with team

    [Header("Scene Names")]
    public string mainMenuSceneName = "TitleScreen";
    public string cutSceneName = "CutScene";

    public string tutorialGameSceneName = "Tutorial";
    public string firstGameSceneName = "Beach";
    public string secondGameSceneName = "TownSquare";
    public string thirdGameSceneName = "Warehouse";
    public string goodEndingSceneName = "GoodEnding";
    public string badEndingSceneName = "BadEnding";
    
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
        SceneManager.LoadScene(cutSceneName);
    }
    
    public void PlayGame()
    {
        SceneManager.LoadScene(tutorialGameSceneName);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Reset time scale
        SceneManager.LoadScene(mainMenuSceneName);
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
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