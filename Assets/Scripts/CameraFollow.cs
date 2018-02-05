using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform player;
    public Transform startPos;

	// Use this for initialization
	void Start ()
    {}

    // Update is called once per frame
    void Update ()
    {
        float x = player.position.x;
        if (startPos.position.x > x)
            x = startPos.position.x;
        
        float y = player.position.y;
        if (startPos.position.y > y)
            y = startPos.position.y;
        
        transform.position = new Vector3(x, y, -5);
    }
}
