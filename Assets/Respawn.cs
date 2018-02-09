using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Respawn : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform startPlayerPos;
    public GameObject gameOver;
    private bool dead = false;
    AudioSource death;
    public int nbDeath;
    public Text nbDeathText;

    // Use this for initialization
    void Start()
    {
        death = GetComponents<AudioSource>()[1];
        Instantiate(playerPrefab, startPlayerPos.position, Quaternion.identity);
        gameOver.SetActive(false);
        nbDeathText.text = nbDeath.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        nbDeathText.text = nbDeath.ToString();
        if (dead && Input.GetButtonDown("Reset"))
        {
            Instantiate(playerPrefab, startPlayerPos.position, Quaternion.identity);
            gameOver.SetActive(false);
            dead = false;
        }
    }

    public void setDead()
    {
        if (!dead)
            nbDeath++;
        dead = true;
        death.Play();
        gameOver.SetActive(true);
    }

    public bool isDead()
    {
        return dead;
    }


}
