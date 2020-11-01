using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PlayerStatController : MonoBehaviour
{
    public int Health;
    public int Level;
    public float Armor;

    //private float[] experienceRequiredPerLevel = { 5f, 10f, 25f, 50f, 100f, 200f };   
    
    public Text HealthText;
    public Text ArmorText;
    public Text LevelText;
    // Start is called before the first frame update

    private void Start()
    {
        //SetPlayerLevel(1);
    }

    public void SetPlayerLevel(int currentWaveLevel)
    {
        Level = currentWaveLevel;
        Health += Level * 30;
        Armor += Level * 0.1f;
        gameObject.GetComponent<FirstPersonMovement>().speed += currentWaveLevel * 0.5f;
        //This causes coupling so it is better to combine stat and movement after the prototype
        GameObject.FindGameObjectWithTag("Weapon").GetComponent<PistolShoot>().OnLeveledUp(currentWaveLevel);
        UpdatePlayerStatsTexts();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void UpdatePlayerStatsTexts()
    {
        HealthText.text = "♥" + Health.ToString();
        ArmorText.text = "Armor %" + Armor.ToString();
        UpdateWaveText(Level);
    }

    public void TakeDamage(int damage)
    {
        if (Armor > 0)
        {
            Health -= Mathf.RoundToInt((float)damage * (1 - Armor));
        }
        else
        {
            Health -= damage;
        }

        UpdatePlayerStatsTexts();

        if (Health <= 0)
        {
            Health = 0;
            Invoke(nameof(GameOver), 0.2f);
        }
    }

    public void OnHealthPickedUp()
    {
        Health += 30;
        UpdatePlayerStatsTexts();
    }

    private void GameOver()
    {        
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().GameLost();

        //Destroy(gameObject);
    }

    public void UpdateWaveText(int level)
    {
        LevelText.text = "Waves Cleared: " + (level - 1) + " / " + 3;
    }

    public void FreezePlayer()
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        gameObject.GetComponent<FirstPersonMovement>().enabled = false;
        gameObject.GetComponent<Crouch>().enabled = false;
        gameObject.GetComponentInChildren<PistolShoot>().enabled = false;
    }
    
}
