using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using fgames.Menus;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    public bool IsPaused { get; private set; }
    [SerializeField] private PauseUI _pauseUI;

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
        // _pauseMenu.SetActive(true);
        _pauseUI.Show();
        Cursor.lockState = CursorLockMode.None;
        AudioListener.pause = true;
    }

    public void UnPause()
    {
        IsPaused = false;
        // _pauseMenu.SetActive(false);
        _pauseUI.Hide();
        Cursor.lockState = CursorLockMode.Locked;
        AudioListener.pause = false;
    }
}
