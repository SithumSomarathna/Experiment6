using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public bool leftDoor;
    private float move = 0;
    private bool shouldMove = true;
    public float moveTime;
    public float lerpThreshold = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(move == 1 && shouldMove == true)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0f, 0f, 0f), moveTime);
            if(Mathf.Abs(transform.position.x) <= 0f + lerpThreshold) {
                shouldMove = false;
                transform.position = new Vector3(0f, 0f, 0f);
            }
        }
        if (move == -1 && shouldMove == true)
        {
            if (leftDoor == true)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(-41f, 0f, 0f), moveTime);
                if (transform.position.x <= -41f + lerpThreshold) {
                    shouldMove = false;
                    transform.position = new Vector3(-41f, 0f, 0f);
                }
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(41f, 0f, 0f), moveTime);
                if (transform.position.x >= 41f - lerpThreshold) {
                    shouldMove = false;
                    transform.position = new Vector3(41f, 0f, 0f);
                }
            }
        }
    }

    public void MoveIn()
    {
        FindObjectOfType<AudioManager>().Play("Doors");
        shouldMove = true;
        move = 1;
    }

    public void MoveOut()
    {
        FindObjectOfType<AudioManager>().Play("Doors");
        shouldMove = true;
        move = -1;
    }
}
