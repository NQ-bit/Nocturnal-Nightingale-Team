using UnityEngine;
using Yarn.Unity;

public class UpdateVariables : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;

    void Awake()
    {
        dialogueRunner.AddCommandHandler("update_reputation", UpdateReputation);
        dialogueRunner.AddCommandHandler("set_reputation", SetReputation);
    }

    void Start()
    {
        dialogueRunner.VariableStorage.SetValue("$reputation", VariableManager.Instance.Reputation);
    }

    public void UpdateReputation()
    {
        dialogueRunner.VariableStorage.TryGetValue<float>("$reputation", out float currentReputation);
        VariableManager.Instance.Reputation = (int)currentReputation;
        Debug.Log($"Reputation updated to: {VariableManager.Instance.Reputation}");
    }

    public void SetReputation()
    {
        int currentReputation = VariableManager.Instance.Reputation;
        dialogueRunner.VariableStorage.SetValue("$reputation", currentReputation);
        Debug.Log($"Reputation set to: {VariableManager.Instance.Reputation}");
    }
}