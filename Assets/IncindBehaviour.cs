using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncindBehaviour : MonoBehaviour
{
    public ParticleSystem particles;
    public GameObject spike;
    public float borderHeight;
    // Start is called before the first frame update
    void Start()
    {
        
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
        if(collision.gameObject.tag == "Platform")
        {
            Instantiate(spike, new Vector3(transform.position.x, Mathf.Floor(transform.position.y) + 0.5f), Quaternion.Euler(0f, 0f, 0f));
            FindObjectOfType<AudioManager>().Play("Incind");
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("Incind");
        }
        Die();
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
