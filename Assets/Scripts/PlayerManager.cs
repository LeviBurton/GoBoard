using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerDeath))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMover))]
public class PlayerManager : TurnManager
{
    public PlayerMover playerMover;
    public PlayerInput playerInput;

    public UnityEvent deathEvent;

    Board m_board;

    protected override void Awake()
    {
        base.Awake();

        playerInput = GetComponent<PlayerInput>();
        playerMover = GetComponent<PlayerMover>();

        m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();

        playerInput.InputEnabled = true;
    }

    void Update()
    {
        if (playerMover.isMoving || m_gameManager.CurrentTurn != Turn.Player)
        {
            return;
        }

        playerInput.GetKeyInput();

        if (playerInput.V == 0)
        {
            if (playerInput.H < 0)
            {
                playerMover.MoveLeft();
            }
            else if (playerInput.H > 0)
            {
                playerMover.MoveRight();
            }
        }
        else if (playerInput.H == 0)
        {
            if (playerInput.V < 0)
            {
                playerMover.MoveBackward();
            }
            else if (playerInput.V > 0)
            {
                playerMover.MoveForward();
            }
        }
    }

    public void Die()
    {
        if (deathEvent != null)
        {
            deathEvent.Invoke();
        }
    }

    void CaptureEnemies()
    {
        if (m_board != null)
        {
            List<EnemyManager> enemies = m_board.FindEnemiesAt(m_board.PlayerNode);

            if (enemies.Count != 0)
            {
                foreach (var enemy in enemies)
                {
                    if (enemy != null)
                    {
                        enemy.Die();
                    }
                }
            }
        }
    }

    public override void FinishTurn()
    {
        CaptureEnemies();
        base.FinishTurn();
    }
}
