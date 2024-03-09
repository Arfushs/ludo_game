using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    private GridSquare currentGridSquare = null;


    public void Initialize(GridSquare gridSquare)
    {
        currentGridSquare = gridSquare;
        gridSquare.RegisterPawn(this);
    }
}
