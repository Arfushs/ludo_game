using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void PawnBrokenEvent(Pawn p);

    public static PawnBrokenEvent OnPawnBroken;
}
