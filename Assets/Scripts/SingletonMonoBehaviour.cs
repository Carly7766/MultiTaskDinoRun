using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T)FindObjectOfType(typeof(T));

                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<T>();
                    _instance.name = $"{typeof(T).Name} (Singleton)";

                    DontDestroyOnLoad(obj);
                }
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}