using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float Range { get { return _range; } }
    [SerializeField] private float _range = 10f;
    public event InteractionHandler InteractEvent;
    public event InteractionHandler OnHoverEvent;
    public event InteractionHandler EndHoverEvent;

    public void Hover(PlayerInteraction player)
    {
        OnHoverEvent?.Invoke(player);
    }

    public void Interact(PlayerInteraction player)
    {
        InteractEvent?.Invoke(player);
    }

    public void EndHover(PlayerInteraction player)
    {
        EndHoverEvent?.Invoke(player);
    }

    public delegate void InteractionHandler(PlayerInteraction player);
}
