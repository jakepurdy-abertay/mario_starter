using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// variables taken from CharacterController.Move example script
	// https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
	public float speed = 6.0F;
    public float slowSpeed = 2.0f;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;

	public int Lives = 3; // number of lives the player hs

    public long Score = 0; // score

	Vector3 start_position; // start position of the player

    GameObject Gardener;
    CharacterController controller;

    public Material mat_stand;
    public Material mat_run;
    public Material mat_jump;

    float currentSpeed;


    int lastInput = 1;

    Vector3 moveDirection;

    void Start()
	{
		// record the start position of the player
		start_position = transform.position;
        Gardener = GameObject.FindGameObjectWithTag("Gardener");

        // get the character controller attached to the player game object
        controller = GetComponent<CharacterController>();
    }

	public void Reset()
	{
		// reset the player position to the start position
		transform.position = start_position;
	}

	void Update()
	{

		// check to see if the player is on the ground
		if (controller.isGrounded) 
		{
			// check to see if the player should jump
			if (Input.GetButton("Jump"))
				moveDirection.y = jumpSpeed;
		}

        if (Input.GetButton("Fire1"))
        {
            Gardener.GetComponent<BoxCollider>().enabled = true;
            Gardener.GetComponent<Renderer>().enabled = true;
            currentSpeed = slowSpeed;
        } else
        {
            Gardener.GetComponent<BoxCollider>().enabled = false;
            Gardener.GetComponent<Renderer>().enabled = false;
            currentSpeed = speed;
        }

		// apply gravity to movement direction
		moveDirection.y -= gravity * Time.deltaTime;

        if (Input.GetAxis("Horizontal") != 0)
        {
            lastInput = System.Math.Sign(Input.GetAxis("Horizontal"));
        }

            if (!controller.isGrounded)
        {
            gameObject.GetComponent<Renderer>().material = mat_jump;
        } else if (Input.GetAxis("Horizontal") != 0)
        {
            gameObject.GetComponent<Renderer>().material = mat_run;
            lastInput = System.Math.Sign(Input.GetAxis("Horizontal"));
        } else
        {
            gameObject.GetComponent<Renderer>().material = mat_stand;
        }
        
        moveDirection.x = Input.GetAxis("Horizontal")*currentSpeed;

        gameObject.transform.localScale = new Vector3(lastInput*2, 2,0.1f);
        Gardener.transform.position = new Vector3((float)(0.5 * lastInput), 0, 0) + gameObject.transform.position;

        // make the call to move the character controller
        controller.Move(moveDirection * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // find out what we've hit
        if (hit.collider.gameObject.CompareTag("Platform") && moveDirection.y > 0)
        {
            moveDirection.y = 0;
        }
    }
}