using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraSizeController : MonoBehaviour {

    public float scaleUpdater;
    public float sizeLimit;
    public int speedRotate = 20;

    private bool isFadeOut = false;
    private float startTime;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!isFadeOut && transform.localScale.x < sizeLimit)
            transform.localScale += new Vector3(scaleUpdater, scaleUpdater, 0);

        transform.Rotate(Vector3.forward * speedRotate * Time.deltaTime);

        if (isFadeOut)
        {
            float t = (Time.time - startTime) / 0.15f;
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1, 0, t));
        }
    }

    public void launchFadeOut()
    {
        startTime = Time.time;
        isFadeOut = true;
    }
}
