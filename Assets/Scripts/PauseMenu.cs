using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;

    private bool _isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        _isPaused = false;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        _isPaused = true;
    }

    public void Continue()
    {
        Resume();
    }

    public void QuitApp()
    {
        Application.Quit();
        Debug.Log("Application has quit.");
    }
}
