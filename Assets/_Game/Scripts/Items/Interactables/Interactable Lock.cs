using UnityEngine;

[RequireComponent(typeof(Lock))]
[RequireComponent(typeof(Interactable))]
public class InteractableLock : MonoBehaviour
{
    private Lock _lock;

    void Start()
    {
        _lock = GetComponent<Lock>();
        Interactable interactable = GetComponent<Interactable>();
        interactable.InteractEvent += lockOpen;
    }

    private void lockOpen(PlayerInteraction pi)
    {
        _lock.Open();
    }
}