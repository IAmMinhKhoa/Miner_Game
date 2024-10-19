using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    private float velocity = 1.5f;
    private float rotationSpeed = 10f;
    private bool isMouseClick;
    public int score = 0;
    public UnityEvent<int> OnChangeScore;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }
    void Start()
    {
        InputManager.Instance.OnMouseClick.AddListener(OnMouseClick);
    }
    private void OnMouseClick(bool isMouseClick)
    {
        this.isMouseClick = isMouseClick;
        rb.useGravity = true;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rb.velocity = Vector2.up * velocity;
        } 
            
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0,0,rb.velocity.y* rotationSpeed);
    }
    private void OnCollisionEnter(Collision collision)
    {
		StartCoroutine(GameManager.Instance.GameOver());
	}
    public void UpScore(int value)
    {
        score += value;
        OnChangeScore?.Invoke(score);
    }
    private void OnDisable()
    {
        OnChangeScore.RemoveAllListeners();
    }
}
