 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
________ DEFINITION ________
Class Name: MonoSingleton
Purpose: A base class that any class can extend to persist across scenes with one instance (singleton pattern)
________ USAGE ________
* when creating a MonoBehavior class, make it extend this class with itself as the type.
* if you need to do custom things in Awake(), make sure you call base.Awake()
 +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public static T Instance
    {
        get{
            if(instance == null){
                instance = FindObjectOfType<T>();
                if(instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    public virtual void Awake() 
    {
        if(instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
