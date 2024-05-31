using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;

public class PlayerInteraction : NetworkBehaviour
{
    [SerializeField] 
    private GameObject _camera;
    private Camera _cam;
    private Vector3 _screenCenter;
    private Interactable _current;
    private Dictionary<Interactable, float> _tracking;
    private const float _hoverTime = 0.05f;
    private bool _disabled = false;
    // Start is called before the first frame update
    void Start()
    {
        _cam = _camera.GetComponent<Camera>();
        _screenCenter = new Vector3(Screen.width/2, Screen.height/2, 0);
        _tracking = new Dictionary<Interactable, float>();
    }

    public override void OnNetworkSpawn() 
    {
        if (!IsOwner) _disabled=true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_disabled) return;
        updateTracking();
        setInteractable();
        if (Input.GetButtonDown("Interact"))
        {
            interact();
        }
    }

    private void updateTracking()
    {
        List<Interactable> remove = new List<Interactable>();
        foreach (Interactable key in _tracking.Keys.ToList())
        {
            _tracking[key] -= Time.deltaTime;
            if (_tracking[key] < 0) key.EndHover();
            remove.Add(key);
        } 
        foreach (var toRemove in remove)
        {
            _tracking.Remove(toRemove);
        }
        _current = null;
    }

    private void setInteractable()
    {
        Ray ray = _cam.ScreenPointToRay(_screenCenter);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
            if (interactable != null && Utility.InRange(hit.point, transform.position, interactable.Range))
            {
                interactable.Hover();
                _current = interactable;
                _tracking[interactable] = _hoverTime;
                return;
            }
        }
    }

    private void interact()
    {
        if (_current == null) return;
        _current.Interact();
    }
}
