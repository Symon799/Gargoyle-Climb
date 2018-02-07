using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour {

    private Respawn game;
    public GameObject deathParticle;

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
            GameObject particle = Instantiate(deathParticle, transform.position, Quaternion.identity);
            Destroy(particle, 1.0f);
            Destroy(this.gameObject);
            game.setDead();
        }
    }

}
