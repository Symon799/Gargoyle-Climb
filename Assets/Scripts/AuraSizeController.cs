using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraSizeController : MonoBehaviour {

    public float scaleUpdater;
    public float sizeLimit;
    public int speedRotate = 20;

    private bool isGrowing = true;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isGrowing && transform.localScale.x < sizeLimit)
            transform.localScale += new Vector3(scaleUpdater, scaleUpdater, 0);

        transform.Rotate(Vector3.forward * speedRotate * Time.deltaTime);
    }

    public void launchFadeOut()
    {
        isGrowing = false;
    }
}
