using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warningFlash : MonoBehaviour
{
    public float blinkTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Flash());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Flash()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(blinkTime / 6);
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(blinkTime / 6);
        GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(blinkTime / 6);
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(blinkTime / 6);
        GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(blinkTime / 6);
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(blinkTime / 6);
        Destroy(gameObject);
    }
}
