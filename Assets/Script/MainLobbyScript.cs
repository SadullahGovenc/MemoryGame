using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainLobbyScript : MonoBehaviour
{
    public GameObject quitPanel;
    //Çoklu level var ise oyuncuyu kaldýðý yerden devam etmesini saðlamak için .playerpref ile kaydederiz.
    public void Quit()
    {
        quitPanel.SetActive(true);
        
    }
    private void Start()
    {
        if (Time.timeScale ==0)
        {
            Time.timeScale = 1;
        }
    }
    public void Answer(string answer)
    {

        if (answer =="Yes")
        {
            Application.Quit();
        }
        else
        {
            quitPanel.SetActive(false);
        }
        // quitPanel.SetActive(false);  // stop panelini kapatýyoruz.
        

    }

    public void GameStart()
    {
        SceneManager.LoadScene(1);
       // SceneManager.LoadScene(PlayerPrefs.GetInt("continue"));
    }
}
