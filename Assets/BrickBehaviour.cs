using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBehaviour : MonoBehaviour
{
    public GameObject wallCheck;
    public float borderHeight;
    public Rigidbody2D rb;
    public bool going;
    public float acc;
    public float speed;
    public LayerMask wall;
    public float maxSpeed;
    private bool vertical = false;
    public float StallTime;
    public float hits;
    private float curHits = 0;
    public ParticleSystem particles;
    public AudioSource scrape;
    // Start is called before the first frame update
    void Start()
    {
        scrape.Stop();
        float random = Random.Range(0, 4);
        /*
        transform.Rotate(0f, 0f, 90f * random);
        if(random == 1 || random == 3)
        {
            vertical = true;
        }
        */
        going = false;
        StartCoroutine(Stall());
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        wallCheck.transform.localPosition = new Vector3(speed/2, 0f, 0f);
        if (vertical == false) {
            
            if (Physics2D.OverlapArea(new Vector2(wallCheck.transform.position.x - speed/2 - 0.9f, wallCheck.transform.position.y + 0.9f), new Vector2(wallCheck.transform.position.x + speed / 2 + 0.9f, wallCheck.transform.position.y - 0.9f), wall))
            {
                //Debug.Log("horizontal");
                transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y);
                wallCheck.transform.localPosition = new Vector3(1f, 0f);
                
                while(Physics2D.OverlapCircle(wallCheck.transform.position, 0.5f, wall) == false)
                {
                    transform.Translate(Vector2.right);
                }
                
                curHits += 1;
                scrape.Stop();
                FindObjectOfType<AudioManager>().Play("Brick_Impact");
                GameObject cam = GameObject.Find("Main Camera");
                cam.GetComponent<cameraMovement>().shakeAmount = 0.2f;
                cam.GetComponent<cameraMovement>().shakeDuration = 10;
                Rotate();
                speed = 0;
            }
        }
        else
        {
            
            if (Physics2D.OverlapArea(new Vector2(wallCheck.transform.position.x - 0.9f, wallCheck.transform.position.y + speed / 2 + 0.9f), new Vector2(wallCheck.transform.position.x + 0.9f, wallCheck.transform.position.y - speed / 2 - 0.9f), wall))
            {
                //Debug.Log("vertical");
                transform.position = new Vector3(transform.position.x, Mathf.Round(transform.position.y));
                wallCheck.transform.localPosition = new Vector3(1f, 0f);
                while (Physics2D.OverlapCircle(wallCheck.transform.position, 0.5f, wall) == false)
                {
                    transform.Translate(Vector2.right);
                }
                curHits += 1;
                scrape.Stop();
                FindObjectOfType<AudioManager>().Play("Brick_Impact");
                GameObject cam = GameObject.Find("Main Camera");
                cam.GetComponent<cameraMovement>().shakeAmount = 0.2f;
                cam.GetComponent<cameraMovement>().shakeDuration = 10;
                Rotate();
                speed = 0;
            }
        }

        if(curHits >= hits)
        {
            Die();
        }
        
        if(going == true) { 
            transform.Translate(Vector2.right * speed);
            if (speed + acc < maxSpeed)
            {
                speed += acc;
            }
            else
            {
                speed = maxSpeed;
            }
        }

        if (transform.position.y > borderHeight)
        {
            transform.position = new Vector3(transform.position.x, -borderHeight + 0.1f);
        }
        if (transform.position.y < -borderHeight)
        {
            transform.position = new Vector3(transform.position.x, borderHeight - 0.1f);
        }
    }

    void Rotate()
    {
        going = false;
        speed = 0;
        vertical = !vertical;
        float Checked = 0;
        wallCheck.transform.localPosition = new Vector3(0f, 1f, 0f);
        if (Physics2D.OverlapCircle(wallCheck.transform.position, 0.5f, wall) && Checked == 0)
        {
            transform.Rotate(0f, 0f, -90f);
            StartCoroutine(Stall());
            Checked = 1;
        }
        wallCheck.transform.localPosition = new Vector3(0f, -1f, 0f);
        if (Physics2D.OverlapCircle(wallCheck.transform.position, 0.5f, wall) && Checked == 0)
        {
            transform.Rotate(0f, 0f, 90f);
            StartCoroutine(Stall());
            Checked = 1;
        }
        if(Checked == 0)
        {
            float dir = Random.Range(0, 2);
            if (dir == 0)
            {
                transform.Rotate(0f, 0f, 90f);
            }
            else
            {
                transform.Rotate(0f, 0f, -90f);
            }
            StartCoroutine(Stall());
            Checked = 1;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Die();
        }
    }


    IEnumerator Stall()
    {
        yield return new WaitForSeconds(StallTime);
        going = true;
        scrape.Play();
    }

    

    public void Die()
    {
        Instantiate(particles, transform.position, Quaternion.identity);
        GameObject cam = GameObject.Find("Main Camera");
        cam.GetComponent<cameraMovement>().shakeAmount = 0.2f;
        cam.GetComponent<cameraMovement>().shakeDuration = 10;
        Destroy(gameObject);
    }
}
