using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    [Header("Pawn Visuals")] 
    [SerializeField] private GameObject yellowPawn;
    [SerializeField] private GameObject greenPawn;
    [SerializeField] private GameObject bluePawn;
    [SerializeField] private GameObject redPawn;
    
    private GridSquare _currentGridSquare = null;
    
    [Header("Pawn Info")]
    [SerializeField] private TeamColor _pawnColor;
    public int pawnIndex;
    
    private bool _isPassive = true;

    private void Awake()
    {
        SetPawnVisual();
    }

    public void Initialize(GridSquare gridSquare)
    {
        _currentGridSquare = gridSquare;
        gridSquare.RegisterPawn(this);
        _isPassive = false;
    }

    public GridSquare GetGrid() => _currentGridSquare;
    public TeamColor GetPawnColor() => _pawnColor;
    public bool IsPassive() => _isPassive;

    public IEnumerator Move(List<GridSquare> path)
    {
        float duration = .2f;
        _currentGridSquare.UnRegisterPawn(this);
        foreach (GridSquare grid in path)
        {
            Vector3 pos = grid.GetPosition();
            yield return transform.DOJump(pos, .2f, 1, duration).WaitForCompletion();
            _currentGridSquare = grid;
        }
        _currentGridSquare.RegisterPawn(this);
    }

    private void SetPawnVisual()
    {
        CloseAllVisuals();
        switch (_pawnColor)
        {
            case TeamColor.BLUE:
                bluePawn.SetActive(true);
                break;
            case TeamColor.RED:
                redPawn.SetActive(true);
                break;
            case TeamColor.YELLOW:
                yellowPawn.SetActive(true);
                break;
            case TeamColor.GREEN:
                greenPawn.SetActive(true);
                break;
        }
    }

    // Tum pawn meshlerini kapatır
    private void CloseAllVisuals()
    {
        yellowPawn.SetActive(false);
        greenPawn.SetActive(false);
        bluePawn.SetActive(false);
        redPawn.SetActive(false);
    }

    
    public void DoPassive(Transform pos)
    {
        _isPassive = true;
        _currentGridSquare.UnRegisterPawn(this);
        transform.DOJump(pos.position, .5f, 1, 1);
    }
}
