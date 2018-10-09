using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkShop : MonoBehaviour {

    [SerializeField]
    private AudioClip _winAudio;
    [SerializeField]
    private Player _player;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && _player.checkForCoin() && Input.GetButtonDown("Action"))
        {
            _player.buyGun();
            AudioSource.PlayClipAtPoint(_winAudio, Camera.main.transform.position);
        }
    }
}
