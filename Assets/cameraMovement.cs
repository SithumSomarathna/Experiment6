using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    public float shakeDuration;
    public float shakeAmount;
    public float decreaseFactor;
    private Vector3 original;
    public float mult;
    // Start is called before the first frame update
    void Start()
    {
        original = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(shakeDuration > 0)
        {
            Vector3 rand = Random.insideUnitCircle;
            transform.localPosition = original + rand * shakeAmount * mult;

            shakeDuration -= decreaseFactor;
        }
        else
        {
            shakeDuration = 0;
            transform.position = original;
        }
    }

    
}
