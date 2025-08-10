using UnityEngine;
using Yarn.Unity;
using System.Collections.Generic;
using System;
using System.Linq;

public class UIManager : MonoBehaviour
{

    // YARN COMMANDS

    [YarnCommand("reveal_object")]
    public static void RevealObject(string objectName)
    {
        Debug.Log($"[UIManager] Attempting to reveal: {objectName}");
        
        Debug.Log($"[Yarn Command] reveal_object called with: '{objectName}'");

        GameObject obj = GameObject.Find(objectName);

        if (obj == null)
        {
            // Try finding it as an inactive object
            obj = Resources.FindObjectsOfTypeAll<GameObject>()
                .FirstOrDefault(g => g.name == objectName);
                
            if (obj == null)
            {
                Debug.LogError($"Yarn Command: Object '{objectName}' not found in scene");
                return;
            }
        }
        
        try 
        {
            obj.SetActive(true);
            Debug.Log($"Successfully displayed object: {objectName}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error activating object '{objectName}': {e.Message}");
        }
    }
    
    [YarnCommand("hide_object")]
    public static void HideObject(string objectName)
    {
       Debug.Log($"[UIManager] Attempting to hide: {objectName}");
        
        Debug.Log($"[Yarn Command] hide_object called with: '{objectName}'");

        GameObject obj = GameObject.Find(objectName);

        if (obj == null)
        {
            Debug.LogError($"Yarn Command: Object '{objectName}' not found in scene");
            return;
        }
        
        try 
        {
            obj.SetActive(false);
            Debug.Log($"Successfully hid object: {objectName}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error hiding object '{objectName}': {e.Message}");
        }
    }
    
}