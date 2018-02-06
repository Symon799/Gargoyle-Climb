﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform startPlayerPos;
    private bool dead = false;

    // Use this for initialization
    void Start()
    {
        Instantiate(playerPrefab, startPlayerPos.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (dead && Input.GetButtonDown("Reset"))
        {
            Instantiate(playerPrefab, startPlayerPos.position, Quaternion.identity);
            dead = false;
        }
    }

    public void setDead()
    {
        dead = true;
    }

    public bool isDead()
    {
        return dead;
    }


}
