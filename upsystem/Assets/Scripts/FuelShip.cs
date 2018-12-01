using System;
using UnityEngine;

class FuelShip : Ship
{
	public void Initialize(int StartingCrew, int StartingSupply, int StartingFuel, bool Damaged)
    {
        base.Initialize(StartingCrew, StartingSupply, StartingFuel, !Damaged, ShipType.Fuel, 4, 2, 12);
    }
}