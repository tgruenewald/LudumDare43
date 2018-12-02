using System;
using UnityEngine;

class SupplyShip : Ship
{
    private String _name = "Supply Ship";
    public override String Name { get { return _name; } set { this._name += " " + value; } }

    public void Initialize(int StartingCrew, int StartingSupply, int StartingFuel, bool Damaged)
    {
        // currently handled in inspector.
    }
}