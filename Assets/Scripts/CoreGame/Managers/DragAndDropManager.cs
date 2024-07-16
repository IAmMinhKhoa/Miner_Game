using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropManager : Patterns.Singleton<DragAndDropManager>
{
    private GameObject _dragAndDropObject;
    public GameObject LastDragObject { get; set; }    
    public GameObject DragAndDropObject 
    { 
        get => _dragAndDropObject; 
        set
        {
            _dragAndDropObject = value;
            LastDragObject = value;
        } 
    }
}
