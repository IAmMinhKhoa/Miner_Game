using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : Patterns.Singleton<InputManager>
{
    // Start is called before the first frame update
    public UnityEvent<bool> OnMouseClick;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            OnMouseClick?.Invoke(true);
        }    
    }
}
