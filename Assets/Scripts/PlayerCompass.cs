using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCompass : MonoBehaviour
{
    public GameObject arrowPrefab;
    public float scale = 1f;
    public float startDistance = 0.25f;
    public float endDistance = 0.5f;
    public float moveTime = 1f;
    public iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;
    public float delay = 0f;

    List<GameObject> m_arrows = new List<GameObject>();
    Board m_board;

    private void Awake()
    {
        m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();

        SetupArrows();
    }

    void SetupArrows()
    {
        if (arrowPrefab == null)
        {
            Debug.Log("No arrow prefab!");
            return;
        }

        foreach (var dir in Board.directions)
        {
            var dirVector = new Vector3(dir.normalized.x, 0f, dir.normalized.y);
            Quaternion rotation = Quaternion.LookRotation(dirVector);
            GameObject arrowInstance = Instantiate(
                arrowPrefab, 
                transform.position + dirVector * startDistance, 
                rotation);
            arrowInstance.transform.localScale = new Vector3(scale, scale, scale);
            arrowInstance.transform.parent = transform;
            m_arrows.Add(arrowInstance);
        }
    }

    void MoveArrow(GameObject arrowInstance)
    {
        iTween.MoveBy(arrowInstance, iTween.Hash(
            "z", endDistance - startDistance,
            "looptype", iTween.LoopType.loop,
            "time", moveTime,
            "easetype", easeType
        ));
    }

    void MoveArrows()
    {
        foreach (var arrow in m_arrows)
        {
            MoveArrow(arrow);
        }
    }

    public void ShowArrows(bool state)
    {
        if (m_board == null)
        {
            Debug.Log("PlayerCompass: Error no board found!");
            return;
        }
        if (m_arrows == null)
        {
            Debug.Log("PlayerCompass: Error no arrows found!");
            return;
        }

        if (m_board.PlayerNode != null)
        {
            for (int i = 0; i < Board.directions.Length; i++)
            {
                Node neighbor = m_board.PlayerNode.FindNeighborAt(Board.directions[i]);
                if (neighbor == null || !state)
                {
                    m_arrows[i].SetActive(false);
                }
                else
                {
                    bool activeState = m_board.PlayerNode.LinkedNodes.Contains(neighbor);
                    m_arrows[i].SetActive(activeState);
                }
            }
        }

        ResetArrows();
        MoveArrows();
    }

    void ResetArrows()
    {
        for (int i = 0; i < Board.directions.Length; i++)
        {
            iTween.Stop(m_arrows[i]);
            Vector3 dirVector = new Vector3(Board.directions[i].normalized.x, 0f, Board.directions[i].normalized.y);
            m_arrows[i].transform.position = transform.position + dirVector * startDistance;
        }
    }
}
