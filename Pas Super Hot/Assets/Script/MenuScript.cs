using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuScript : MonoBehaviour
{
    // Start is called before the first frame update

    public void Play()
    {
        SceneManager.LoadScene("PlayTestScene");
    }

    public void Quit()
    {
        // Build to run only
        Application.Quit();
    }

   

   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
