using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Breakbackgroundscript : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    
    

    public void restart()
    {
        pausePanel.SetActive(false);
        SceneManager.LoadScene("PlayTestScene");
        Time.timeScale = 1;
    }

    public void quit()
    {
        // Build to run only
        pausePanel.SetActive(false);
        SceneManager.LoadScene("MenuScene");
        Time.timeScale = 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
