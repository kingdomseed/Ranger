using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    [SerializeField]
    private Player _player;
    [SerializeField]
    private AudioClip _audioClip;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && Input.GetButtonDown("Action"))
        {
            _player.pickUpCoin();
            AudioSource.PlayClipAtPoint(_audioClip, Camera.main.transform.position);
            Destroy(gameObject);
        }
    }
}
