using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T _instance;
    static bool _isApplicationQuiting;
    
    public static T Instance
    {
        get
        {
            if (_isApplicationQuiting) return null;
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if(_instance != null)
        {
            Destroy(this);
            return;
        }
        else _instance = (T)(MonoBehaviour)this;
    }
}
