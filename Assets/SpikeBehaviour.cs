using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBehaviour : MonoBehaviour
{
    public GameObject spike;
    public float spreadTime;
    public LayerMask wall;
    public LayerMask incind;
    private bool left;
    private bool right;
    public float aliveTime;
    public ParticleSystem particles;
    public AudioClip flame;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource.PlayClipAtPoint(flame, transform.position, 0.5f);
        if (Physics2D.OverlapCircle(new Vector2(transform.position.x - 1f, transform.position.y - 0.5f), 0.4f, wall) && !(Physics2D.OverlapCircle(new Vector2(transform.position.x - 1f, transform.position.y), 0.4f, incind) || Physics2D.OverlapCircle(new Vector2(transform.position.x - 1f, transform.position.y), 0.4f, wall)))
        {
            left = true;
        }
        if (Physics2D.OverlapCircle(new Vector2(transform.position.x + 1f, transform.position.y - 0.5f), 0.4f, wall) && !(Physics2D.OverlapCircle(new Vector2(transform.position.x + 1f, transform.position.y), 0.4f, incind) || Physics2D.OverlapCircle(new Vector2(transform.position.x + 1f, transform.position.y), 0.4f, wall)))
        {
            right = true;
        }
        if(left == true || right == true)
        {
            StartCoroutine(Spawn());
        }
        StartCoroutine(Life());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(spreadTime);
        if (GameObject.Find("GameManager").GetComponent<GameManagement>().boarding == false)
        {
            if (left)
            {
                Instantiate(spike, new Vector3(transform.position.x - 1f, transform.position.y), Quaternion.identity);
            }
            if (right)
            {
                Instantiate(spike, new Vector3(transform.position.x + 1f, transform.position.y), Quaternion.identity);
            }
        }
    }

    IEnumerator Life()
    {
        yield return new WaitForSeconds(aliveTime);
        if (gameObject != null)
        {
            Die();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Shield") 
        {
            Die();
        }
    }

    public void Die()
    {
        Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
