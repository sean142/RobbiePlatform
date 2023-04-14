using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void PlayGame()
    {    
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameManager.instance.deathNum = 0;
        UIManager.UpdateDeathUI(GameManager.instance.deathNum);
        GameManager.instance.gameTime = Time.deltaTime;
        UIManager.instance.orbText.gameObject.SetActive(true);
        UIManager.instance.timeText.gameObject.SetActive(true);
        UIManager.instance.deathtText.gameObject.SetActive(true);
        UIManager.instance.orb.SetActive(true);
        UIManager.instance.time.SetActive(true);
        UIManager.instance.death.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
