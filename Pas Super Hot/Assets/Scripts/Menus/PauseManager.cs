using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel = default;

    private bool is_paused = false;

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Pause();
        }
    }

    private void Pause()
    {
        if (is_paused)
        {
            is_paused = false;
            Cursor.lockState = CursorLockMode.Locked;
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
            return;
        }
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        is_paused = true;
        return;
    }
}
