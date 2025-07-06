using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public enum LevelAudioType
    {
        Forest, Cave
    }
    public class AudioManager
    {
        private static AudioSource Source;
        private static LevelAudioType Type;
        private static AudioClip[] Clips;
        public static void PlayMusic(LevelAudioType type = LevelAudioType.Forest)
        {
            if (Type == type)
                return;

            if (Source == null)  
            {
                FindBootstrap();
            }
            if(Clips == null || Clips.Length == 0)
            {
                LoadClips();
            }
            Source.clip = type == LevelAudioType.Forest ? Clips[0] : Clips[1];
            Source.Play();
            Type = type;
        }

        private static void FindBootstrap()
        {
            Source = GameObject.FindFirstObjectByType<Bootstrap>().GetComponent<AudioSource>();
        }
        private static void LoadClips()
        {
            Clips = new AudioClip[2];
            Clips[0] = Resources.Load<AudioClip>("Forest");
            Clips[1] = Resources.Load<AudioClip>("Cave");
        }
    }
}
