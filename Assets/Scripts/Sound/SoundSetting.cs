using System;
using System.Collections;
using System.Collections.Generic;
using NOOD.Sound;
using UnityEngine;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

public class SoundSetting : MonoBehaviour
{
    
    
    [Header("DATA")] [SerializeField] private SOmusicBackground SoDataMusic;
 
    [Header("UI")] [SerializeField] private Button btnRandomMs;
    [SerializeField] private Button btnPreviousMs;
    [SerializeField] private Button btnNextMs;
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnLoop;

    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSFX;

    [Header("TOGGLE")]
    private bool _togglePauseMusic=false;
    private bool _toggleLoopMusic=false;
    private bool _toggleRandomMusic=false;
 //   [Header("VARIABLE")] 
    private MusicEnum currentMusic =>SoundManager.GetCurrentMusic() ;
  

    private IEnumerator ff()
    {
        yield return new WaitForSeconds(1f);

    }
    private void OnDestroy()
    {
        
    }
    
    

    #region Event

    private void togglePauseMusic()
    {
        if (!_togglePauseMusic) SoundManager.StopMusic(currentMusic);
        else  SoundManager.PlayMusic(currentMusic);
    }

    private void nextMusic()
    {
        SoundManager.StopAllMusic();
        SoundManager.PlayMusic(SoundManager.GetNextMusicEnum(currentMusic));
    }
    private void previousMusic()
    {
        SoundManager.StopAllMusic();
        SoundManager.PlayMusic(SoundManager.GetPreviousMusicEnum(currentMusic));
    }

    private void toggleloopMusic()
    {
        
    }

    private void toogleRandomMusic()
    {
        
    }
    #endregion
}
