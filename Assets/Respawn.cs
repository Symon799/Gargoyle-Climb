using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform startPlayerPos;
    bool isDead = false;

    // Use this for initialization
    void Start()
    {
        Instantiate(playerPrefab, startPlayerPos.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead && Input.GetButtonDown("Reset"))
        {
            Instantiate(playerPrefab, startPlayerPos.position, Quaternion.identity);
            isDead = false;
        }
    }

    public void setDead()
    {
        isDead = true;
    }

    
}
