using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sawBlade : MonoBehaviour
{
    public Rigidbody2D rb; //Referecing the Rigidbody
    public float speed; //variable for the speed of the saw
    private float direction; //variable that will store the direction of the saw
    private bool hittingWall = false; //bool to check if the saw is hitting a wall
    public Transform wallCheck; //Referencing the transform of the wallCheck object (child of this object)
    public LayerMask wall; //Referencing the layer of the platforms
    public float wallCheckOffset_x; 
    public float wallCheckOffset_y;
    public float borderHeight; //Height threshold for the saw to fall through the bottom of the level to the top
    public Transform sprite; //Referencing the transform of the sprite
    public float rotSpeed; //Rotation speed of the sprite
    public float aliveTime; //How long (in seconds) that the saw will stay in the level
    //private float curWallHits = 0; <-- IGNORE THIS
    private bool groundHit = false; //Has the saw hit the ground
    public ParticleSystem particles;
    public AudioSource sound;

    // Start is called before the first frame update
    void Start()
    {
        sound.Pause();
        direction = -1; //Initialize direction to face left
        StartCoroutine(AliveTime()); //Start the coroutine function
        float swap = Random.Range(0, 2);    //Generate a random number between 0 and 1
        if(swap == 0)                       //If the number is 0, flip the sprite to face left, otherwise it will remain facing left (so that the starting direction is randomized)
        {                                   
            Flip();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Check if the saw is hitting the wall in its direction of travel. This is done by creating an overlap circle next to the saw and checking if the circle intersects with the wall
        hittingWall = Physics2D.OverlapArea(new Vector2(wallCheck.position.x + wallCheckOffset_x, wallCheck.position.y + wallCheckOffset_y), new Vector2(wallCheck.position.x - wallCheckOffset_x, wallCheck.position.y - wallCheckOffset_y), wall);

        if (hittingWall) //If hitting a wall, flip the object and start going the other way
        {
            //curWallHits += 1; <-- Ignore this
            direction = direction * -1;
            Flip();
        }

        if(groundHit == true) //When this object is spawned, it will fall directly downwards until it hits the floor and then it will move left or right. This bit here is to check if it has hit the ground for the first time
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime * -1);
        }
        

        if (rb.position.y < -borderHeight) //If the object has fallen below the level, teleport it to the top
        {
            rb.transform.position = new Vector3(rb.position.x, borderHeight, 0f);
        }

        sprite.transform.Rotate(0f, 0f, rotSpeed); //Rotate the sprite
    }

    private void Flip() //everytime Flip() is called, the whole gameobject is flipped
    {
        transform.Rotate(0f, 180f, 0f);
    }

    IEnumerator AliveTime()
    {
        yield return new WaitForSeconds(aliveTime); //Once this time frame is reached, execute the Die() function
        Die();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Platform") //If a platform is hit, make goundHit = true
        {
            groundHit = true;
            sound.UnPause();
        }
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Shield") // IF a player is hit, execute the Die() function
        {
            Die();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Platform")
        {
            sound.Pause();
        }
    }

    public void Die() // when Die() is called, the gameobject is destroyed and particles are spawned
    {
        FindObjectOfType<AudioManager>().Play("Saw_Exp");
        Instantiate(particles, transform.position, Quaternion.identity);
        GameObject cam = GameObject.Find("Main Camera");
        cam.GetComponent<cameraMovement>().shakeAmount = 0.15f;
        cam.GetComponent<cameraMovement>().shakeDuration = 10;
        Destroy(gameObject);
    }
}
