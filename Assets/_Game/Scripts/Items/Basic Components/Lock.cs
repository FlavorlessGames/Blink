using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Lock : MonoBehaviour
{
    public event LockHandler LockOpen;
    public int Index;
    void Start()
    {
        Interactable i = GetComponent<Interactable>();
        i.InteractEvent += lockOpen;
    }

    private void lockOpen(PlayerInteraction player)
    {
        LockOpen?.Invoke(this);
    }

    public delegate void LockHandler(Lock l);
}