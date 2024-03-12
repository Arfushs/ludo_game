using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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

    [Header("Pawn Info")] 
    [SerializeField] private Pawn testPawn;
    
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

    private void Start()
    {
        testPawn.Initialize(gridList[0],TeamColor.BLUE);
    }

    [Button("Move Pawn")]
    public void TestFunc(int dice)
    {
        testPawn.StartCoroutine(testPawn.Move(GetPawnPossiblePath(testPawn,dice)));
    }

    private List<GridSquare> GetPawnPossiblePath(Pawn p, int dice)
    {
        List<GridSquare> possiblePath = new List<GridSquare>();
        int currentIndex = gridList.IndexOf(p.GetGrid());
        if (!CheckIsLocked(p.GetPawnColor()))
        {
            for (int i = 1; i <= dice; i++)
            {
                possiblePath.Add(gridList[(currentIndex + i)%gridList.Count]);
            }
        }
        else
        {
            // TODO Kilitlerin açık olma durumunu yaz
        }

        return possiblePath;
    }

    private bool CheckIsLocked(TeamColor color)
    {
        bool b = false;
        switch (color)
        {
            case TeamColor.RED:
                b = isRedLocked;
                break;
            case TeamColor.GREEN:
                b = isGreenLocked;
                break;
            case TeamColor.YELLOW:
                b = isYellowLocked;
                break;
            case TeamColor.BLUE:
                b = isBlueLocked;
                break;
        }
        return b;
    }
    
}
