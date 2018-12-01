using System;
using UnityEngine;

class SupplyShip : Ship
{
    public void Initialize(int StartingCrew, int StartingSupply, int StartingFuel, bool Damaged)
    {
        base.Initialize(StartingCrew, StartingSupply, StartingFuel, !Damaged, ShipType.Supply, 4, 32, 2);
    }
}