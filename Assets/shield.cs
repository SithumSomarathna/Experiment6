using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shield : MonoBehaviour
{
    public float aliveTime;
    public float blinkTime;
    public float blinks;
    public SpriteRenderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(Life());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Life()
    {
        yield return new WaitForSeconds(aliveTime - blinkTime);
        float i = 0;
        rend.enabled = false;
        while (i < 2 * blinks)
        {
            yield return new WaitForSeconds(blinkTime / (2 * blinks));
            rend.enabled = !rend.enabled;
            i += 1;
        }
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
