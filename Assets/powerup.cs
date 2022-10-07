using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerup : MonoBehaviour
{
    public float hue = 0;
    public float hueShift;
    public SpriteRenderer rend;
    public float aliveTime;
    public float blinkTime;
    public float blinks;
    public ParticleSystem particles;
    public float burstTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Alive());
        StartCoroutine(Flash());
    }

    // Update is called once per frame
    void Update()
    {
        rend.color = Color.HSVToRGB(hue, 1, 1);
        hue += hueShift;
        if(hue >= 1)
        {
            hue = hue - 1;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    IEnumerator Alive()
    {
        yield return new WaitForSeconds(aliveTime - blinkTime);
        if (gameObject != null)
        {
            float i = 0;
            rend.enabled = false;
            while (i < 2 * blinks)
            {
                yield return new WaitForSeconds(blinkTime / (2 * blinks));
                rend.enabled = !rend.enabled;
                i += 1;
            }

            GameObject.Find("EnemySpawnController(Clone)").GetComponent<EnemySpawn>().StartPowerup();
            Destroy(gameObject);
        }

    }

    IEnumerator Flash()
    {
        if (gameObject != null) { 
            yield return new WaitForSeconds(burstTime);
            Instantiate(particles, transform.position, Quaternion.identity);
            StartCoroutine(Flash());
        }
    }
}
