using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance
    {
        get { 
            if (instance == null)
                Debug.LogError(typeof(T) + " is missing from the scene. Drag in the prefab");
            return instance; 
        }
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            //Debug.LogError("[Singleton] You're tring to create another instance of " + instance);
            Destroy(this.gameObject);
        }
        else
        {
            instance = (T)this;
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
