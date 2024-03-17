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

    public void RegisterPawn(Pawn pawn)
    {
        pawnList.Add(pawn);
        CheckCanBrokeAnyPawn(pawn);
        ReadjustPawnPositions();
        //Debug.Log("This pawn '"+pawn.name+ "'" + " registered on " + name);
    }

    public void UnRegisterPawn(Pawn pawn)
    {
        pawnList.Remove(pawn);
        ReadjustPawnPositions();
        //Debug.Log("This pawn '"+pawn.name+ "'" + " unregistered on " + name);
    }

   
    public Vector3 GetPosition()
    {
        return onePawnPositionList[0].position;
    }

    private void ReadjustPawnPositions()
    {
        switch (pawnList.Count)
        {
            case 1:
                pawnList[0].transform.position = onePawnPositionList[0].position;
                break;
            case 2:
                pawnList[0].transform.position = twoPawnPositionList[0].position;
                pawnList[1].transform.position = twoPawnPositionList[1].position;
                break;
            case 3:
                pawnList[0].transform.position = threePawnPositionList[0].position;
                pawnList[1].transform.position = threePawnPositionList[1].position;
                pawnList[2].transform.position = threePawnPositionList[2].position;
                break;
            case 4:
                pawnList[0].transform.position = fourPawnPositionList[0].position;
                pawnList[1].transform.position = fourPawnPositionList[1].position;
                pawnList[2].transform.position = fourPawnPositionList[2].position;
                pawnList[3].transform.position = fourPawnPositionList[3].position;
                break;
        }
    }

    private void CheckCanBrokeAnyPawn(Pawn p)
    {
        // TODO Kırılabilecek piyon var mı register ederken bak
    }
}
