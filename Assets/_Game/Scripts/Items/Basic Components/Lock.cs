using UnityEngine;

public class Lock : MonoBehaviour
{
    public event LockHandler LockOpen;
    public int Index;

    public void Open()
    {
        LockOpen?.Invoke(this);
    }

    public delegate void LockHandler(Lock l);
}