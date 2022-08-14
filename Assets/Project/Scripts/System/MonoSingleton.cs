using UnityEngine;

public abstract class Singleton<T> where T : Singleton<T>, new()
{
    private static volatile T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }

            return instance;
        }
    }
}

public abstract class MonoSingleton : MonoBehaviour
{
    private static volatile GameObject unityObject;

    public static GameObject UnityObject
    {
        get
        {
            if (unityObject == null)
            {
                unityObject = new GameObject("MonoSingleton");
                DontDestroyOnLoad(unityObject);
            }

            return unityObject;
        }
    }
}

public abstract class MonoSingleton<T> : MonoSingleton where T : MonoSingleton<T>
{
    private static volatile T instance;
    private static object gate = new object();

    protected MonoSingleton()
    {
    }

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>() as T;
                if (FindObjectsOfType<T>().Length > 1)
                {
                    return instance;
                }

                if (instance == null)
                {
                    lock (gate)
                    {
                        instance = UnityObject.AddComponent<T>();
                    }
                }
            }

            return instance;
        }

        private set => instance = value;
    }

    static void Destroyed(T component)
    {
        if (instance != component)
        {
            return;
        }

        instance.OnFinalize();
        instance = null;
    }

    protected virtual void OnInitialize()
    {
    }

    protected virtual void OnFinalize()
    {
    }

    void Awake()
    {
        OnInitialize();
    }

    void OnDestroy()
    {
        Destroyed(this as T);
    }

    void OnApplicationQuit()
    {
        Destroyed(this as T);
    }
}