using UnityEngine;

public class SingletonDontDestroy<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = (T)FindObjectOfType(typeof(T));
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}