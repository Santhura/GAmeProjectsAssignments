﻿using UnityEngine;
using System.Collections;

public class PaddleScript : MonoBehaviour
{
    public Transform LeftBlockTransform, RightBLockTransform;
    public float speed = 2;

    private AudioSource audio;
    public GameObject particlePaddle;
    // Use this for initialization
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float input;

        if (Input.touchCount > 0)//touch input
        {
            input = Input.touches[0].position.x >= Screen.width / 2 ? 1f : -1f;
        }
        else//keyboard input
        {
            input = Input.GetAxis("Horizontal");
        }

        if (input > 0.1f)
        {
            transform.Translate(new Vector2(input * speed * Time.deltaTime, 0));
        }
        else if(input < 0.1f)
        {
            transform.Translate(new Vector2(input * speed * Time.deltaTime, 0));
        }
        float currentX = Mathf.Clamp(transform.position.x, LeftBlockTransform.position.x + 1, RightBLockTransform.position.x - 1);
        transform.position = new Vector3(currentX, transform.position.y, transform.position.z);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Ball")
        {
            audio.Play();
            Instantiate(particlePaddle, transform.position, Quaternion.identity);
        }
    }
     
}
