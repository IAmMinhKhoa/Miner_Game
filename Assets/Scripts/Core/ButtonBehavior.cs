using NOOD.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ButtonState
{
    Click,
    Default,
    Hover
}

public class ButtonBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,IPointerClickHandler
{
    public Image frame;
    public Sprite idleFrame;
    private Sprite defaultImage;
    public Color idleTextColor;
    public Sprite hoverFrame;
    public Color hoverTextColor;

    public TextMeshProUGUI btnText;
    public UnityEvent onClickEvent; // public UnityEvent

    private ButtonState _state;

    public SoundEnum clickSoundFx;
    private bool toggleBtn = false;

    // Start is called before the first frame update
    private void Awake()
    {
        frame = GetComponent<Image>();
        if (frame == null)
        {
            frame = GetComponentInChildren<Image>();
        }
        defaultImage=frame.sprite;
        var btn = GetComponent<Button>();
        if (btn == null) return;

        var navigation = new Navigation();
        navigation.mode = Navigation.Mode.None;
        btn.navigation = navigation;
    }

    private void Start()
    {
        _state = ButtonState.Click;
    }
        

    public void SetState(ButtonState state)
    {
        _state = state;
        Debug.Log("cpncc:" + state);    
        // Change frame
        switch (state)
        {
            case ButtonState.Click:
                if (idleFrame != null)
                {
                    frame.sprite = idleFrame;
                    SetText(idleTextColor);
                }
                break;
            case ButtonState.Default:
                if (idleFrame != null)
                {
                    frame.sprite = defaultImage;
             
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
            SetState(ButtonState.Click);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       /* if (!toggleBtn)
            SetState(ButtonState.Click  );
        else
            SetState(ButtonState.Default);
        toggleBtn = !toggleBtn;*/
    }
}