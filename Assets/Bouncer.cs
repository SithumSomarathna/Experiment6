using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    public Rigidbody2D rb;
    public float initForce;
    private float bounces = 0;
    public float maxBounces;
    public ParticleSystem particles;
    public float borderHeight;
    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(-rb.transform.right * initForce);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > borderHeight)
        {
            transform.position = new Vector3(transform.position.x, -borderHeight + 0.1f);
        }
        if (transform.position.y < -borderHeight)
        {
            transform.position = new Vector3(transform.position.x, borderHeight - 0.1f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        bounces += 1;
        FindObjectOfType<AudioManager>().Play("Bounce_Gun");
        if (bounces >= maxBounces)
        {          
            Die();
        }
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Shield")
        {
            Die();
        }
    }

    public void Die()
    {
        FindObjectOfType<AudioManager>().Play("Bouncer_Exp");
        Instantiate(particles, transform.position, Quaternion.identity);
        GameObject cam = GameObject.Find("Main Camera");
        cam.GetComponent<cameraMovement>().shakeAmount = 0.05f;
        cam.GetComponent<cameraMovement>().shakeDuration = 5;
        Destroy(gameObject);
    }
}
