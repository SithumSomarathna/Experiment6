using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile : MonoBehaviour
{
    public List<GameObject> playerList;
    private float closestDistance = Mathf.Infinity;
    private GameObject bestTarget;
    public float AliveTime;
    public float speed = 0;
    public float acc;
    public ParticleSystem boost;
    public ParticleSystem particles;
    public float rotateSpeed;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("Missile");
        Instantiate(boost, transform.position, Quaternion.Euler(0f, 0f, 175f), gameObject.transform);
        float playerNumber = 1;
        while (playerNumber <= 6)
        {
            string name = "Player " + playerNumber.ToString() + "(Clone)";
            if (GameObject.Find(name) != null)
            {
                playerList.Add(GameObject.Find(name));
            }
            playerNumber += 1;
        }

        foreach (GameObject potentialTarget in playerList)
        {
            if (potentialTarget != null && potentialTarget.activeSelf == true)
            {
                Vector3 directionToTarget = potentialTarget.transform.position - transform.position;
                float distanceToTaget = directionToTarget.sqrMagnitude;
                if (distanceToTaget < closestDistance)
                {
                    closestDistance = distanceToTaget;
                    bestTarget = potentialTarget;
                }
            }
            else
            {
                playerList.Remove(potentialTarget);
            }
        }
        if (closestDistance == Mathf.Infinity)
        {
            Die();
        }
        StartCoroutine(Alive());
    }

    // Update is called once per frame
    void Update()
    {
        if (bestTarget != null)
        {
            //Debug.Log(bestTarget);
            Vector3 direction = bestTarget.transform.position - transform.position;
            transform.right = Vector3.Lerp(transform.right, direction, rotateSpeed * Time.deltaTime);
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            speed += acc;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        }
        else
        {
            Die();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("col");
        Die();
    }

    public void Die()
    {
        GameObject cam = GameObject.Find("Main Camera");
        cam.GetComponent<cameraMovement>().shakeAmount = 0.35f;
        cam.GetComponent<cameraMovement>().shakeDuration = 10;
        FindObjectOfType<AudioManager>().Play("Missile_Exp");
        Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator Alive()
    {
        yield return new WaitForSeconds(AliveTime);
        if(gameObject != null)
        {
            Die();
        }
    }

    
}
