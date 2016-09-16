using UnityEngine;
using System.Collections;

public class BlockScript : MonoBehaviour {

    public GameObject brickParticle;
	// Use this for initialization
	void Start ()
    {
        InitializeColor();
    }

    public void InitializeColor()
    {
        GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    }


    // Update is called once per frame
    void Update () {
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        GameObject particle = Instantiate(brickParticle, gameObject.transform.position, Quaternion.identity) as GameObject;
        particle.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
        gameObject.SetActive(false);
        GameManager.audio.Play();
        GameManager.Score += 20;
        GameManager.BlocksAlive--;
    }
}
