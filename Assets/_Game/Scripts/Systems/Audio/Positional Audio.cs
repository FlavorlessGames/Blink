using UnityEngine.Audio;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;

public class PositionalAudio : NetworkBehaviour
{
    [SerializeField] private List<PositionalSound> _sounds;
    private List<PositionalSound> _constants;

    void Awake()
    {
        _constants = new List<PositionalSound>();
        foreach (PositionalSound s in _sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;
            s.source.volume = s.Volume;
            s.source.pitch = s.Pitch;

            s.source.spatialBlend = s.SpatialBlend;
            s.source.minDistance = s.MinDistance;
            s.source.maxDistance = s.MaxDistance;
            if (s.ConstantlyPlay) _constants.Add(s);
        }
    }

    void Start()
    {
        foreach (PositionalSound s in _constants)
        {
            s.source.loop = true;
            s.source.Play();
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