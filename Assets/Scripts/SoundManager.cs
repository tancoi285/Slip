using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private IEnumerable<AudioSource> _rs;
    private List<AudioSource> _lsAS = new List<AudioSource>();
    private Dictionary<Sound, AudioClip> _dicSound = new Dictionary<Sound, AudioClip>();

    void Start()
    {
        _rs = _lsAS.Where(x => !x.isPlaying);
        AddSound();
     //   print(Math.Pow(88, 17) % 247);
      //  print(Mathf.Pow(88, 17) % 247);
    }

    AudioSource GetAudioSource()
    {
        AudioSource newAS = _rs.FirstOrDefault();
        if (!newAS)
        {
            newAS = gameObject.AddComponent<AudioSource>();
            newAS.volume = 0.5f;
            _lsAS.Add(newAS);
        }
        return newAS;
    }

    void AddSound()
    {
        foreach (Sound sound in Enum.GetValues(typeof(Sound)))
            _dicSound.Add(sound, Resources.Load<AudioClip>("Audios/" + sound.ToString()));
    }

    public void Play(Sound sound)
    {
        AudioSource AS = GetAudioSource();
        AS.clip = _dicSound[sound];
        if (AS.clip == _dicSound[Sound.Main])
            AS.loop = true;
        AS.Play();
    }

    public void StopLoop()
    {
        foreach (AudioSource source in transform.GetComponents(typeof(AudioSource)))
            if (source.loop)
            {
                source.loop = false;
                source.Pause();
            }
    }

    public enum Sound
    {
        Clear, Click, CollectCoin, Main
    }
}
