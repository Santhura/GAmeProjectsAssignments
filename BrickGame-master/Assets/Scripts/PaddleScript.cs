using UnityEngine;
using System.Collections;

public class PaddleScript : MonoBehaviour
{
    public Transform LeftBlockTransform, RightBLockTransform;
    public float speed = 100;

    private new AudioSource audio;
    public GameObject particlePaddle;
    private Rigidbody2D rb;

    public static bool hasPowerUp;
    private float shotCoolDown = 0f;
    public GameObject bulletPrefab;

    // Use this for initialization
    void Start()
    {
        hasPowerUp = false;
        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
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
            rb.velocity = new Vector2(input * speed, 0);
        }
        else if (input < 0.1f)
        {
            rb.velocity = new Vector2(input * speed, 0);
        }
        float currentX = Mathf.Clamp(transform.position.x, LeftBlockTransform.position.x + 1, RightBLockTransform.position.x - 1);
        transform.position = new Vector3(currentX, transform.position.y, transform.position.z);

        if(hasPowerUp)
        {
            if(Input.GetKey(KeyCode.X))
            {
                shotCoolDown -= Time.deltaTime;

                if (shotCoolDown <= 0)
                {
                    Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                    shotCoolDown = .8f;
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            audio.Play();
            Instantiate(particlePaddle, transform.position, Quaternion.identity);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "PowerUp")
        {
            hasPowerUp = true;
            Destroy(col.gameObject);
        }
    }
}
