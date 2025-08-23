using UnityEngine;
using Yarn.Unity;
using System.Collections;

public class YarnSceneCommands : MonoBehaviour
{
    [Header("Scene Transition Settings")]
    public float transitionDelay = 1f; // Delay before transition starts
    public bool pauseDialogueForTransition = true;
    
    private DialogueRunner dialogueRunner;
    
    private void Start()
    {
        dialogueRunner = FindFirstObjectByType<DialogueRunner>();
        if (dialogueRunner == null)
        {
            Debug.LogError("YarnSceneCommands requires a DialogueRunner in the scene!");
        }
    }
    
    // Basic scene transition command
    [YarnCommand("change_scene")]
    public static void ChangeScene(string sceneName)
    {
        Debug.Log($"Yarn Command: Changing to scene '{sceneName}'");
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
    
    // Scene transition with delay (useful for dramatic effect)
    [YarnCommand("change_scene_delayed")]
    public static void ChangeSceneDelayed(string sceneName, float delay = 2f)
    {
        YarnSceneCommands instance = FindFirstObjectByType<YarnSceneCommands>();
        if (instance != null)
        {
            instance.StartCoroutine(instance.DelayedSceneChange(sceneName, delay));
        }
    }
    
    // End dialogue and then change scene
    [YarnCommand("end_dialogue_and_change_scene")]
    public static void EndDialogueAndChangeScene(string sceneName)
    {
        YarnSceneCommands instance = FindFirstObjectByType<YarnSceneCommands>();
        if (instance != null)
        {
            instance.StartCoroutine(instance.EndDialogueAndTransition(sceneName));
        }
    }
    
    // Change scene with specific player position (useful for doorways)
    [YarnCommand("change_scene_with_position")]
    public static void ChangeSceneWithPosition(string sceneName, float x, float y)
    {
            // Store position for after scene load
            PlayerPrefs.SetFloat("NextSceneX", x);
            PlayerPrefs.SetFloat("NextSceneY", y);
            PlayerPrefs.SetInt("SetPlayerPosition", 1);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        
    }
    
    // Fade to black and change scene (dramatic effect)
    [YarnCommand("fade_to_scene")]
    public static void FadeToScene(string sceneName, float fadeTime = 2f)
    {
        YarnSceneCommands instance = FindFirstObjectByType<YarnSceneCommands>();
        if (instance != null)
        {
            instance.StartCoroutine(instance.FadeTransition(sceneName, fadeTime));
        }
    }
    
    private IEnumerator DelayedSceneChange(string sceneName, float delay)
    {
        Debug.Log($"Waiting {delay} seconds before changing to {sceneName}");
        
        // Optionally pause dialogue during delay
        if (pauseDialogueForTransition && dialogueRunner != null)
        {
            // You can add visual cues here (screen effects, etc.)
        }
        
        yield return new WaitForSeconds(delay);
        ChangeScene(sceneName);
    }
    
    private IEnumerator EndDialogueAndTransition(string sceneName)
    {
        // Stop the current dialogue
        if (dialogueRunner != null && dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.Stop();
        }
        
        // Wait a frame for dialogue to fully close
        yield return null;
        
        // Brief pause for better UX
        yield return new WaitForSeconds(0.5f);
        
        // Change scene
        ChangeScene(sceneName);
    }
    
    private IEnumerator FadeTransition(string sceneName, float fadeTime)
    {
        // This would integrate with your fade system
        Debug.Log($"Fading to black over {fadeTime} seconds, then loading {sceneName}");

        // You could trigger screen fade effects here
        // For example, if you have a UI fade panel:
        // UIManager.Instance.FadeToBlack(fadeTime);
        //TODO: Add fade logic here
        
        yield return new WaitForSeconds(fadeTime);
        ChangeScene(sceneName);
    }
}