using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _gravity = 9.8f;
    [SerializeField]
    private float _sensitivity = 2.0f;
    [SerializeField]
    private bool invertY = true;

    private CharacterController _controller;

    // Setting these as instance variables to simplify CalculateLook() assignments
    private float _mouseX;
    private float _mouseY;

	void Start ()
    {
        // Access to the character controller on this player object including the Move() funciton.
        _controller = GetComponent<CharacterController>();
	}

	void Update ()
    {
        CalculateMovement();
        CalculateLook();
    }

    private void CalculateMovement()
    {
        // Temporarily store the vertical and horizontal input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Specify the direction the player is moving. We are moving on x and z (y is up and down).
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput);
        Vector3 velocity = direction * _speed;

        // convert veolicty vector to World Space
        velocity = transform.TransformDirection(velocity);

        // Have the character affected by gravity by applying a toward force.
        velocity.y -= _gravity;
        _controller.Move(velocity * Time.deltaTime);
    }

    private void CalculateLook()
    {
        _mouseX = Input.GetAxis("Mouse X");

        // Determines if the Y is inverted. Only accessibly from editor as of now.
        if(invertY)
        {
            _mouseY = -Input.GetAxis("Mouse Y");
        } else
        {
            _mouseY = Input.GetAxis("Mouse Y");
        }

        // Calculating rotation for left / right / up / down
        float rotationX = transform.localEulerAngles.x + (_mouseY * _sensitivity);
        float rotationY = transform.localEulerAngles.y + (_mouseX * _sensitivity);
        transform.localEulerAngles = new Vector3(rotationX, rotationY, transform.localEulerAngles.z);
    }
}
