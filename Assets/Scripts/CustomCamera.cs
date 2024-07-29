using NOOD;
using UnityEngine;

public class CustomCamera : Patterns.Singleton<CustomCamera>
{
    [SerializeField] private float minY, maxY;
    private Camera _camera;

    protected override void Awake()
    {
        base.Awake();
        _camera = this.GetComponent<Camera>();
    }

    void Start()
    {
        _camera.orthographicSize = NoodyCustomCode.CalculateOrthoCamSize(_camera, 0).size;
    }

    void Update()
    {
        if(NoodyCustomCode.IsPointerOverUIElement() == false)
            NoodyCustomCode.DragCamera(this.gameObject, minX: 0, maxX: 0, minY, maxY, isHorizontal: false, isVertical: true, direction: -1);
        
    }

    public void SetMaxY(float y)
    {
        maxY = y;
    }

    public Transform GetCurrentTransform()
    {
        return transform;
    }

        
}
