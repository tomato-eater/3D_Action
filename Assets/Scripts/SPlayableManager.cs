using UnityEngine;

/// <summary>
/// 
/// </summary>
public class SPlayableManager : MonoBehaviour
{
    public static SPlayableManager instance { get; private set; }
    int playableCharacter = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }


}
