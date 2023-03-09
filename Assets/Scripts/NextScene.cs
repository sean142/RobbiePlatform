using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    int playerLayer;
    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject.layer == playerLayer)
        {
            Debug.Log("Next Scene");
        }
        GameManager.NextScene();

    }
}
