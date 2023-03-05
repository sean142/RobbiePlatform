using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public GameObject explosionVFXPrefab;
    int player;

    public LayerMask groundLayer;

    void Start()
    {
        player = LayerMask.NameToLayer("Player");

        GameManager.RegisterOrb(this);
    }
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == player)
        {
            Instantiate(explosionVFXPrefab, transform.position, transform.rotation);
            AudioManager.PlayerOrbAudio();
            gameObject.SetActive(false);
            GameManager.PlayerGrabbeOrb(this);
        }
    }
}
