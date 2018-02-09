using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform startPlayerPos;
    public GameObject gameOver;
    private bool dead = false;
    AudioSource death;

    // Use this for initialization
    void Start()
    {
        death = GetComponents<AudioSource>()[1];
        Instantiate(playerPrefab, startPlayerPos.position, Quaternion.identity);
        gameOver.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (dead && Input.GetButtonDown("Reset"))
        {
            Instantiate(playerPrefab, startPlayerPos.position, Quaternion.identity);
            gameOver.SetActive(false);
            dead = false;
        }
    }

    public void setDead()
    {
        dead = true;
        death.Play();
        gameOver.SetActive(true);

    }

    public bool isDead()
    {
        return dead;
    }


}
