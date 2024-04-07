using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void PawnBrokenEvent(Pawn p);

    public delegate void LockedOpenEvent(TeamColor color);

    public static PawnBrokenEvent OnPawnBroken;
    public static LockedOpenEvent OnLockedOpen;
}
