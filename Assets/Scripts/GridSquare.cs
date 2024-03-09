using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSquare : MonoBehaviour
{
    [Header("Position Objects")]
    [SerializeField] private List<Transform> onePawnPositionList;
    [SerializeField] private List<Transform> twoPawnPositionList;
    [SerializeField] private List<Transform> threePawnPositionList;
    [SerializeField] private List<Transform> fourPawnPositionList;

    private List<Pawn> pawnList = new List<Pawn>();

    public void RegisterPawn(Pawn pawm)
    {
        pawnList.Add(pawm);
    }

    public void UnRegisterPawn(Pawn pawn)
    {
        pawnList.Remove(pawn);
    }
}
