using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    ScenceFader fader;
    List<Orb> orbs;
    Door lockedDoor;

    public float gameTime;
 //   public bool gameIsOver;
    bool canTrans;
    public int deathNum;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        orbs = new List<Orb>();

        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        Debug.Log("GameManager instance: " + instance);
    }

    private void Update()
    {
       // if (gameIsOver)
           // return;
       // if (canTrans)
         //  return;

        gameTime += Time.deltaTime;
        UIManager.UpdateTimeUI(gameTime);
    }

    public static void RegisterDoor(Door door)
    {
        instance.lockedDoor = door;
    }

    public static void RegisterScenceFader(ScenceFader obj)
    {
        instance.fader = obj;
    }

    public static void RegisterOrb(Orb orb)
    {
        if (instance == null)
            return;
        if (!instance.orbs.Contains(orb))
            instance.orbs.Add(orb);
        UIManager.UpdateOrbUI(instance.orbs.Count);
    }

    public static void PlayerGrabbeOrb(Orb orb)
    {
        if (!instance.orbs.Contains(orb))
            return;
        instance.orbs.Remove(orb);

        if (instance.orbs.Count == 0)
            instance.lockedDoor.Open();
        UIManager.UpdateOrbUI(instance.orbs.Count);
    }/*
    public static void PlayerWon()
    {
        instance.gameIsOver = true;
        UIManager.DisplayGameOver();
        AudioManager.PlayerWonAudio();
        Time.timeScale = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-2);
    }*/
    /*
    public static bool GameOver()
    {
        return instance.gameIsOver;
    }*/

    public static void PlayerDied()
    {
        instance.fader.FadeOut();
        instance.deathNum++;
        UIManager.UpdateDeathUI(instance.deathNum);
        instance.Invoke("RestartScene", 1.5f);
    }

    void RestartScene()
    {
        instance.orbs.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void NextScene()
    {
        instance.canTrans = true;
        PlayerController.instance.canMove = false;
        AudioManager.PlayerWonAudio();
        //AudioManager.PlayerNextSceneAudio();
        AudioManager.instance.playerSource.mute = true;
        instance.Invoke("TONextScene",2f);
    }  

    void TONextScene()
    {
        AudioManager.instance.playerSource.mute = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
