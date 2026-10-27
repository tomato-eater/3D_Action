using UnityEngine;

/// <summary>
/// 
/// </summary>
public class SPlayableManager : MonoBehaviour
{
    public static SPlayableManager Instance { get; private set; }
    int playableCharacter = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


}
