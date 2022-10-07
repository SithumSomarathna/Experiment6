using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovement : MonoBehaviour
{
    private bool shouldMove = false;
    public float moveTime;
    public float lerpThreshold = 0.5f;
    private Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (shouldMove == true)
        {
            
            transform.position = Vector3.Lerp(transform.position, pos, moveTime);
            if (Mathf.Abs(transform.position.x - pos.x) <= lerpThreshold)
            {
                shouldMove = false;
                transform.position = pos;
            }
            
            //transform.position = pos;
        }
        
    }

    public void Move(float Xpos)
    {
        shouldMove = true;
        pos = new Vector3(Xpos, 0f, 0f);
    }
}
