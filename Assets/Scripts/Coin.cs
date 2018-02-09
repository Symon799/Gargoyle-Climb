using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    private Respawn game;

    // Use this for initialization
    void Start () {
        game = GameObject.FindGameObjectWithTag("Game").GetComponent<Respawn>() as Respawn;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            AudioSource[] audios = GetComponents<AudioSource>();
            audios[4].Play();
            game.nbCoin++;
            Destroy(collision.gameObject);
        }
    }
}
