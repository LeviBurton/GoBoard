using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mover : MonoBehaviour
{
    // where are we currently headed
    public Vector3 destination;

    // should we rotate to face destination?
    public bool faceDestination = false;

    // are we currently moving?
    public bool isMoving = false;
    
    // what easetype to use for iTweening
    public iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;

    // how fast we move
    public float moveSpeed = 1.5f;

    // time to rate to face direction
    public float rotateTime = 0.5f;

    // delay to use before any call to iTween
    public float itweenDelay = 0f;

    // reference to Board
    protected Board m_board;

    Node currentNode;
    public Node CurrentNode { get => currentNode;  }

    public UnityEvent finishMovementEvent;

    protected virtual void Awake()
    {
        m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
    }

    protected virtual void Start()
    {
        UpdateCurrentNode();
    }

    public void Move(Vector3 destinationPos, float delayTime = 0.25f)
    {
        if (isMoving)
        {
            return;
        }

        if (m_board != null)
        {
            Node targetNode = m_board.FindNodeAt(destinationPos);

            if (targetNode != null && CurrentNode != null)
            {
                if (CurrentNode.LinkedNodes.Contains(targetNode))
                {
                    StartCoroutine(MoveRoutine(destinationPos, delayTime));
                }
            }
            else
            {
                Debug.LogWarning("Move: target node or current node are null!");
            }
        }
    }

    protected virtual IEnumerator MoveRoutine(Vector3 destinationPos, float delayTime)
    {
        isMoving = true;
        destination = destinationPos;

        if (faceDestination)
        {
            yield return new WaitForSeconds(0.25f);
            FaceDestination();
        }

        yield return new WaitForSeconds(delayTime);

        iTween.MoveTo(gameObject, iTween.Hash(
            "x", destinationPos.x,
            "y", destinationPos.y,
            "z", destinationPos.z,
            "delay", itweenDelay,
            "easeType", easeType,
            "speed", moveSpeed
        ));

        while (Vector3.Distance(destinationPos, transform.position) > 0.01f)
        {
            yield return null;
        }

        iTween.Stop(gameObject);

        transform.position = destinationPos;

        isMoving = false;

        UpdateCurrentNode();
    }

    public void MoveLeft()
    {
        var newPosition = transform.position + new Vector3(-Board.spacing, 0f, 0f);
        Move(newPosition, 0);
    }

    public void MoveRight()
    {
        var newPosition = transform.position + new Vector3(Board.spacing, 0f, 0f);
        Move(newPosition, 0);
    }

    public void MoveForward()
    {
        var newPosition = transform.position + new Vector3(0f, 0f, Board.spacing);
        Move(newPosition, 0);
    }

    public void MoveBackward()
    {
        var newPosition = transform.position + new Vector3(0f, 0f, -Board.spacing);
        Move(newPosition, 0);
    }

    protected void UpdateCurrentNode()
    {
        if (m_board != null)
        {
            currentNode = m_board.FindNodeAt(transform.position);
        }
    }

    protected void FaceDestination()
    {
        Vector3 relativePosition = destination - transform.position;

        Quaternion newRotation = Quaternion.LookRotation(relativePosition, Vector3.up);

        float newY = newRotation.eulerAngles.y;

        iTween.RotateTo(gameObject, iTween.Hash(
            "y", newY,
            "delay", 0f,
            "easeType", easeType,
            "time", rotateTime
            ));
    }
}
