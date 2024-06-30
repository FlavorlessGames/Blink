using UnityEngine.Audio;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;

public class GlobalAudio : NetworkBehaviour
{
    public static GlobalAudio Instance;
    [SerializeField] private List<Sound> _sounds;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        foreach (Sound s in _sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;
            s.source.volume = s.Volume;
            s.source.pitch = s.Pitch;
        }
    }

    public void LocalPlay(string name)
    {
        Sound sound = _sounds.Find(s => s.Name == name);
        if (sound == null)
        {
            Debug.LogError(string.Format("Sound: '{0}' not found in Global Audio"));
            return;
        }
        sound.source.Play();
    }

    public void Play(string name)
    {
        PlayRPC(name);
    }

    [Rpc(SendTo.Everyone)]
    public void PlayRPC(string name)
    {
        Sound sound = _sounds.Find(s => s.Name == name);
        if (sound == null)
        {
            Debug.LogError(string.Format("Sound: '{0}' not found in Global Audio"));
            return;
        }
        sound.source.Play();
    }
}