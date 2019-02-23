using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    Stationary,
    Patrol,
    Spinner
}

public class EnemyMover : Mover
{
    public Vector3 directionToMove = new Vector3(0f, 0f, Board.spacing);

    public MovementType movementType = MovementType.Stationary;

    public float standTime = 1f;

    protected override void Awake()
    {
        base.Awake();

        base.faceDestination = true;
    }

    protected override void Start()
    {
        base.Start();
    }

    public void MoveOneTurn()
    {
        switch (movementType)
        {
            case MovementType.Patrol:
                Patrol();
                break;

            case MovementType.Stationary:
                Stand();
                break;

            case MovementType.Spinner:
                Spin();
                break;
        }
    }

    void Patrol()
    {
        StartCoroutine(PatrolRoutine());
    }

    IEnumerator PatrolRoutine()
    {
        var startPos = new Vector3(CurrentNode.Coordinate.x, 0f, CurrentNode.Coordinate.y);

        // one space forward
        var newDest = startPos + transform.TransformVector(directionToMove);

        // two spaces forward
        var nextDest = startPos + transform.TransformVector(directionToMove * 2f);

        Move(newDest, 0f);

        while (isMoving)
        {
            yield return null;
        }

        if (m_board != null)
        {
            Node newDestNode = m_board.FindNodeAt(newDest);
            Node nextDestNode = m_board.FindNodeAt(nextDest);

            if (nextDestNode == null || !newDestNode.LinkedNodes.Contains(nextDestNode))
            {
                destination = startPos;
                FaceDestination();

                yield return new WaitForSeconds(rotateTime);
            }
        }

        base.finishMovementEvent.Invoke();
    }

    void Stand()
    {
        StartCoroutine(StandRoutine());
    }

    IEnumerator StandRoutine()
    {
        yield return new WaitForSeconds(standTime);

        base.finishMovementEvent.Invoke();
    }

    void Spin()
    {
        StartCoroutine(SpinRoutine());
    }

    IEnumerator SpinRoutine()
    {
        Vector3 localForward = new Vector3(0f, 0f, Board.spacing);
        destination = transform.TransformVector(localForward * -1f) + transform.position;

        FaceDestination();

        yield return new WaitForSeconds(rotateTime);

        base.finishMovementEvent.Invoke();
    }
}
