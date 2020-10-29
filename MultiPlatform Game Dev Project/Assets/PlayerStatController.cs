using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
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
        Health *= Level;
        Armor *= Level;
        UpdatePlayerStatsTexts();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void UpdatePlayerStatsTexts()
    {
        HealthText.text = "♥" + Health.ToString();
        ArmorText.text = "Armor" + Armor.ToString();
        LevelText.text = "Level " + Level;
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

    private void GameOver()
    {       
        Destroy(gameObject);        
    }
    
}
