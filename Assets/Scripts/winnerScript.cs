using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class winnerScript : MonoBehaviour
{
    [SerializeField]
    private TMP_Text winningText = null;
    [SerializeField]
    private Image winningImage = null;

    [SerializeField] private AudioClip _audioClip;

    private void Start()
    {
        int winningPlayer = 0;
        int winningChestScore = 0;
        int winningSeedScore = 0;
        for (int i = 0; i < GameManager.instance.gamers.Length; i++)
        {
            if (GameManager.instance.gamers[i].treasure > winningChestScore)
            {
                winningPlayer = i;
                winningChestScore = GameManager.instance.gamers[i].treasure;
                winningSeedScore = GameManager.instance.gamers[i].seeds;
            }
            else if (GameManager.instance.gamers[i].treasure == winningChestScore &&
                     GameManager.instance.gamers[i].seeds > winningSeedScore)
            {
                winningPlayer = i;
                winningChestScore = GameManager.instance.gamers[i].treasure;
                winningSeedScore = GameManager.instance.gamers[i].seeds;
            }
        }
        
        winningText.SetText("PLAYER " + winningPlayer + " WON!!!");
        winningImage.sprite = GameManager.instance.gamers[winningPlayer].icon;
        
        if (AudioManager.instance != null && _audioClip != null) AudioManager.instance.PlaySound(_audioClip);
    }
}
