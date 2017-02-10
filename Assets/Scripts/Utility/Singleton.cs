using UnityEngine;
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T instance;

    public static T Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (Instance)
            Destroy(Instance);
        instance = this as T;
        Init();
    }

    protected virtual void Init()
    {

    }
}
