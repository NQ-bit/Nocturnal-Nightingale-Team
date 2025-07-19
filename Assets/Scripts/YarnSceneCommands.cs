using UnityEngine;
using Yarn.Unity;
using System.Collections;
using System.Linq;
using System;

public class YarnSceneCommands : MonoBehaviour
{
    [Header("Target Clue Objects")]
    [SerializeField] private GameObject[] clueObjects;
    
    private DialogueRunner dialogueRunner;

    private void Start()
    {
        dialogueRunner = FindFirstObjectByType<DialogueRunner>();
        if (dialogueRunner == null)
        {
            Debug.LogError("YarnSceneCommands requires a DialogueRunner in the scene!");
        }
        
        clueObjects = GameObject.FindGameObjectsWithTag("Clue");
    }

    [YarnCommand("display_object")]
    public void DisplayObject(string objectName)
    {
        Debug.Log($"Yarn Command: Displaying object '{objectName}'");
    }
    
    // Basic scene transition command
    [YarnCommand("change_scene")]
    public void ChangeScene(string sceneName)
    {
        Debug.Log($"Yarn Command: Changing to scene '{sceneName}'");
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
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
    
    
    // Trigger ending directly from dialogue
    [YarnCommand("trigger_ending")]
    public static void TriggerEnding()
    {
        Debug.Log("Yarn Command: Triggering game ending");
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TriggerEnding();
        }
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