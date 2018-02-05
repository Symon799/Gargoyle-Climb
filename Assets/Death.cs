using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour {

    private Respawn game;

    // Use this for initialization
    void Start()
    {
        game = GameObject.FindGameObjectWithTag("Game").GetComponent<Respawn>() as Respawn;
    }

    // Update is called once per frame
    void Update ()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Death")
        {
            Debug.Log("DEATH");
            Destroy(this.gameObject);
            game.setDead();
        }
    }

}
