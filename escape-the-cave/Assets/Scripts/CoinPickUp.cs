using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    [SerializeField] AudioClip coinPickUpSFX;

    
    [SerializeField] int scoreToAdd = 1;

    GameSession gameSession;
    bool coinConsumed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {      
        
        if(other.gameObject.GetComponent<Player>() != null)
        {
            PlayCoinPickUpSFX();

            if (!coinConsumed)
            {
                GiveScoreToPlayer();
                coinConsumed = true;
            }           
            
            Destroy(gameObject);
        }
    }

    private void GiveScoreToPlayer()
    {
        gameSession = FindObjectOfType<GameSession>();

        if (gameSession != null)
        {
            gameSession.AddToScore(scoreToAdd);
        }
        coinConsumed = true;
    }

    private void PlayCoinPickUpSFX()
    {
        AudioSource.PlayClipAtPoint(coinPickUpSFX, Camera.main.transform.position);
    }
}
