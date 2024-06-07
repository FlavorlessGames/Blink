using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Door : NetworkBehaviour
{
    [SerializeField] private bool _open;
    [Range(0.1f, 5f)]
    [SerializeField] private float _animationTime = 0.5f;
    [SerializeField] private Transform _openPosition;
    [SerializeField] private Transform _closedPosition;
    [SerializeField] private GameObject _door;
    private float _timer;
    private Coroutine _lerpCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        _timer = _animationTime;
    }
    
    public void Open()
    {
        if (_open) return;
        OpenClose();
    }

    public void Close()
    {
        if (!_open) return;
        OpenClose();
    }

    public void OpenClose()
    {
        TransformData tdata= new TransformData(_open ? _closedPosition : _openPosition);
        float duration = _timer < _animationTime ? _timer : _animationTime;

        if (_lerpCoroutine != null) StopCoroutine(_lerpCoroutine);

        _lerpCoroutine = StartCoroutine(lerpTransform(new TransformData(_door.transform), tdata, duration));
        _open = !_open;
    }

    private IEnumerator lerpTransform(TransformData from, TransformData to, float duration)
    {
        _timer = 0f;
        float t = 0f;
        while (_timer < duration)
        {
            _timer += Time.deltaTime;
            t = _timer / duration;

            float posx = Mathf.Lerp(from.position.x, to.position.x, t);
            float posy = Mathf.Lerp(from.position.y, to.position.y, t);
            float posz = Mathf.Lerp(from.position.z, to.position.z, t);
            float w = Mathf.Lerp(from.rotation.w, to.rotation.w, t);
            float x = Mathf.Lerp(from.rotation.x, to.rotation.x, t);
            float y = Mathf.Lerp(from.rotation.y, to.rotation.y, t);
            float z = Mathf.Lerp(from.rotation.z, to.rotation.z, t);

            var position = new Vector3(posx, posy, posz);
            var rotation = new Quaternion(x, y, z, w);
            setTransformRpc(position, rotation);

            yield return null;
        }
    }

    [Rpc(SendTo.Server)]
    private void setTransformRpc(Vector3 position, Quaternion rotation)
    {
        _door.transform.position = position;
        _door.transform.rotation = rotation;
    }


    private struct TransformData
    {
        public Vector3 position;
        public Quaternion rotation;

        public TransformData(Transform t)
        {
            position = t.position;
            rotation = t.rotation;
        }
    }
}
