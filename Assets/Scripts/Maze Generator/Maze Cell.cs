using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField] private GameObject _leftWall;
    [SerializeField] private GameObject _rightWall;
    [SerializeField] private GameObject _frontWall;
    [SerializeField] private GameObject _backWall;

    [SerializeField] private GameObject firstLootbox;
    [SerializeField] private GameObject secondLootbox;

    public bool IsVisited { get; private set; }

    private void Start()
    {
        if(transform.position.x >= -280 && transform.position.x <= 280 && transform.position.z >= -280 && transform.position.z <= 280)
        {
            ClearBackWall();
            ClearFrontWall();
            ClearLeftWall();
            ClearRightWall();
            return;
        }
        
        
        if(transform.position.x >= 250 && transform.position.x <= 330 && transform.position.z >= -18 && transform.position.z <= 18)
        {
            ClearBackWall();
            ClearFrontWall();
            ClearLeftWall();
            ClearRightWall();
            return;
        }
        
        if(transform.position.x <= -250 && transform.position.x >= -330 && transform.position.z >= -18 && transform.position.z <= 18)
        {
            ClearBackWall();
            ClearFrontWall();
            ClearLeftWall();
            ClearRightWall();
            return;
        }
        
        if(transform.position.x >= -18 && transform.position.x <= 18 && transform.position.z >= 250 && transform.position.z <= 330)
        {
            ClearBackWall();
            ClearFrontWall();
            ClearLeftWall();
            ClearRightWall();
            return;
        }
        
        if(transform.position.x >= -18 && transform.position.x <= 18 && transform.position.z <= -250 && transform.position.z >= -330)
        {
            ClearBackWall();
            ClearFrontWall();
            ClearLeftWall();
            ClearRightWall();
            return;
        }
    }

    public void Visit()
    {
        IsVisited = true;
    }

    public void ClearLeftWall()
    {
        _leftWall.SetActive(false);
    }
    public void ClearRightWall()
    {
        _rightWall.SetActive(false);
    }
    public void ClearFrontWall()
    {
        _frontWall.SetActive(false);
    }
    public void ClearBackWall()
    {
        _backWall.SetActive(false);
    }
}
