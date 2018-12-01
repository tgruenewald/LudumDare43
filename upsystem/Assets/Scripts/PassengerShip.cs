using System;
using UnityEngine;

class PassengerShip : Ship
{
    public void initialize(int StartingCrew, int StartingSupply, int StartingFuel, bool Damaged)
    {
        base.Initialize(StartingCrew, StartingSupply, StartingFuel, !Damaged, ShipType.Passenger, 8, 2, 2);
    }
} 