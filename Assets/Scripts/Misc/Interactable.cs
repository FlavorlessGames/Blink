using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float Range { get { return _range; } }
    [SerializeField] private float _range = 10f;
    public event GenericHandler InteractEvent;
    public event GenericHandler OnHoverEvent;
    public event GenericHandler EndHoverEvent;

    public void Hover()
    {
        OnHoverEvent?.Invoke();
    }

    public void Interact()
    {
        InteractEvent?.Invoke();
    }

    public void EndHover()
    {
        EndHoverEvent?.Invoke();
    }

    public delegate void GenericHandler();
}
