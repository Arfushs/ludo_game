using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private Transform gridSquareContainer;
    private List<GridSquare> gridList = new List<GridSquare>();

    private void Awake()
    {
        GridSquare[] allSquares = gridSquareContainer.GetComponentsInChildren<GridSquare>();
        int counter = 1;
        foreach (GridSquare grid in allSquares)
        {
            grid.name = "Squre: " + counter; 
            gridList.Add(grid);
            counter++;
        }
        
    }
}
