using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu (menuName = "ScriptableObjects/DataMusicBackground")]
public class SOmusicBackground : ScriptableObject
{
   [Header("Data")] public List<DataMusicBr> MusicBrs;
   private List<DataMusicBr> AllMusicBrs = new List<DataMusicBr>();
   public DataMusicBr currentMusic;

   private void Awake()
   {
      MusicBrs = GetUnlockedMusic();
   }
   private List<DataMusicBr> GetUnlockedMusic()
   {
      return AllMusicBrs.Where(m => !m.Lock).ToList();
   }
   public DataMusicBr GetRandomMusic()
   {
      if (MusicBrs == null || MusicBrs.Count == 0)
      {
         Debug.LogError("Music list is empty or null.");
         return null;
      }

      int randomIndex = Random.Range(0, MusicBrs.Count);
      return currentMusic=MusicBrs[randomIndex];
   }
   
   public DataMusicBr GetNextMusic()
   {
      if (MusicBrs == null || MusicBrs.Count == 0)
      {
         Debug.LogWarning("Music list is empty or null.");
         return null;
      }

      int currentIndex = MusicBrs.IndexOf(currentMusic);
      int nextIndex = (currentIndex + 1) % MusicBrs.Count;
      return currentMusic=MusicBrs[nextIndex];
   }
   public DataMusicBr GetPreviousMusic()
   {
      if (MusicBrs == null || MusicBrs.Count == 0)
      {
         Debug.LogWarning("Music list is empty or null.");
         return null;
      }

      int currentIndex = MusicBrs.IndexOf(currentMusic);
      int previousIndex = (currentIndex - 1 + MusicBrs.Count) % MusicBrs.Count;
      return currentMusic=MusicBrs[previousIndex];
   }
}
[System.Serializable]
public class DataMusicBr
{
   public int id;
   public string name;
   public AudioClip audioClip;
   public bool Lock=false;
}
