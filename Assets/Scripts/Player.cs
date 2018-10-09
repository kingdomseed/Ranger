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
    private GameObject _weapon;
    [SerializeField]
    private GameObject _muzzelFlash;
    [SerializeField]
    private GameObject _hitMarker;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private bool invertY = true;
    [SerializeField]
    private UIManager _uiManager;

    private bool cursorVisibility = false;
    private CharacterController _controller;

    private bool _hasCoin = false;

    // ;)
    private int _currentAmmunation;
    private int _maxAmmunation = 100;

    private bool isReloading = false;

    // Setting these as instance variables to simplify CalculateLook() assignments
    private float _mouseX;
    private float _mouseY;

	void Start ()
    {
        // Access to the character controller on this player object including the Move() funciton.
        _controller = transform.parent.GetComponent<CharacterController>();

        // Hide mouse cursor
        Cursor.visible = cursorVisibility;
        Cursor.lockState = CursorLockMode.Locked;

        _currentAmmunation = _maxAmmunation;
    }

	void Update ()
    {
        // if ESC pressed, unhide mouse cursor
        if(Input.GetButtonDown("Cancel"))
        {
            Cursor.visible = !cursorVisibility;
            Cursor.lockState = CursorLockMode.None;
        }
        CalculateMovement();
        CalculateLook();
        if (Input.GetButton("Fire1") && _currentAmmunation > 0 && _weapon.activeInHierarchy){
 
            ShootOrActivate();
        } else
        {
            _muzzelFlash.SetActive(false);
            _audioSource.Stop();
        }
        if(Input.GetButtonDown("Reload") && !isReloading)
        {
            isReloading = true;
            StartCoroutine(Reload());
        }
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

    private void ShootOrActivate()
    {
        // Shoot a ray (for a raycast) from the center of the ViewPort (0.5f to 0.5f)
        Ray rayOrigin = GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        // Turn on the muzzle flash, subtract ammo, play shooting sound
        _muzzelFlash.SetActive(true);
        _currentAmmunation--;
        _uiManager.UpdateAmmunation(_currentAmmunation);

        if(!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }

        // Cast meaningful rays
        RaycastHit hitInfo;
        if (Physics.Raycast(rayOrigin, out hitInfo))
        {
            Instantiate(_hitMarker, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));

            
            Destructible crate = hitInfo.transform.GetComponent<Destructible>();
            if (crate != null)
            {
                crate.DestroyCrate();
            }
        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(1.5f);
        _currentAmmunation = _maxAmmunation;
        isReloading = false;
        _uiManager.UpdateAmmunation(_currentAmmunation);
    }

    public void pickUpCoin()
    {
        _hasCoin = true;
        _uiManager.UpdateCoinInventory(_hasCoin);
    }

    public bool checkForCoin()
    {
        return _hasCoin;
    }

    public void buyGun()
    {
        // check for a coin
        if (_hasCoin)
        {
            // remove the coin, update the UI, activate the weapon
            _hasCoin = false;
            _uiManager.UpdateCoinInventory(_hasCoin);
            _weapon.SetActive(true);
        }
    }
}
