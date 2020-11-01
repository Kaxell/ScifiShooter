using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PickUps : MonoBehaviour
{
    private int haveKey;
    private int needKey;
    public Text keyCount;
    private AudioSource pickUpSound;

    // Start is called before the first frame update
    void Start()
    {
        haveKey = 0;
        needKey = 3;
        keyCount.text = "";
        SetCountText();
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().SpawnKey(needKey);
        pickUpSound = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUpKey")
        {
            other.gameObject.SetActive(false);
            haveKey++;
            SetCountText();
            if (haveKey == needKey)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().OnKeysCollected();
            }
            pickUpSound.Play();
        }
        else if (other.gameObject.tag == "PickUpHealth")
        {
            other.gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatController>().OnHealthPickedUp();
            pickUpSound.Play();
        }
        else if (other.gameObject.tag == "PickUpAmmo")
        {
            other.gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("Weapon").GetComponent<PistolShoot>().OnAmmoPickedUp();
            pickUpSound.Play();
        }

    }
    private void SetCountText()
    {
        keyCount.text = "Keys Collected: " + haveKey.ToString() + " / "+ needKey.ToString();
 
    }
}
