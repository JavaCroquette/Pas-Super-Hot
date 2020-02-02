using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject pausePanel;
    
    private bool is_paused;
    public void pause()
    {
        if (is_paused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            is_paused = false;
            pausePanel.SetActive(false);
            Time.timeScale = 1;
            return;
        }
        Time.timeScale = 0;
        is_paused = true;
        pausePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        return;
    }

    public void resume()
    {
        pause();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            pause();
        }
    }
}
