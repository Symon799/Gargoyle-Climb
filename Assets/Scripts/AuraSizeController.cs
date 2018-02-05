using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraSizeController : MonoBehaviour {

    public float scaleUpdater;
    public float sizeLimit;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.localScale.x < sizeLimit)
            transform.localScale += new Vector3(scaleUpdater, scaleUpdater, 0);
	}
}
