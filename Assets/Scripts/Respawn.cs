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
    public int nbCoin;
    public Text nbCoinText;
    public Text nbDeathText;

    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;
        death = GetComponents<AudioSource>()[1];
        Instantiate(playerPrefab, startPlayerPos.position, Quaternion.identity);
        gameOver.SetActive(false);
        nbDeathText.text = nbDeath.ToString();
        nbCoinText.text = nbCoin.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        nbDeathText.text = nbDeath.ToString();
        nbCoinText.text = nbCoin.ToString();
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
