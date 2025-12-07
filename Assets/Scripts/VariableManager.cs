using UnityEngine;

public class VariableManager : MonoBehaviour
{
    public static VariableManager Instance { get; private set; }
    
    public int Reputation { get; set; }

    void Awake()
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
    }
}