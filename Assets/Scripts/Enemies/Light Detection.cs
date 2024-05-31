using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetection : MonoBehaviour
{
    [SerializeField] private const float _waitTime = .05f;
    public event GenericHandler SpottedEvent;
    public event GenericHandler EyesAvertedEvent;
    private float _timer;
    
    void Update()
    {
        wait();
    }

    public void Spotted()
    {
        _timer = _waitTime;
        SpottedEvent?.Invoke();
    }

    private void wait()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            return;
        }
        EyesAvertedEvent?.Invoke();
    }

    public delegate void GenericHandler();
}
