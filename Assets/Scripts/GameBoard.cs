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
    [SerializeField] private List<GridSquare> greenLockedGridList = new List<GridSquare>();
    [SerializeField] private List<GridSquare> blueLockedGridList = new List<GridSquare>();
    [SerializeField] private List<GridSquare> redLockedGridList = new List<GridSquare>();
    [SerializeField] private List<GridSquare> yellowLockedGridList = new List<GridSquare>();
    
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

    private void OnEnable()
    {
        EventManager.OnPawnBroken += OnPawnBroken;
    }

    private void OnDisable()
    {
        EventManager.OnPawnBroken -= OnPawnBroken;
    }

    private void Start()
    {
        PutPawnsIntoInitialPositions();
    }
    
    private List<GridSquare> GetPawnPossiblePath(Pawn p, int dice)
    {
        List<GridSquare> possiblePath = new List<GridSquare>();
        int currentIndex;
        List<GridSquare> lockedList = GetLockedList(p.GetPawnColor());

        if (!p.GetGrid().IsLock)
        {
            // Eger piyon yıldızın ustundeyse ve kilitler acıksa
            if (p.GetGrid().IsStar && p.GetPawnColor() == p.GetGrid().StarColor &&
                CheckIsLocked(p.GetPawnColor()))
            {
                FillPathToLockedList(possiblePath,lockedList,dice);
                return possiblePath;
            }
            
            currentIndex = gridList.IndexOf(p.GetGrid());
            for (int i = 1; i <= dice; i++)
            {
                // Eger kilitler acık degilse
                if(!CheckIsLocked(p.GetPawnColor()))
                    possiblePath.Add(gridList[(currentIndex + i)%gridList.Count]);
                else
                {
                    // kilitler acıksa
                    possiblePath.Add(gridList[(currentIndex + i)%gridList.Count]);
                    
                    if (gridList[(currentIndex + i) % gridList.Count].IsStar
                        && gridList[(currentIndex + i) % gridList.Count].StarColor == p.GetPawnColor())
                    {
                        FillPathToLockedList(possiblePath,lockedList,dice - i);
                        return possiblePath;
                    }
                }
            }
        }
        else
        {
            currentIndex = lockedList.IndexOf(p.GetGrid());
            for (int i = 1; i <= dice; i++)
            {
                possiblePath.Add(lockedList[(currentIndex + i)%lockedList.Count]);
            }
        }
        
        return possiblePath;
    }

    private void FillPathToLockedList(List<GridSquare> path, List<GridSquare> lockedList, int count)
    {
        for (int i = 0; i < count; i++)
        {
            path.Add(lockedList[i]);
        }
    }

    private List<GridSquare> GetLockedList(TeamColor color)
    {
        switch (color)
        {
            case TeamColor.BLUE:
                return blueLockedGridList;
            case TeamColor.RED:
                return redLockedGridList;
            case TeamColor.GREEN:
                return greenLockedGridList;
            case TeamColor.YELLOW:
                return yellowLockedGridList;
        }

        return null;
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

    private void OnPawnBroken(Pawn p)
    {
        DoPassivePawn(p);
    }
    
    
}
