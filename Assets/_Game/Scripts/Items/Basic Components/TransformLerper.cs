using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// using Unity.Netcode;

public class TransformLerper : MonoBehaviour 
{
    private Coroutine _lerpCoroutine;
    private Dictionary<GameObject, Coroutine> _routines = new ();

    public void LerpTo(GameObject go, Transform destination, float duration)
    {
        TransformData from = new TransformData(go.transform);
        TransformData to = new TransformData(destination);

        if (_routines.ContainsKey(go))
        {
            StopCoroutine(_routines[go]);
        }

        _routines[go] = StartCoroutine(lerpTransform(go, from, to, duration));
    }
    
    private IEnumerator lerpTransform(GameObject go, TransformData from, TransformData to, float duration)
    {
        float _timer = 0f;
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
            setTransformRpc(go, position, rotation);

            yield return null;
        }
    }

    private void setTransformRpc(GameObject go, Vector3 position, Quaternion rotation)
    {
        go.transform.position = position;
        go.transform.rotation = rotation;
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