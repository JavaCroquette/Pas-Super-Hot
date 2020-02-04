using UnityEngine;
using UnityEngine.SceneManagement;


public class Breakbackgroundscript : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel = default;
    
    public void Restart()
    {
        pausePanel.SetActive(false);
        SceneManager.LoadScene("Merge1");
        Time.timeScale = 1;
    }

    public void Quit()
    {
        // Build to run only
        pausePanel.SetActive(false);
        SceneManager.LoadScene("MenuScene");
        Time.timeScale = 1;
    }
}
