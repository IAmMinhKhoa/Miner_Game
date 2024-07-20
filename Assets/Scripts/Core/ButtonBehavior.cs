using NOOD.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal enum ButtonState
{
    Idle,
    Hover
}

public class ButtonBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public Image frame;
    public Sprite idleFrame;
    public Color idleTextColor;
    public Sprite hoverFrame;
    public Color hoverTextColor;

    public TextMeshProUGUI btnText;
    public UnityEvent onClickEvent; // public UnityEvent

    private ButtonState _state;

    public SoundEnum clickSoundFx;

    // Start is called before the first frame update
    private void Awake()
    {
        frame = GetComponent<Image>();
        if (frame == null)
        {
            frame = GetComponentInChildren<Image>();
        }

        var btn = GetComponent<Button>();
        if (btn == null) return;

        var navigation = new Navigation();
        navigation.mode = Navigation.Mode.None;
        btn.navigation = navigation;
    }

    private void Start()
    {
        _state = ButtonState.Idle;
    }


    private void SetState(ButtonState state)
    {
        _state = state;

        // Change frame
        switch (state)
        {
            case ButtonState.Idle:
                if (idleFrame != null)
                {
                    frame.sprite = idleFrame;
                    SetText(idleTextColor);
                }
                break;
            case ButtonState.Hover:
                if (hoverFrame != null)
                {
                    frame.sprite = hoverFrame;
                    SetText(hoverTextColor);

                }
                break;
        }
    }
    public void SetText(string text)
    {
        if (btnText == null) return;
        btnText.text = text;
    }
    public void SetText(Color color)
    {
        if (btnText == null || color == null) return;
        color.a = 1f;
        btnText.color = color;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetState(ButtonState.Hover);

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        SetState(ButtonState.Idle);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        gameObject.TryGetComponent<Button>(out var button);

        if (button.interactable)
        {
            //SoundManager.Instance.PlaySfx(clickSoundFx);
            onClickEvent?.Invoke();
        }
        if (gameObject == null)
        {
            SetState(ButtonState.Idle);
        }
    }
}