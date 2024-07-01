using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string Name;
    public AudioClip Clip;
    [Range(0f, 1f)]
    public float Volume = .5f;
    [Range(.1f, 3f)]
    public float Pitch = 1f;

    public AudioSource source { get { return _source; } set { _source = value; } }
    private AudioSource _source;
    public bool ConstantlyPlay;
}


[System.Serializable]
public class PositionalSound : Sound
{
    [Range(0f, 1f)]
    public float SpatialBlend = 1f;
    public int MinDistance = 1;
    public int MaxDistance = 500;
}