using System;
using System.Collections;
using System.Collections.Generic;
using NOOD.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
internal enum BahaviorMS
{
    Next,
    Previous,
    Current
}
public class SoundSetting : MonoBehaviour
{



    [Header("UI")][SerializeField] private Button btnRandomMs;
    [SerializeField] private Button btnPreviousMs;
    [SerializeField] private Button btnNextMs;
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnLoop;
    [SerializeField] private Button btnListMusic;

    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSFX;

    [SerializeField] private TMP_Text textNameMusic;

    [Header("TOGGLE")]
    private bool _togglePauseMusic = false;
    private bool _toggleLoopMusic = false;
    private bool _toggleRandomMusic = false;

    public bool ToggleLoopMusic
    {
        get => _toggleLoopMusic;
        set
        {
            _toggleLoopMusic = value;
            if (value) _toggleRandomMusic = false;
        }
    }

    public bool ToggleRandomMusic
    {
        get => _toggleRandomMusic;
        set
        {
            _toggleRandomMusic = value;
            if (value) _toggleLoopMusic = false;
        }
    }

    private MusicEnum currentMusic => SoundManager.GetCurrentMusic();


    private Coroutine _coroutineCurrentMusic;
    private void Awake()
    {
        SoundManager.InitSoundManager();
    }
    private void Start()
    {
        btnPlay.onClick.AddListener(togglePauseMusic);
        btnNextMs.onClick.AddListener(nextMusic);
        btnPreviousMs.onClick.AddListener(previousMusic);

        btnRandomMs.onClick.AddListener(toggleRandomMusic);
        btnLoop.onClick.AddListener(toggleloopMusic);


        sliderMusic.onValueChanged.AddListener(ChangeMusicVolume);
        sliderSFX.onValueChanged.AddListener(ChangeSFXVolume);


        PlayMusic(forceMusic: "a");
    }

    private void OnDestroy()
    {
        btnPlay.onClick.RemoveAllListeners();
        btnNextMs.onClick.RemoveAllListeners();
        btnPreviousMs.onClick.RemoveAllListeners();

        btnRandomMs.onClick.RemoveAllListeners();
        btnLoop.onClick.RemoveAllListeners();
    }

    private IEnumerator TimmingNextMusic()
    {
        MusicEnum _currentMusic = currentMusic;
        float timeMusic = SoundManager.GetMusicLength(_currentMusic);
        yield return new WaitForSeconds(timeMusic + 2);
        Debug.Log("End of music");
        PlayMusic(); // Play next music
    }


    /// <summary>
    /// UseCase: ONLY RANDOM || LOOP => TRUE
    /// --------
    /// Random = true -> next || previous random play music
    /// --------
    /// Loop = true -> Default play current || next & previous -> play current
    /// --------
    /// Random & Loop = false -> next & previous common
    /// </summary>
    private void PlayMusic(BahaviorMS behavior = BahaviorMS.Next, string forceMusic="")
    {
        MusicEnum musicEnum = ValidateMusic(behavior);

        if (!string.IsNullOrEmpty(forceMusic))
        {
            if (Enum.TryParse(forceMusic, out MusicEnum parsedEnum))
            {
                musicEnum = parsedEnum;
            }
            else
            {
                Debug.LogError($"The name '{forceMusic}' does not correspond to any MusicEnum value.");
            }
        }

        SoundManager.StopAllMusic();
        SoundManager.PlayMusic(musicEnum);

        if (_coroutineCurrentMusic != null)
        {
            StopCoroutine(_coroutineCurrentMusic);
        }

        _coroutineCurrentMusic = StartCoroutine(TimmingNextMusic());

    }

    private MusicEnum ValidateMusic(BahaviorMS behavior = BahaviorMS.Next)
    {
        MusicEnum musicEnum = currentMusic;
        Debug.Log("cc 1:" + musicEnum);
        switch (behavior)
        {
            case BahaviorMS.Next:
                musicEnum = SoundManager.GetNextMusicEnum(currentMusic);
                break;
            case BahaviorMS.Previous:
                musicEnum = SoundManager.GetPreviousMusicEnum(currentMusic);
                break;
            case BahaviorMS.Current:
                //else is current
                break;
            default:
                break;
        }
        Debug.Log("cc 2:" + musicEnum);

        if (_toggleLoopMusic)
        {
            musicEnum = currentMusic;
        }
        else if (_toggleRandomMusic && behavior != BahaviorMS.Current)
        {
            musicEnum = SoundManager.GetRandomMusic();
        }
        Debug.Log("cc 3:" + musicEnum);
        return musicEnum;
    }

    #region Logic

    private DataMusic GetDataMusic(MusicEnum type)
    {
        return SoundManager.GetDataMusic(type);
    }

    private List<DataMusic> GetAllDataMusic()
    {
        return SoundManager.GetAllDataMusic();
    }

    #endregion

    #region Event

    private void togglePauseMusic()
    {
        if (!_togglePauseMusic)
        {
            StopCoroutine(_coroutineCurrentMusic);
            _coroutineCurrentMusic = null;
            SoundManager.StopAllMusic();
        }

        else PlayMusic(BahaviorMS.Current);
        _togglePauseMusic = !_togglePauseMusic;
    }

    private void nextMusic()
    {
        PlayMusic();
        _togglePauseMusic = false;
    }
    private void previousMusic()
    {
        PlayMusic(BahaviorMS.Previous);
        _togglePauseMusic = false;
    }

    private void toggleloopMusic()
    {
        ToggleLoopMusic = !ToggleLoopMusic;
    }

    private void toggleRandomMusic()
    {
        ToggleRandomMusic = !ToggleRandomMusic;
    }

    // Hàm thay ??i volume c?a nh?c
    private void ChangeMusicVolume(float value)
    {
        SoundManager.GlobalMusicVolume = value;
    }

    // Hàm thay ??i volume c?a hi?u ?ng âm thanh
    private void ChangeSFXVolume(float value)
    {
        SoundManager.GlobalSoundVolume = value;
    }
    #endregion
}
