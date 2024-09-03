using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NOOD.Sound
{
    public class SoundManager
    {
#region Object Init
        private static SoundDataSO soundData;
        private static GameObject soundManagerGlobal;
#endregion

#region List
        private static List<MusicPlayer> enableMusicPlayers;
        private static List<SoundPlayer> enableSoundPlayers;
        private static List<MusicPlayer> disableMusicPlayers;
        private static List<SoundPlayer> disableSoundPlayers;
#endregion

#region Parameter
        private static float globalSoundVolume;
        private static float globalMusicVolume;
        public static float GlobalSoundVolume
        {
            get
            {
                return globalSoundVolume;
            }
            set
            {
                globalSoundVolume = value;
                AppliedVolume();
            }
        }
        public static float GlobalMusicVolume
        {
            get
            {
                return globalMusicVolume;
            }
            set
            {
                globalMusicVolume = value;
                AppliedVolume();
            }
        }
#endregion

#region Init
        public static void FindSoundData()
        {
            SoundDataSO[] soundDataSOs = Resources.LoadAll<SoundDataSO>("");
            if(soundDataSOs.Length > 0)
                soundData = Resources.FindObjectsOfTypeAll<SoundDataSO>()[0];
            if(soundData == null)
                Debug.LogError("Can't find SoundData, please create one in Resources folder using Create -> SoundData");
            else
                Debug.Log("Load SoundData success");
        }

        private static void InitIfNeed()
        {
            if(soundManagerGlobal == null)
            {
                Debug.Log("SoundManager Init");
                soundManagerGlobal = new GameObject("SoundManagerGlobal");
                disableMusicPlayers = new List<MusicPlayer>();
                disableSoundPlayers = new List<SoundPlayer>();
                enableMusicPlayers = new List<MusicPlayer>();
                enableSoundPlayers = new List<SoundPlayer>();
                GlobalMusicVolume = 1;
                GlobalSoundVolume = 1;
                FindSoundData();
            }
        }
        public static void InitSoundManager()
        {
            InitIfNeed();
        }
#endregion

#region SoundRegion
        /// <summary>
        /// Play sound with your volume
        /// </summary>
        /// <param name="soundEnum"></param>
        /// <param name="volume"></param>
        public static void PlaySound(SoundEnum soundEnum, Vector3 position, float volume = 1)
        {
            InitIfNeed();
            if(soundData == null)
            {
                FindSoundData();
            }

            AudioSource soundAudioPayer;
            SoundPlayer soundPlayer;
            if (disableSoundPlayers.Any(x => x.soundType == soundEnum))
            {
                soundPlayer = disableSoundPlayers.First(x => x.soundType == soundEnum);
                soundAudioPayer = soundPlayer.GetComponent<AudioSource>();
                soundPlayer.gameObject.SetActive(true);

                // Remove when get
                disableSoundPlayers.Remove(soundPlayer);
            }
            else
            {
                GameObject newObj = new GameObject("SoundPlayer" + soundEnum.ToString());
                newObj.transform.SetParent(soundManagerGlobal.transform);
                soundPlayer = newObj.AddComponent<SoundPlayer>();
                soundPlayer.soundType = soundEnum;
                soundAudioPayer = newObj.AddComponent<AudioSource>();
            }
            AudioClip audioClip = soundData.soundDic.Dictionary[soundEnum.ToString()];

            soundPlayer.transform.position = position;
            soundAudioPayer.playOnAwake = false;
            soundAudioPayer.volume = volume;
            soundAudioPayer.clip = audioClip;
            soundAudioPayer.Play();
            Fade(soundAudioPayer, audioClip.length * 0.2f, 1); // Fade time = 20% of sound length
            enableSoundPlayers.Add(soundPlayer);

            // Add to list when end sound
            NoodyCustomCode.StartDelayFunction(() =>
            {
                if(soundAudioPayer != null)
                {
                    Fade(soundAudioPayer, audioClip.length * 0.2f, 0, onComplete: () =>
                    {
                        soundAudioPayer.gameObject.SetActive(false);
                        enableSoundPlayers.Remove(soundPlayer);
                        disableSoundPlayers.Add(soundPlayer);
                    });
                }
            }, audioClip.length - (audioClip.length * 0.2f)); // Start delay function after 80% time
        }
        /// <summary>
        /// Play sound with globalSoundVolume
        /// </summary>
        /// <param name="soundEnum"></param>
        public static void PlaySound(SoundEnum soundEnum)
        {
            PlaySound(soundEnum, Vector3.zero, GlobalSoundVolume);
        }
        public static void PlaySound(SoundEnum soundEnum, Vector3 position)
        {
            PlaySound(soundEnum, position, GlobalSoundVolume);
        }
        /// <summary>
        /// Stop all soundPlayers has the same soundEnum
        /// </summary>
        /// <param name="soundEnum"></param>
        public static void StopSound(SoundEnum soundEnum)
        {
            InitIfNeed();
            if(soundData == null)
            {
                FindSoundData();
            }
            SoundPlayer[] soundPlayerArray = GameObject.FindObjectsByType<SoundPlayer>(sortMode: FindObjectsSortMode.None).Where(x => x.soundType == soundEnum).ToArray();

            foreach(var soundPlayer in soundPlayerArray)
            {
                if(soundPlayer.isActiveAndEnabled)
                {
                    AudioSource soundAudioPlayer = soundPlayer.GetComponent<AudioSource>();
                    Fade(soundAudioPlayer, 0.2f, 0, onComplete: () =>
                    {
                        soundPlayer.gameObject.SetActive(false);
                        enableSoundPlayers.Remove(soundPlayer);
                        disableSoundPlayers.Add(soundPlayer);
                    });
                }
            }
        }
        /// <summary>
        /// Stop all soundPlayer found
        /// </summary>
        public static void StopAllSound()
        {
            InitIfNeed();
            if(soundData == null)
            {
                FindSoundData();
            }

            foreach(var soundPlayer in GameObject.FindObjectsOfType<SoundPlayer>())
            {
                if(soundPlayer.isActiveAndEnabled)
                {
                    soundPlayer.gameObject.SetActive(false);

                    enableSoundPlayers.Remove(soundPlayer);
                    disableSoundPlayers.Add(soundPlayer);
                }
            }
        }
        /// <summary>
        /// Get the sound length base on soundEnum (data from soundData)
        /// </summary>
        /// <param name="soundEnum"></param>
        /// <returns></returns>
        public static float GetSoundLength(SoundEnum soundEnum)
        {
            if(soundData == null)
            {
                FindSoundData();
            }
            return soundData.soundDic.Dictionary[soundEnum.ToString()].length;
        }
#endregion

#region MusicRegion
        /// <summary>
        /// Play music with new MusicPlayer gameObject and with your volume
        /// </summary>
        /// <param name="musicEnum"></param>
        public static void PlayMusic(MusicEnum musicEnum, float volume = 1, bool alwaysPlay = false,bool loop=false)
        {
            //  InitIfNeed();
   
            if (soundData == null)
            {
				Debug.Log("Sound Data null");
                FindSoundData();
            }

            if (enableMusicPlayers.Any(x => x.musicType == musicEnum)) return;

            MusicPlayer musicPlayer;
            if (disableMusicPlayers.Any(x => x.musicType == musicEnum))
            {
                musicPlayer = disableMusicPlayers.First(x => x.musicType == musicEnum);
                musicPlayer.gameObject.SetActive(true);
                disableMusicPlayers.Remove(musicPlayer);
            }
            else
            {
                GameObject newObj = new GameObject("MusicPlayer");
                newObj.transform.SetParent(soundManagerGlobal.transform);
                musicPlayer = newObj.AddComponent<MusicPlayer>();
            }

            musicPlayer.musicType = musicEnum;
            musicPlayer.isAlwaysPlay = alwaysPlay;
            enableMusicPlayers.Add(musicPlayer);

            AudioSource musicAudioSource = musicPlayer.GetComponent<AudioSource>();
            if (musicAudioSource == null)
            {
                musicAudioSource = musicPlayer.gameObject.AddComponent<AudioSource>();
            }

            AudioClip audioClip = soundData.musicDic.Dictionary[musicEnum.ToString()].audioClip;
            musicAudioSource.volume = volume;
            musicAudioSource.clip = audioClip;
            musicAudioSource.loop = loop;
            musicAudioSource.Play();
            Fade(musicAudioSource, 0.5f, globalMusicVolume);
        }
        /// <summary>
        /// Play music with globalMusicVolume
        /// </summary>
        /// <param name="musicEnum"></param>
        public static void PlayMusic(MusicEnum musicEnum)
        {
            PlayMusic(musicEnum, GlobalMusicVolume);
        }
        /// <summary>
        /// Change all music volumes that have the same musicEnum
        /// </summary>
        /// <param name="musicEnum"></param>
        /// <param name="volume"></param>
        public static void ChangeMusicVolume(MusicEnum musicEnum, float volume)
        {
            InitIfNeed();
            foreach(var musicPlayer in enableMusicPlayers)
            {
                if(musicPlayer.musicType == musicEnum)
                {
                    AudioSource audioSource = musicPlayer.GetComponent<AudioSource>();
                    audioSource.volume = volume;
                }
            }
        }
        /// <summary>
        /// Change music clip of audio source from sourceMusicEnum clip to toMusicEnum clip
        /// </summary>
        /// <param name="sourceMusicEnum"></param>
        /// <param name="toMusicEnum"></param>
        public static void ChangeMusic(MusicEnum sourceMusicEnum, MusicEnum toMusicEnum)

        {
            InitIfNeed();
            if(soundData == null)
            {
                FindSoundData();
            }

            MusicPlayer musicPlayer;
            AudioSource musicAudioSource;
            if(enableMusicPlayers.Any(x => x.musicType == sourceMusicEnum))
            {
                musicPlayer = enableMusicPlayers.First(x => x.musicType == sourceMusicEnum);
                musicAudioSource = musicPlayer.GetComponent<AudioSource>();
                Fade(musicAudioSource, 0.2f, 0, onComplete: () =>
                {
                    musicPlayer.musicType = toMusicEnum;
                    musicAudioSource.clip = soundData.musicDic.Dictionary[toMusicEnum.ToString()].audioClip;
                    Fade(musicAudioSource, 0.2f, 1);
                });
            }
            else
            {
                Debug.Log("No source music enum, just play to music enum instead");
                PlayMusic(toMusicEnum);
            }
        }

        public static void ChangeMusicAndVolume(MusicEnum sourceMusicEnum, MusicEnum toMusicEnum, float volume)
        {
            ChangeMusic(sourceMusicEnum, toMusicEnum);
            ChangeMusicVolume(toMusicEnum, volume);
        }
        #region Stop Music
        /// <summary>
        /// Stop all MusicPlayer with the same musicEnum
        /// </summary>
        /// <param name="musicEnum"></param>
        public static void StopMusic(MusicEnum musicEnum)
        {
            InitIfNeed();
            if(enableMusicPlayers.Any(x => x.musicType == musicEnum))
            {
                MusicPlayer musicPlayer =  enableMusicPlayers.First(x => x.musicType == musicEnum);

                AudioSource musicAudioSource = musicPlayer.GetComponent<AudioSource>();
                Fade(musicAudioSource, 0.5f, 0, onComplete: () =>
                {
                    musicAudioSource.Stop();
                    musicPlayer.gameObject.SetActive(false);
                    enableMusicPlayers.Remove(musicPlayer);
                    disableMusicPlayers.Add(musicPlayer);
                });
            } 
        }
        /// <summary>
        /// Resume MusicPlayer with the same musicEnum
        /// </summary>
        /// <param name="musicEnum"></param>
        public static void ResumeMusic(MusicEnum musicEnum)
        {
            InitIfNeed();
            if(enableMusicPlayers.Any(x => x.musicType == musicEnum))
            {
                MusicPlayer musicPlayer =  disableMusicPlayers.First(x => x.musicType == musicEnum);

                musicPlayer.gameObject.SetActive(true);
                musicPlayer.TryGetComponent<AudioSource>(out AudioSource audioSource);

                audioSource.Play();
                Fade(audioSource, 0.5f, GlobalMusicVolume);
                enableMusicPlayers.Add(musicPlayer);
                disableMusicPlayers.Remove(musicPlayer);
            } 
        }
        /// <summary>
        /// Stop all MusicPlayers found
        /// </summary>
        public static void StopAllMusic()
        {
         //  InitIfNeed();
            if(soundData == null)
            {
                FindSoundData();
            }
            for (int i = enableMusicPlayers.Count - 1; i >= 0; i--)
            {
                var musicPlayer = enableMusicPlayers[i];

                AudioSource audioSource = musicPlayer.GetComponent<AudioSource>();
                audioSource.Stop();
                musicPlayer.gameObject.SetActive(false);
                enableMusicPlayers.RemoveAt(i);
                disableMusicPlayers.Add(musicPlayer);
                            
            }
                
        }
        /// <summary>
        /// Resume all MusicPlayers found
        /// </summary>
        public static void ResumeAllMusic()
        {
            InitIfNeed();
            if(soundData == null)
            {
                FindSoundData();
            }
            foreach(var musicPlayer in disableMusicPlayers)
            {
                musicPlayer.TryGetComponent<AudioSource>(out AudioSource audioSource);

                audioSource.Play();
                audioSource.volume = GlobalMusicVolume;
                
                musicPlayer.gameObject.SetActive(true);
                enableMusicPlayers.Add(musicPlayer);
                disableMusicPlayers.Remove(musicPlayer);
            }
        }

		/// <summary>
		/// Pause all MusicPlayer with the same musicEnum
		/// </summary>
		/// <param name="musicEnum"></param>
		public static void PauseMusic(MusicEnum musicEnum)
		{
			InitIfNeed();

			if (enableMusicPlayers.Any(x => x.musicType == musicEnum))
			{
				MusicPlayer musicPlayer = enableMusicPlayers.First(x => x.musicType == musicEnum);
				AudioSource musicAudioSource = musicPlayer.GetComponent<AudioSource>();

				if (musicAudioSource.isPlaying)
				{
					musicAudioSource.Pause();
				}
			}
		}
		/// <summary>
		/// Continue playing the music that was paused for a specific musicEnum.
		/// </summary>
		/// <param name="musicEnum"></param>
		public static void ContinueMusic(MusicEnum musicEnum)
		{
			InitIfNeed();

			if (enableMusicPlayers.Any(x => x.musicType == musicEnum))
			{
				MusicPlayer musicPlayer = enableMusicPlayers.First(x => x.musicType == musicEnum);
				AudioSource musicAudioSource = musicPlayer.GetComponent<AudioSource>();

				if (musicAudioSource != null && musicAudioSource.isPlaying == false)
				{
					musicAudioSource.UnPause();
				}
			}
		}
		#endregion
		#endregion

		#region Support functions
		/// <summary>
		/// Fade in or out base on target volume
		/// </summary>
		/// <param name="audioSource"></param>
		/// <param name="duration"></param>
		/// <param name="targetVolume"></param>
		public static void Fade(AudioSource audioSource, float duration, float targetVolume, Action onComplete = null)
        {
            float currentTime = 0;
            float start = audioSource.volume;
            NoodyCustomCode.StartUpdater(audioSource, () =>
            {
                if (currentTime < duration)
                {
                    currentTime += Time.deltaTime;
                    audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                    return false;
                }
                else
                {
                    onComplete?.Invoke();
                    return true;
                }
            });
        }
        /// <summary>
        /// Get music length (data from SoundData)
        /// </summary>
        /// <param name="musicEnum"></param>
        /// <returns></returns>
        public static float GetMusicLength(MusicEnum musicEnum)
        {
            if(soundData == null)
            {
                FindSoundData();
            }
            return soundData.musicDic.Dictionary[musicEnum.ToString()].audioClip.length;
        }
        public static bool IsMusicPlaying(MusicEnum musicEnum)
        {
            InitIfNeed();
            return enableMusicPlayers.Any(x => x.musicType == musicEnum);
        }
        private static void AppliedVolume()
        {
            // To sound
            SoundPlayer[] soundPlayers = GameObject.FindObjectsByType<SoundPlayer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach(var sound in soundPlayers)
            {
                if(sound.TryGetComponent<AudioSource>(out AudioSource audioSource))
                {
                    audioSource.volume = globalSoundVolume;
                }
            }
            // To music
            MusicPlayer[] musicPlayers = GameObject.FindObjectsByType<MusicPlayer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach(var musicPlayer in musicPlayers)
            {
                AudioSource audioSource = musicPlayer.GetComponent<AudioSource>();
                audioSource.volume = globalMusicVolume;
            }
        }
        public static void RemoveMusicPlayer(MusicPlayer musicPlayer)
        {
            if (enableMusicPlayers.Contains(musicPlayer))
                enableMusicPlayers.Remove(musicPlayer);
            if (disableMusicPlayers.Contains(musicPlayer))
                disableMusicPlayers.Remove(musicPlayer);
        }
        public static void RemoveSoundPlayer(SoundPlayer soundPlayer)
        {
            if (enableSoundPlayers.Contains(soundPlayer))
                enableSoundPlayers.Remove(soundPlayer);
            if (disableSoundPlayers.Contains(soundPlayer))
                disableSoundPlayers.Remove(soundPlayer);
        }


        /// <summary>
        /// get current enum playing or return default music
        /// </summary>
        /// <returns></returns>
        public static MusicEnum GetCurrentMusic() 
        {
            if (enableMusicPlayers.Count > 0)
            {
                MusicEnum currentEnumMs = enableMusicPlayers[enableMusicPlayers.Count - 1].musicType;
                return currentEnumMs;
            }
            else
            {
                if(disableMusicPlayers.Count>0) return disableMusicPlayers[disableMusicPlayers.Count - 1].musicType;
                return GetRandomMusic();
            }
            Debug.LogError("enableMusicPlayers COUNT COUNT = 0");
            return 0;
            
        }
       public static MusicEnum GetRandomMusic()
        {
            var values = (MusicEnum[])Enum.GetValues(typeof(MusicEnum));
            var random = new System.Random();
            return values[random.Next(values.Length)];
        }
        public static MusicEnum GetNextMusicEnum(MusicEnum current)
        {
            // Get all enum values
            var values = (MusicEnum[])Enum.GetValues(typeof(MusicEnum));
            // Find the index of the current value
            int index = Array.IndexOf(values, current);
            // Get the next index, wrapping around if necessary
            int nextIndex = index + 1;
            nextIndex= nextIndex>=values.Count()? 0 : nextIndex;
            return values[nextIndex];
        }
        public static MusicEnum GetPreviousMusicEnum(MusicEnum current)
        {
            // Get all enum values
            var values = (MusicEnum[])Enum.GetValues(typeof(MusicEnum));
            // Find the index of the current value
            int index = Array.IndexOf(values, current)+1;
            int nextIndex = index - 1;
            nextIndex= nextIndex<=0? values.Count() : nextIndex;
            return values[nextIndex-1];
        }

        public static DataMusic GetDataMusic(MusicEnum current)
        {
            return soundData.musicDic.Dictionary[current.ToString()];
        }
        public static List<DataMusic> GetAllDataMusic()
        {
            if (soundData == null || soundData.musicDic == null)
            {
                Debug.LogError("Sound data or music dictionary is null.");
                return new List<DataMusic>();
            }

            return soundData.musicDic.Dictionary.Values.ToList();
        }
        public static Dictionary<string, DataMusic> GetAllDataMusicDictionary()
        {
            if (soundData == null || soundData.musicDic == null || soundData.musicDic.Dictionary == null)
            {
                return new Dictionary<string, DataMusic>();
            }

            return soundData.musicDic.Dictionary;
        }
        #endregion
    }

    public class SoundPlayer : MonoBehaviour 
    {
        public SoundEnum soundType;
        private void OnDestroy()
        {
            SoundManager.RemoveSoundPlayer(this);
        }
    }
    public class MusicPlayer : MonoBehaviour 
    {
        public MusicEnum musicType;
        public bool isAlwaysPlay;
        private void OnDestroy()
        {
            SoundManager.RemoveMusicPlayer(this);
        }
    }
    
   
}
