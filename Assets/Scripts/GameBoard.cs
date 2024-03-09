using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [Header("Grids Info")]
    [SerializeField] private Transform gridSquareContainer;
    private List<GridSquare> gridList = new List<GridSquare>();

    [Header("Locked Info")] 
    [SerializeField] private bool isGreenLocked;
    [SerializeField] private bool isYellowLocked;
    [SerializeField] private bool isBlueLocked;
    [SerializeField] private bool isRedLocked;

    private void Awake()
    {
        // Square Gridleri gridList listesine doldurur.
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
