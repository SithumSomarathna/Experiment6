using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Placer : MonoBehaviour
{
    public GameObject text;
    private TextMeshPro textObject;
    private bool shouldMove = false;
    public float moveTime;
    private Vector3 pos;
    public float lerpThreshold;
    // Start is called before the first frame update
    void Start()
    {
        textObject = text.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldMove == true)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, pos, moveTime);
            if (Mathf.Abs(transform.localPosition.y - pos.y) <= lerpThreshold) {
                shouldMove = false;
                Debug.Log(pos);
                transform.localPosition = pos;
            }
        }
    }

    public void WriteScore(float score)
    {
        textObject.SetText(score.ToString());
    }

    public void MovePlacer(float rank)
    {
        pos = new Vector3(0f, -3f + rank * -4.5f /*3125f*/, 0f);
        shouldMove = true;
    }
}
