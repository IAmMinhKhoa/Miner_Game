using NOOD.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ButtonState
{
    Click,
    Default
}

public class ButtonBehavior : MonoBehaviour, IPointerDownHandler
{
    [Header("UI Components")]
    public Image frame;
    public Sprite clickImage;
    private Sprite defaultImage;

    [Header("Events")]
    public UnityEvent onClickEvent;

    [Header("Audio")]
    public SoundEnum clickSoundFx = SoundEnum.click;

    private ButtonState _state;

    private void Awake()
    {
        if (frame == null)
        {
            frame = GetComponent<Image>() ?? GetComponentInChildren<Image>();
        }

        defaultImage = frame.sprite;

        var btn = GetComponent<Button>();
        if (btn != null)
        {
            var navigation = new Navigation { mode = Navigation.Mode.None };
            btn.navigation = navigation;
        }
    }


    public void SetState(ButtonState state)
    {
        _state = state;
        frame.sprite = state switch
        {
            ButtonState.Click when clickImage != null => clickImage,
            ButtonState.Default when defaultImage != null => defaultImage,
            _ => frame.sprite
        };
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        if (TryGetComponent(out Button button) && button.interactable)
        {
            // SoundManager.Instance.PlaySfx(clickSoundFx);
            SoundManager.PlaySound(SoundEnum.click);
            onClickEvent?.Invoke();
        }

        SetState(ButtonState.Click);
    }

}