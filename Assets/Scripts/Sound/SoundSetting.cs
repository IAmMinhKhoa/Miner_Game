using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NOOD;
using NOOD.Sound;
using Sirenix.OdinInspector;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
internal enum BahaviorMS
{
    Next,
    Current,
	OnClickNext,
	OnClickPrevious
}
public class SoundSetting : MonoBehaviour
{

    [SerializeField] PlayList _playList;

    [Header("UI")]
    [SerializeField] GameObject Container;
    [SerializeField] private Button btnRandomMs;
    [SerializeField] private Button btnPreviousMs;
    [SerializeField] private Button btnNextMs;
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnLoop;
    [SerializeField] private Button btnListMusic;

    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSFX;

    [SerializeField] private TMP_Text textNameMusic;

    [SerializeField] ButtonBehavior btnMusic;
    [SerializeField] ButtonBehavior btnSound;
    [Header("SPINE")]
    #region SPINE 
    [SerializeField] SkeletonGraphic spineDiaThan;
    [SerializeField] SkeletonGraphic spineCanGat;
    [SerializeField] SkeletonGraphic spineRandom;
    [SerializeField] SkeletonGraphic spinePrevious;
    [SerializeField] SkeletonGraphic spineNext;
    [SerializeField] SkeletonGraphic spinePause;
    [SerializeField] SkeletonGraphic spineLoop;
    #endregion
    [Header("TOGGLE")]
    private bool _togglePauseMusic = false;
    private bool _toggleLoopMusic = false;
    private bool _toggleRandomMusic = false;
    //----- Quick On/Off Music & Sound -----
    private bool _toggleMusic = true;
    private bool _togglesound = true;
    public bool TogglePauseMusic
    {
        get => _togglePauseMusic;
        set
        {
            _togglePauseMusic = value;
            if(value)
            {
                spineDiaThan.AnimationState.SetAnimation(0, "DiaThan - Idle", false);

                spineCanGat.AnimationState.SetAnimation(0, "Caygat - Idle", false);
            }
            else
            {
                spineDiaThan.AnimationState.SetAnimation(0, "DiaThan - Active", true);

                spineCanGat.AnimationState.SetAnimation(0, "Caygat - Active", false);
            }
        }
    }
    public bool ToggleLoopMusic
    {
        get => _toggleLoopMusic;
        set
        {
            _toggleLoopMusic = value;
  

            if (value)
            {
                spineLoop.AnimationState.SetAnimation(0, "Loop - Active", false);
                _toggleRandomMusic = false;
                //return button random
                spineRandom.AnimationState.SetAnimation(0, "Random - Idle", false);
            }
            else
            {
                var trackEntry = spineLoop.AnimationState.SetAnimation(0, "Loop - Active2", false);
            }
        }
    }

    public bool ToggleRandomMusic
    {
        get => _toggleRandomMusic;
        set
        {
            _toggleRandomMusic = value;
          

            if (value)
            {
                var trackEntry = spineRandom.AnimationState.SetAnimation(0, "Random - Active", false);
                _toggleLoopMusic = false;
                //return button loop
                spineLoop.AnimationState.SetAnimation(0, "Loop - Idle", false);
            }
            else
            {
                var trackEntry = spineRandom.AnimationState.SetAnimation(0, "Random - Active2", false);
            }


        }
    }

    private MusicEnum currentMusic => SoundManager.GetCurrentMusic();

   // private Coroutine _coroutineCurrentMusic;
    private void Awake()
    {
        SoundManager.InitSoundManager();
        PlayMusic(forceMusic: "b");
    }
    private void Start()
    {
        sliderMusic.value = SoundManager.GlobalMusicVolume;
        sliderSFX.value = SoundManager.GlobalSoundVolume;


        btnPlay.onClick.AddListener(togglePauseMusic);
        btnNextMs.onClick.AddListener(OnClicknextMusic);
        btnPreviousMs.onClick.AddListener(OnClickPreviousMusic);

        btnRandomMs.onClick.AddListener(toggleRandomMusic);
        btnLoop.onClick.AddListener(toggleloopMusic);
        btnListMusic.onClick.AddListener(OpenModalListMusic);

        sliderMusic.onValueChanged.AddListener(ChangeMusicVolume);
        sliderSFX.onValueChanged.AddListener(ChangeSFXVolume);

      
       
    }

