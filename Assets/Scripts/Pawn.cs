using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    private GridSquare currentGridSquare = null;

    private TeamColor pawnColor;
    public void Initialize(GridSquare gridSquare, TeamColor color)
    {
        transform.position = gridSquare.GetPosition();
        currentGridSquare = gridSquare;
        gridSquare.RegisterPawn(this);
        pawnColor = color;
    }

    public GridSquare GetGrid() => currentGridSquare;
    public TeamColor GetPawnColor() => pawnColor;

    public IEnumerator Move(List<GridSquare> path)
    {
        float duration = .2f;
        currentGridSquare.UnRegisterPawn(this);
        foreach (GridSquare grid in path)
        {
            Vector3 pos = grid.GetPosition();
            yield return transform.DOJump(pos, .2f, 1, duration).WaitForCompletion();
            currentGridSquare = grid;
        }
        currentGridSquare.RegisterPawn(this);
    }
}
