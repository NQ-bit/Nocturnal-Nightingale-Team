using UnityEngine;
using Yarn.Unity;
using System.Collections.Generic;

[RequireComponent(typeof(DialogueRunner))]
public class UIManager : MonoBehaviour
{
    [System.Serializable]
    public class ManagedObject
    {
        public string id;
        public GameObject gameObject;
    }

    [SerializeField]
    private List<ManagedObject> managedObjects = new List<ManagedObject>();
    private Dictionary<string, GameObject> objectDictionary = new Dictionary<string, GameObject>();
    
    private DialogueRunner dialogueRunner;

    private void Awake()
    {
        dialogueRunner = GetComponent<DialogueRunner>();
        dialogueRunner.AddCommandHandler<string>("reveal_object", RevealObject);
        dialogueRunner.AddCommandHandler<string>("hide_object", HideObject);

        // Build dictionary from serialized list
        foreach (var obj in managedObjects)
        {
            if (obj.gameObject != null && !string.IsNullOrEmpty(obj.id))
            {
                objectDictionary[obj.id] = obj.gameObject;
            }
        }
    }

    private void RevealObject(string objectName)
    {
        if (objectDictionary.TryGetValue(objectName, out GameObject obj))
        {
            obj.SetActive(true);
            Debug.Log($"Successfully revealed: {objectName}");
        }
        else
        {
            Debug.LogWarning($"Object not found in managed objects: {objectName}");
        }
    }

    private void HideObject(string objectName)
    {
        if (objectDictionary.TryGetValue(objectName, out GameObject obj))
        {
            obj.SetActive(false);
            Debug.Log($"Successfully hidden: {objectName}");
        }
        else
        {
            Debug.LogWarning($"Object not found in managed objects: {objectName}");
        }
    }
}