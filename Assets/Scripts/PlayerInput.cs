﻿using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        player.SetDirectionalInput(directionalInput);

        if (Input.GetButtonDown("Jump"))
        {
            player.OnJumpInputDown();
        }

        if (Input.GetButtonUp("Jump"))
        {
            player.OnJumpInputUp();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            player.OnDashInputDown();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            player.OnDashInputUp();
        }

        if (Input.GetButtonDown("Fire3"))
        {
            //todo
        }

        if (Input.GetButtonUp("Fire3"))
        {
            //todo
        }
    }
}
