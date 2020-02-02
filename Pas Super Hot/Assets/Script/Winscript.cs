using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winscript : MonoBehaviour
{
    public GameObject winPanel;
    public void Win()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        winPanel.SetActive(true);
    }

}
