using UnityEngine;

public abstract class MonoBehaviourSingleton<T> : MonoBehaviour, ISerializationCallbackReceiver
    where T : MonoBehaviourSingleton<T>
{
    public static T Instance;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Debug.Log($"{name} is destroyed");
        }
        else
        {
            Instance = (T)this;
            Initialize();
            Debug.Log($"{name} is initialized");
        }
    }

    protected abstract void Initialize();

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        /* Nothing to do here */
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        if (Instance == null)
        {
            Instance = (T)this;
        }
    }
}
