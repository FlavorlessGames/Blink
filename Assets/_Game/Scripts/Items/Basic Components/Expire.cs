using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expire : MonoBehaviour
{
    [SerializeField] private float _duration = 1f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_duration < 0f) Destroy(gameObject);
        _duration -= Time.deltaTime;
    }
}
