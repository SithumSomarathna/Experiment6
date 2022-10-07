using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float acc;
    public float speed;
    public int dir = 0;
    private bool dirSet = false;
    public LayerMask wall;
    public ParticleSystem particles;
    public float borderHeight;
    public float maxSpeed;
    public Rigidbody2D rb;
    public AudioSource sound;
    // Start is called before the first frame update
    void Start()
    {
        dir = Random.Range(1, 5);
       
        while(dirSet == false)
        {
            switch (dir)
            {
                case 1:
                    if(Physics2D.OverlapCircle(new Vector2(transform.position.x - 0.5f, transform.position.y), 0.2f, wall))
                    {
                        dirSet = true;
                    }
                    else
                    {
                        dir += 1;
                    }
                    break;
                case 2:
                    if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + 0.5f), 0.2f, wall))
                    {
                        dirSet = true;
                    }
                    else
                    {
                        dir += 1;
                    }
                    break;
                case 3:
                    if (Physics2D.OverlapCircle(new Vector2(transform.position.x + 0.5f, transform.position.y), 0.2f, wall))
                    {
                        dirSet = true;
                    }
                    else
                    {
                        dir += 1;
                    }
                    break;
                case 4:
                    if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.5f), 0.2f, wall))
                    {
                        dirSet = true;
                    }
                    else
                    {
                        dir = 1;
                    }
                    break;              
            }
            
        }
        
        switch (dir)
        {
            case 1:
                break;
            case 2:
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -90f));
                break;
            case 3:
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -180f));
                break;
            case 4:
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));
                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector2.right * speed);
        if(speed + acc < maxSpeed)
        {
            speed += acc;
        }
        else
        {
            speed = maxSpeed;
        }

        if(transform.position.y > borderHeight)
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
        Die();
    }

    public void Die() // when Die() is called, the gameobject is destroyed and particles are spawned
    {
        FindObjectOfType<AudioManager>().Play("Bullet_Exp");
        sound.Stop();
        var particlePos = new Vector3(0f, 0f);
        switch (dir)
        {
            case 1:
                particlePos = new Vector3(-0.85f, 0f);
                break;
            case 2:
                particlePos = new Vector3(0f, 0.85f);
                break;
            case 3:
                particlePos = new Vector3(0.85f, 0f);
                break;
            case 4:
                particlePos = new Vector3(0f, -0.85f);
                break;
        }
        Instantiate(particles, transform.position + particlePos, Quaternion.identity);
        GameObject cam = GameObject.Find("Main Camera");
        cam.GetComponent<cameraMovement>().shakeAmount = 0.15f;
        cam.GetComponent<cameraMovement>().shakeDuration = 10;
        Destroy(gameObject);
    }
}
