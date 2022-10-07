using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb; //Referencing the Rigidbody
    public float speed; //variable for horizontal speed
    public float jump; //variable for the force of the jump
    public float playerNumb;
    
    private float direction; //direction the player is moving, 1 = right, -1 = left
    //private float CurrentMove = 0; <-- IGNORE THIS
    private bool hittingWall = false; //bool for if the player has hit a wall on the side
    private bool hittingFloor = false; //bool for if the player has hit the floor underneath it
    public Transform wallCheck; //Referencing the transform for the wallCheck
    public Transform floorCheck; //Referencing the transform for the wallCheck
    public LayerMask wall; //Referencing the layer of the platform
    public LayerMask players; //Referencing the layer of the players
    public float wallCheckOffset_x; 
    public float wallCheckOffset_y;
    public float floorCheckOffset_x;
    public float floorCheckOffset_y;
    public bool upPressed; //bool for if the up buttom is pressed 
    public float borderHeight; //Height threshold for the saw to fall through the bottom of the level to the top
    public string key; //The buttom to press to make the player jump (this can be changed which makes the game flexible)
    public ParticleSystem particles;
    public float powerUp = 0; //0: no powerrup, 1: weapon, 2: shield, 3: jumps
    public float powerUpTime;
    public GameObject Shield;
    public GameObject Weapon;
    public GameObject Jumps;
    public AudioSource runSound;
    public Sprite S_jump;
    public Sprite S_jump1;
    public Sprite S_jump2;
    public SpriteRenderer rend;
    public Animator anim;
    public float midJumpThresh;
    [SerializeField]
    private int jumpsLeft; //variable for how many jumps the player has left
    private bool isJumping = true;

    // Use this for initialization
    void Start()
    {
        direction = 1; //Initialize the direction to be facin right
        //Initializing more vairables
        upPressed = false; 
        jumpsLeft = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Check if the player is hitting the wall in its direction of travel. This is done by creating an overlap rectangle next to the player and checking if the rectangle intersects with the wall
        hittingWall = (Physics2D.OverlapArea(new Vector2(wallCheck.position.x + wallCheckOffset_x, wallCheck.position.y + wallCheckOffset_y), new Vector2(wallCheck.position.x - wallCheckOffset_x, wallCheck.position.y - wallCheckOffset_y), wall) || (Physics2D.OverlapArea(new Vector2(wallCheck.position.x + wallCheckOffset_x, wallCheck.position.y + wallCheckOffset_y), new Vector2(wallCheck.position.x - wallCheckOffset_x, wallCheck.position.y - wallCheckOffset_y), players) && powerUp != 1));

        if (hittingWall) //If hitting a wall, flip the object and start going the other way
        {
            direction = direction * -1;
            Flip();
        }

        transform.Translate(Vector2.right * speed * Time.deltaTime); //Move the player in the direction its facing

        if (!Input.GetKey(key)) { upPressed = false; } //Check if the up key is pressed

        if (Input.GetKey(key) && (jumpsLeft > 0 || powerUp == 3) && upPressed == false) //If pressing the up key and the player has 1 or more jumps left, make the player jump
        {           
            rb.velocity = Vector2.up * jump; //Add velocity upwards
            upPressed = true;
            rend.sprite = S_jump;
            jumpsLeft -= 1; //Reduce the number of jumps left by one
            FindObjectOfType<AudioManager>().Play("Player_Jump");
        }

        //CurrentMove = Input.GetAxisRaw("Horizontal") * speed; <-- IGNORE THIS

        if(rb.position.y < -borderHeight)
        { //If the player is below the level, teleport him to the top
            rb.transform.position = new Vector3(rb.position.x, borderHeight - 0.1f, 0f);
        }
        if (rb.position.y > borderHeight)
        {  //If the player is above the level, teleport him to the bottom
            rb.transform.position = new Vector3(rb.position.x, -borderHeight + 0.1f, 0f);
        }

        if(isJumping == true)
        {
            if(rb.velocity.y < midJumpThresh && rb.velocity.y > -midJumpThresh && rend.sprite == S_jump)
            {
                rend.sprite = S_jump1;
            }
            if (rb.velocity.y < -midJumpThresh && rend.sprite == S_jump1)
            {
                rend.sprite = S_jump2;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && powerUp != 2) //If the player hits an enemy, execute the Die() function
        {
            Die();
        }
        if (collision.gameObject.tag == "Player" && powerUp != 2 && powerUp != 1) //If the player hits a player with the spikes powerrup, execute the Die() function
        {
            if (collision.gameObject.GetComponent<PlayerMovement>().powerUp == 1)
            {
                Die();
            }
        }

        //Check if the player is hitting the wall in its direction of travel. This is done by creating an overlap rectangle under the player and checking if the rectangle intersects with the floor
        hittingFloor = (Physics2D.OverlapArea(new Vector2(floorCheck.position.x + floorCheckOffset_x, floorCheck.position.y + floorCheckOffset_y), new Vector2(floorCheck.position.x - floorCheckOffset_x, floorCheck.position.y - floorCheckOffset_y), wall) || Physics2D.OverlapArea(new Vector2(floorCheck.position.x + floorCheckOffset_x, floorCheck.position.y + floorCheckOffset_y - 0.5f), new Vector2(floorCheck.position.x - floorCheckOffset_x, floorCheck.position.y - floorCheckOffset_y - 0.5f), players));

        if (hittingFloor) //If the player has hit the ground, reset his jumps back to 2
        {
            jumpsLeft = 2;
            runSound.Play();
            anim.enabled = true;
            isJumping = false;
        }

        if(collision.gameObject.tag == "Powerup")
        {
            powerUp = Random.Range(1, 4);
            Debug.Log(powerUp);
            switch (powerUp)
            {
                case 1:
                    Instantiate(Weapon, transform.position, Quaternion.identity, gameObject.transform);
                    break;
                case 2:
                    Instantiate(Shield, transform.position, Quaternion.identity, gameObject.transform);
                    break;
                case 3:
                    Instantiate(Jumps, transform.position, Quaternion.identity, gameObject.transform);
                    break;
            }
            StartCoroutine(KillPowerUp());
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Platform")
        {
            runSound.Stop();
            anim.enabled = false;
            rend.sprite = S_jump;
            isJumping = true;
        }
    }


    private void Flip() //everytime Flip() is called, the whole gameobject is flipped
    {
        transform.Rotate(0f, 180f, 0f);
    }

    private void Die() // when Die() is called, the gameobject is destroyed and particles are spawned
    {
        Instantiate(particles, transform.position, Quaternion.identity);
        GameObject cam = GameObject.Find("Main Camera");
        cam.GetComponent<cameraMovement>().shakeAmount = 0.2f;
        cam.GetComponent<cameraMovement>().shakeDuration = 15;
        if(powerUp != 0)
        {
            GameObject.Find("EnemySpawnController(Clone)").GetComponent<EnemySpawn>().StartPowerup();
        }
        GameObject.Find("GameManager").GetComponent<GameManagement>().PlayerDied(playerNumb);
        Destroy(gameObject);
    }

    IEnumerator KillPowerUp()
    {
        Debug.Log("Powerup Killed");
        yield return new WaitForSeconds(powerUpTime);
        GameObject.Find("EnemySpawnController(Clone)").GetComponent<EnemySpawn>().StartPowerup();
        powerUp = 0;
    }
}
