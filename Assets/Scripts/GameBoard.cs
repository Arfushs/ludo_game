using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

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
    [SerializeField] private GridSquare greenPawnLastGrid;
    [SerializeField] private GridSquare redPawnLastGrid;
    [SerializeField] private GridSquare yellowPawnLastGrid;
    [SerializeField] private GridSquare bluePawnLastGrid;
    
    [Header("Locked Info")] 
    [SerializeField] private bool isGreenLockedOpen;
    [SerializeField] private bool isYellowLockedOpen;
    [SerializeField] private bool isBlueLockedOpen;
    [SerializeField] private bool isRedLockedOpen;

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
    

    private Animator _lockAnimator;
    
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

        _lockAnimator = GetComponent<Animator>();

    }
    

    private void OnEnable()
    {
        EventManager.OnPawnBroken += OnPawnBroken;
        EventManager.OnLockedOpen += OnLockedOpen;
    }

    private void OnDisable()
    {
        EventManager.OnPawnBroken -= OnPawnBroken;
        EventManager.OnLockedOpen -= OnLockedOpen;
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
                CheckIsLockedOpen(p.GetPawnColor()))
            {
                FillPathToLockedList(possiblePath,lockedList,dice);
                return possiblePath;
            }
            
            currentIndex = gridList.IndexOf(p.GetGrid());
            for (int i = 1; i <= dice; i++)
            {
                // Eger kilitler acık degilse
                if(!CheckIsLockedOpen(p.GetPawnColor()))
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
        // Eğer piyon kilidin üstündeyse
        else
        {
            currentIndex = lockedList.IndexOf(p.GetGrid());
            
            if (currentIndex + dice > lockedList.Count)
                return null;
            if (currentIndex + dice <= lockedList.Count)
            {
                for (int i = 1; i <= dice; i++)
                {
                    if(currentIndex + i < lockedList.Count)
                        possiblePath.Add(lockedList[(currentIndex + i)]);
                }

                if (CheckPawnCanGoEnd(p, lockedList))
                {
                    AddLastGridToThePath(p.GetPawnColor(),possiblePath);
                    p.DoOnEnd();
                }
            }
            
        }
        
        return possiblePath;
    }

    private bool CheckPawnCanGoEnd(Pawn p,List<GridSquare> lockedList)
    {
        if (p.GetGrid() == lockedList[lockedList.Count - 1])
            return true;
        return false;
    }

    private void AddLastGridToThePath(TeamColor color, List<GridSquare> path)
    {
        switch (color)
        {
            case TeamColor.RED:
                path.Add(redPawnLastGrid);
                break;
            case TeamColor.GREEN:
                path.Add(greenPawnLastGrid);
                break;
            case TeamColor.YELLOW:
                path.Add(yellowPawnLastGrid);
                break;
            case TeamColor.BLUE:
                path.Add(bluePawnLastGrid);
                break;
        }
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

    private bool CheckIsLockedOpen(TeamColor color)
    {
        bool b = false;
        switch (color)
        {
            case TeamColor.RED:
                b = isRedLockedOpen;
                break;
            case TeamColor.GREEN:
                b = isGreenLockedOpen;
                break;
            case TeamColor.YELLOW:
                b = isYellowLockedOpen;
                break;
            case TeamColor.BLUE:
                b = isBlueLockedOpen;
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
        if (!p.IsPassive() && !p.IsOnEnd())
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

    [Button("Turn Board")]
    public void TurnBoardAccordingColor(TeamColor color)
    {
        switch (color)
        {
            case TeamColor.BLUE:
                transform.DORotate(new Vector3(0,-90,0), 0.5f);
                break;
            case TeamColor.RED:
                transform.DORotate(new Vector3(0,180,0), 0.5f);
                break;
            case TeamColor.GREEN:
                isGreenLockedOpen = true;
                transform.DORotate(new Vector3(0,90,0), 0.5f);
                break;
            case TeamColor.YELLOW:
                transform.DORotate(new Vector3(0,0,0), 0.5f);
                break; 
        }
    }

    [Button("Unlock Locked Path")]
    public void OnLockedOpen(TeamColor color)
    {
        switch (color)
        {
            case TeamColor.BLUE:
                isBlueLockedOpen = true;
                _lockAnimator.Play("blueLockedOpen");
                break;
            case TeamColor.RED:
                isRedLockedOpen = true;
                _lockAnimator.Play("redLockedOpen");
                break;
            case TeamColor.GREEN:
                isGreenLockedOpen = true;
                _lockAnimator.Play("greenLockedOpen");
                break;
            case TeamColor.YELLOW:
                isYellowLockedOpen = true;
                _lockAnimator.Play("yellowLockedOpen");
                break; 
        }
    }
    [Button("Lock Locked Path")]
    public void CloseLocked(TeamColor color)
    {
        switch (color)
        {
            case TeamColor.BLUE:
                isBlueLockedOpen = false;
                _lockAnimator.Play("blueLockedLock");
                break;
            case TeamColor.RED:
                isRedLockedOpen = false;
                _lockAnimator.Play("redLockedLock");
                break;
            case TeamColor.GREEN:
                isGreenLockedOpen = false;
                _lockAnimator.Play("greenLockedLock");
                break;
            case TeamColor.YELLOW:
                isYellowLockedOpen = false;
                _lockAnimator.Play("yellowLockedLock");
                break; 
        }
    }
    
    
}
