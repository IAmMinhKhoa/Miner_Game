using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;

public class CustomCamera : Patterns.Singleton<CustomCamera>
{
    private Camera _camera;

    void Awake()
    {
        _camera = this.GetComponent<Camera>();
    }

    void Start()
    {
        _camera.orthographicSize = NoodyCustomCode.CalculateOrthoCamSize(_camera, 0).size;
    }

    void Update()
    {
        if(NoodyCustomCode.IsPointerOverUIElement() == false)
            NoodyCustomCode.DragCamera(this.gameObject, horizontal: false, vertical: true, direction: -1);
    }

    public Transform GetCurrentTranform()
    {
        return transform;
    }

        
}
