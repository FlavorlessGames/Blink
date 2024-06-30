using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetection : MonoBehaviour
{
    [SerializeField] private float _waitTime = .1f;
    public event GenericHandler SpottedEvent;
    public event GenericHandler EyesAvertedEvent;
    private float _timer;
    
    void Update()
    {
        wait();
    }

    public void Spotted()
    {
        SpottedEvent?.Invoke();
        _timer = _waitTime;
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
