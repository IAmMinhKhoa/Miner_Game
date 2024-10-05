using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patterns
{
  public abstract class Singleton<T> : MonoBehaviour where T : Component
  {
    private static T s_instance;
	// Flag to control whether to persist across scenes or not
	protected bool isPersistent = true;

	public static T Instance
    {
      get
      {
        if(s_instance == null)
        {
          s_instance = FindObjectOfType<T>();
          if(s_instance == null)
          {
            GameObject obj = new GameObject();
            obj.name= typeof(T).Name;
            s_instance = obj.AddComponent<T>();
          }
        }
        return s_instance;
      }
    }


    protected virtual void Awake()
    {
      if(s_instance == null)
      {
        s_instance = this as T;
		if (isPersistent)
		{
			DontDestroyOnLoad(gameObject);
		}
	}
      else
      {
        Destroy(gameObject);
      }
    }
		protected virtual void OnDestroy()
		{
			if (s_instance == this)
			{
				s_instance = null; 
			}
		}
	}
}
