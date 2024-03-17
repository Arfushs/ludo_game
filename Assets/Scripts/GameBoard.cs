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
    [SerializeField] private GridSquare greenInitialGrid;
    [SerializeField] private GridSquare redInitialGrid;
    [SerializeField] private GridSquare yellowInitialGrid;
    [SerializeField] private GridSquare blueInitialGrid;
    
    [Header("Locked Info")] 
    [SerializeField] private bool isGreenLocked;
    [SerializeField] private bool isYellowLocked;
    [SerializeField] private bool isBlueLocked;
    [SerializeField] private bool isRedLocked;

    [Header("Pawn Info")] 
    [SerializeField] private List<Pawn> bluePawnsList = new List<Pawn>();
    [SerializeField] private List<Pawn> redPawnsList = new List<Pawn>();
    [SerializeField] private List<Pawn> yellowPawnsList = new List<Pawn>();
    [SerializeField] private List<Pawn> greenPawnsList = new List<Pawn>();
    [Space(10)]
    [SerializeField] private List<Transform> bluePawnsPosList = new List<Transform>();
    [SerializeField] private List<Transform> redPawnsPosList = new List<Transform>();
    [SerializeField] private List<Transform> yellowPawnsPosList = new List<Transform>();
    [SerializeField] private List<Transform> greenPawnsPosList = new List<Transform>();
    
    
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
        PutPawnsIntoInitialPositions();
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

    // Pawnları Başlangıc pozisyonuna koyar
    private void PutPawnsIntoInitialPositions(List<Pawn> pawnList,List<Transform> posList)
    {
        foreach (Pawn p in pawnList)
        {
            p.transform.position = posList[p.pawnIndex].position;
        }
        
    }

    private void PutPawnsIntoInitialPositions()
    {
        PutPawnsIntoInitialPositions(bluePawnsList,bluePawnsPosList);
        PutPawnsIntoInitialPositions(redPawnsList,redPawnsPosList);
        PutPawnsIntoInitialPositions(yellowPawnsList,yellowPawnsPosList);
        PutPawnsIntoInitialPositions(greenPawnsList,greenPawnsPosList);
    }

    #region Utility Functions
    
    [Button("Put Pawn Into Game")]
    public void PutPawnIntoGame(TeamColor color)
    {
        switch (color)
        {
            case TeamColor.BLUE:
                FindPassivePawn(bluePawnsList)?.Initialize(blueInitialGrid);
                break;
            case TeamColor.RED:
                FindPassivePawn(redPawnsList)?.Initialize(redInitialGrid);
                break;
            case TeamColor.GREEN:
                FindPassivePawn(greenPawnsList)?.Initialize(greenInitialGrid);
                break;
            case TeamColor.YELLOW:
                FindPassivePawn(yellowPawnsList)?.Initialize(yellowInitialGrid);
                break;
        }
    }

    private Pawn FindPassivePawn(List<Pawn> pawns)
    {
        foreach (Pawn p in pawns)
        {
            if (p.IsPassive())
                return p;
        }
        return null;
    }
    
    [Button("Move Pawn")]
    public void MovePawn(Pawn p,int dice)
    {
        if (!p.IsPassive())
        {
            p.StartCoroutine(p.Move(GetPawnPossiblePath(p,dice)));
        }
        else
        {
            Debug.LogWarning("This pawn is passive cannot be move !");
        }
    }

    [Button("DO Pawn Passive")]
    public void DoPassivePawn(Pawn p)
    {
        switch (p.GetPawnColor())
        {
            case TeamColor.BLUE:
                p.DoPassive(bluePawnsPosList[p.pawnIndex]);
                break;
            case TeamColor.RED:
                p.DoPassive(redPawnsPosList[p.pawnIndex]);
                break;
            case TeamColor.GREEN:
                p.DoPassive(greenPawnsPosList[p.pawnIndex]);
                break;
            case TeamColor.YELLOW:
                p.DoPassive(yellowPawnsPosList[p.pawnIndex]);
                break;
        }
    }

    #endregion
    
    
}
