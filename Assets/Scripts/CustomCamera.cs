using System.Threading;
using DG.Tweening;
using NOOD;
using UnityEngine;
using UnityEngine.Rendering;

public class CustomCamera : Patterns.Singleton<CustomCamera>
{
    [SerializeField] private float minY, maxY;
    [SerializeField] private float _dragSpeed = 1;
    private Camera _camera;
    private Vector3 touchPos;
    private bool isDragging;

    protected override void Awake()
    {
        base.Awake();
        _camera = this.GetComponent<Camera>();
    }

    void Start()
    {
        _camera.orthographicSize = NoodyCustomCode.CalculateOrthoCamSize(_camera, 0).size;
        float screenHeight = Camera.main.pixelHeight;
        minY = screenHeight * minY / 1920;
    }

    void Update()
    {
        if (NoodyCustomCode.IsPointerOverUIElement() == true) return;
        
        // Detect left mouse button press (or touch)
        if (Input.GetMouseButtonDown(0))
        {
            touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
        }

        // If the left mouse button is held down and dragging is active
        if (isDragging && Input.GetMouseButton(0))
        {
            Vector3 difference = touchPos - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 tempPos = transform.position + new Vector3(0, difference.y, 0);
            tempPos.y = Mathf.Clamp(tempPos.y, minY, maxY);

            // Move the camera by the difference
            transform.position = tempPos;
        }

        // Reset dragging flag when the mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
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
