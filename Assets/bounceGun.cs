using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bounceGun : MonoBehaviour
{
    private float rot;
    public GameObject bouncer;
    public float rate;
    public ParticleSystem particles;
    // Start is called before the first frame update
    void Start()
    {
        rot = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0f, 0f, rot);
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(rate);
        if(gameObject != null) {
            FindObjectOfType<AudioManager>().Play("Bounce_Gun_Shoot");
            Instantiate(bouncer, transform.position, transform.rotation);
        }
        yield return new WaitForSeconds(rate);
        if (gameObject != null)
        {
            FindObjectOfType<AudioManager>().Play("Bounce_Gun_Shoot");
            Instantiate(bouncer, transform.position, transform.rotation);
        }
        yield return new WaitForSeconds(rate);
        if (gameObject != null)
        {
            FindObjectOfType<AudioManager>().Play("Bounce_Gun_Shoot");
            Instantiate(bouncer, transform.position, transform.rotation);
            Die();
        }
        
    }

    public void Die()
    {
        FindObjectOfType<AudioManager>().Play("Bouncer_Exp");
        Instantiate(particles, transform.position, Quaternion.identity);
        GameObject cam = GameObject.Find("Main Camera");
        cam.GetComponent<cameraMovement>().shakeAmount = 0.1f;
        cam.GetComponent<cameraMovement>().shakeDuration = 10;
        Destroy(gameObject);
    }
}
