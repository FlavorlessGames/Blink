using UnityEngine;

public class MusicManager : MonoBehaviour
{
    void Start()
    {
        GlobalAudio.Instance.LocalPlay("Siren Song");
    }
}