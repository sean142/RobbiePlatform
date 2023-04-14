using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinZone : MonoBehaviour
{
    public int playerLayer;
    public float restartTimer;
    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
            Debug.Log("Player Won");
        PlayerWon();
    }

    public  void PlayerWon()
    {
       // GameManager.instance.gameIsOver = true;
        PlayerController.instance.canMove = false;
        AudioManager.instance.playerSource.mute = true;
        DisplayGameOver();
        AudioManager.PlayerWonAudio();
        Invoke("Restart", restartTimer);
    }

    public void DisplayGameOver()
    {
        UIManager.instance.gameoverText.enabled = true;
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
        UIManager.instance.gameoverText.enabled = false;
        UIManager.instance.orbText.gameObject.SetActive(false);
        UIManager.instance.timeText.gameObject.SetActive(false);
        UIManager.instance.deathtText.gameObject.SetActive(false);
        UIManager.instance.orb.SetActive(false);
        UIManager.instance.time.SetActive(false);
        UIManager.instance.death.SetActive(false);
    }
}
