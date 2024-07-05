using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    public bool IsPaused { get; private set; }

    [SerializeField] private GameObject _pauseMenu;
    void Update()
    {
        pause();
    }

    private void pause()
    {
        if (Input.GetButtonDown("Pause")) togglePause();
    }

    private void togglePause()
    {
        if (IsPaused)
        {
            UnPause();
            return;
        }
        Pause();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else Instance = this;
    }

    public void Pause()
    {
        IsPaused = true;
        _pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        AudioListener.pause = true;
    }

    public void UnPause()
    {
        IsPaused = false;
        _pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        AudioListener.pause = false;
    }
}
