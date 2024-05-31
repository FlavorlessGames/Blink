using UnityEngine;

public class Rotator : MonoBehaviour 
{
    [SerializeField] private float _speed = 15f;
    [SerializeField] private Vector3 _rotation = new Vector3(0, 1, 0);
    [SerializeField] private bool _spinning = false;

    void Update() {
        if (!_spinning) return;
        transform.Rotate(_rotation * _speed * Time.deltaTime);
    }

    public void ToggleSpin()
    {
        _spinning = !_spinning;
    }

    public void Spin()
    {
        _spinning = true;
    }

    public void StopSpinning()
    {
        _spinning = false;
    }
}