    private void OnDestroy()
    {
        btnPlay.onClick.RemoveAllListeners();
        btnNextMs.onClick.RemoveAllListeners();
        btnPreviousMs.onClick.RemoveAllListeners();

        btnRandomMs.onClick.RemoveAllListeners();
        btnLoop.onClick.RemoveAllListeners();
        btnListMusic.onClick.RemoveAllListeners();

        sliderMusic.onValueChanged.RemoveAllListeners();
        sliderSFX.onValueChanged.RemoveAllListeners();
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

        textNameMusic.text = GetDataMusic(musicEnum).name;

        SoundManager.StopAllMusic();
		SoundManager.PlayMusic(musicEnum);
		StartCoroutine(TimmingNextMusic());
/*
        if (_coroutineCurrentMusic != null)
        {
            StopCoroutine(_coroutineCurrentMusic);
        }

        _coroutineCurrentMusic = StartCoroutine(TimmingNextMusic());*/

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
    private MusicEnum ValidateMusic(BahaviorMS behavior = BahaviorMS.Next)
    {
        MusicEnum musicEnum = currentMusic;
        switch (behavior)
        {
            case BahaviorMS.Next:
			case BahaviorMS.OnClickNext:
				musicEnum = SoundManager.GetNextMusicEnum(currentMusic);
                break;
            case BahaviorMS.OnClickPrevious:
                musicEnum = SoundManager.GetPreviousMusicEnum(currentMusic);
                break;
            case BahaviorMS.Current:
                //else is current
                break;
            default:
                break;
        }
        if (_toggleLoopMusic && behavior == BahaviorMS.Next)
        {
            musicEnum = currentMusic;
        }
        else if (_toggleRandomMusic && behavior != BahaviorMS.Current)
        {
            musicEnum = SoundManager.GetRandomMusic();
        }
		return musicEnum;
    }
	private Coroutine pauseCoroutine;
    private IEnumerator TimmingNextMusic()
    {
        MusicEnum _currentMusic = currentMusic;
        float timeMusic = SoundManager.GetMusicLength(_currentMusic);
		if (pauseCoroutine != null)
		{
			StopCoroutine(pauseCoroutine);
			pauseCoroutine = null;
		}
		pauseCoroutine = StartCoroutine(PauseWhileWaiting(timeMusic + 2f));
		yield return pauseCoroutine;
		PlayMusic(); // Play next music
    }
	IEnumerator PauseWhileWaiting(float seconds)
	{
		float timePassed = 0f;
		
		while (timePassed < seconds)
		{
			if (!TogglePauseMusic)
			{
				timePassed += Time.deltaTime; 
			}
			yield return null;  
		}
		pauseCoroutine = null;
	}

	#endregion

	#region Support

	#endregion

	#region Event

	private void togglePauseMusic()
    {
        if (!TogglePauseMusic)
        {
         /*   StopCoroutine(_coroutineCurrentMusic);
            _coroutineCurrentMusic = null;*/
            SoundManager.PauseMusic(currentMusic);

            spinePause.AnimationState.SetAnimation(0, "PlayStop - Active2", false);
            StartCoroutine(Common.IeDoSomeThing(0.25f, () =>
            {
                spinePause.AnimationState.SetAnimation(0, "PlayStop - Idle", false);
            }));
        }

        else
        {
			SoundManager.ContinueMusic(currentMusic);
			spinePause.AnimationState.SetAnimation(0, "PlayStop - Active", false);
            StartCoroutine(Common.IeDoSomeThing(0.25f, () =>
            {
                spinePause.AnimationState.SetAnimation(0, "PlayStop - Idle Active", false);
            }));
        }
        TogglePauseMusic = !TogglePauseMusic;
    }

    private void nextMusic()
    {
		
        PlayMusic();
        TogglePauseMusic = false;
        spinePause.AnimationState.SetAnimation(0, "PlayStop - Idle Active", false);
        spineNext.AnimationState.SetAnimation(0, "PlayNext - Active", false);
    }
    private void OnClickPreviousMusic()
    {
        PlayMusic(BahaviorMS.OnClickPrevious);
        
        TogglePauseMusic = false;
        spinePause.AnimationState.SetAnimation(0, "PlayStop - Idle Active", false);
        spinePrevious.AnimationState.SetAnimation(0, "PlayBack - Active", false);
    }
	private void OnClicknextMusic()
	{

		PlayMusic(BahaviorMS.OnClickNext);
		TogglePauseMusic = false;
		spinePause.AnimationState.SetAnimation(0, "PlayStop - Idle Active", false);
		spineNext.AnimationState.SetAnimation(0, "PlayNext - Active", false);
	}
	private void toggleloopMusic()
    {      
        ToggleLoopMusic = !ToggleLoopMusic;
    }

    private void toggleRandomMusic()
    {
     
        ToggleRandomMusic = !ToggleRandomMusic;
    }

    private void ChangeMusicVolume(float value)
    {

        if (value > 0 && !_toggleMusic)
        {
            _toggleMusic = true;
            btnMusic.SetState(ButtonState.Default); // Sound on state
        }
        else if (value == 0 && _toggleMusic)
        {
            _toggleMusic = false;
            btnMusic.SetState(ButtonState.Click); // Muted state
        }
        SoundManager.GlobalMusicVolume = value;
    }

    private void ChangeSFXVolume(float value)
    {
        if (value > 0 && !_togglesound)
        {
            _togglesound = true;
            btnSound.SetState(ButtonState.Default); // Sound on state
        }
        else if (value == 0 && _togglesound)
        {
            _togglesound = false;
            btnSound.SetState(ButtonState.Click); // Muted state
        }
        SoundManager.GlobalSoundVolume = value;
    }
    public void ToggleMusic()
    {
        _toggleMusic = !_toggleMusic;
        if (!_toggleMusic)
        {
            
            ChangeMusicVolume(0);
            sliderMusic.value = 0;
            btnMusic.SetState(ButtonState.Click);
        }
        else
        {
            
            ChangeMusicVolume(1);
            sliderMusic.value = 1;
            btnMusic.SetState(ButtonState.Default);
        }
        
    }
    public void ToggleSound()
    {
        _togglesound = !_togglesound;
        if (!_togglesound)
        {
            
            ChangeSFXVolume(0);
            sliderSFX.value = 0;
            btnSound.SetState(ButtonState.Click);
        }
        else
        {
           
            ChangeSFXVolume(1);
            sliderSFX.value = 1;
            btnSound.SetState(ButtonState.Default);
        }
    }
    private void OpenModalListMusic()
    {
        PlayList currentPlayList= Instantiate(_playList.gameObject, this.transform).GetComponent<PlayList>();

        Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;

        currentPlayList.gameObject.transform.localPosition= new Vector2(0, posCam.y - 2000); //Under Screen

        currentPlayList.gameObject.transform.DOLocalMoveY(0, 0.6f).SetEase(Ease.OutQuart);

        currentPlayList.InitData((string enumSong) =>
        {
            PlayMusic(forceMusic: enumSong);
            TogglePauseMusic = false;
            spinePause.AnimationState.SetAnimation(0, "PlayStop - Idle Active", false);
        });
    }
    #endregion
    #region AnimateUI
    [Button]
    public void FadeInContainer()
    {
        Container.SetActive(true);
        Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
        Debug.Log("khoaa:" + posCam);
        Container.transform.localPosition = new Vector2(0, posCam.y-2000); //Under Screen
        Container.transform.DOLocalMoveY(0, 0.6f).SetEase(Ease.OutQuart);
        

    }
    [Button]
    public void FadeOutContainer()
    {
        Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
        Container.transform.DOLocalMoveY(posCam.y+2000f, 0.5f).SetEase(Ease.InQuart).OnComplete(() =>
        {
            Container.SetActive(false);
        });
       
    }
    #endregion
}
