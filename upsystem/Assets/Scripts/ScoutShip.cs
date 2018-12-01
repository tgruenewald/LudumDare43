using System;
using UnityEngine;

class ScoutShip : Ship
{
    public void Initialize(int StartingCrew, int StartingSupply, int StartingFuel, bool Damaged)
    {
        base.Initialize(StartingCrew, StartingSupply, StartingFuel, !Damaged, ShipType.Scout, 2, 2, 4);
    }
}