using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public Animator playerAnimController;

    public string playerDeathTrigger = "IsDead";

    void Awake()
    {
        playerAnimController = GetComponentInChildren<Animator>();
    }

    public void Die()
    {
        if (playerAnimController != null)
        {
            playerAnimController.SetTrigger(playerDeathTrigger);
        }
    }
}
