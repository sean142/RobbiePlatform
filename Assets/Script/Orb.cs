using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public GameObject explosionVFXPrefab;
    int player;

    public LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        player = LayerMask.NameToLayer("Player");

        GameManager.RegisterOrb(this);
    }

    // Update is called once per frame
    void Update()
    {
        
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
