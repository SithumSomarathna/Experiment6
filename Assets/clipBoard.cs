using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clipBoard : MonoBehaviour
{
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
        if (move == 1 && shouldMove == true)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0f, 0f, 0f), moveTime);
            if (transform.position.y >= 0f - lerpThreshold)
            {
                shouldMove = false;
                transform.position = new Vector3(0f, 0f, 0f);
                //GameObject.Find("GameManager").GetComponent<GameManagement>().BoardUp();
            }
        }
        if (move == -1 && shouldMove == true)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0f, -42f, 0f), moveTime);
            if (transform.position.y <= -42 + lerpThreshold) {
                shouldMove = false;
                transform.position = new Vector3(0f, -42f, 0f);
            }
        }
    }

    public void MoveIn()
    {
        FindObjectOfType<AudioManager>().Play("ClipBoard");
        shouldMove = true;
        move = 1;
    }

    public void MoveOut()
    {
        shouldMove = true;
        move = -1;
    }
}
