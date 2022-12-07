using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleshipGame.Model
{
    public class Ship
    {

        public const int AircraftCarrierSize = 5;

        public const int BattleshipSize = 4;

        public const int SubmarineSize = 3;

        public const int CruiserSize = 3;

        public const int DestroyerSize = 2;

        public ShipType TypeOfShip { get; set; }

        public List<Position> ShipPositions { get; set; }

        public int OriginalShipSize { get; private set; }

        public bool Destroyed { get; set; }

        public Ship(ShipType shipType, List<Position> shipPositions)
        {
            TypeOfShip = shipType;
            ShipPositions = new List<Position>();
            Destroyed = false;
            foreach (var position in shipPositions)
            {
                if (BattleshipGame.IsPositionValid(position))
                {
                    ShipPositions.Add(position);
                } else
                {
                    throw new ArgumentException("The ship's position is invalid, does not fit on the board!");
                }
            }
            switch (shipType)
            {
                case ShipType.AircraftCarrier:
                    OriginalShipSize = AircraftCarrierSize;
                    break;
                case ShipType.Battleship:
                    OriginalShipSize = BattleshipSize;
                    break;
                case ShipType.Cruiser:
                    OriginalShipSize = CruiserSize;
                    break;
                case ShipType.Submarine:
                    OriginalShipSize = SubmarineSize;
                    break;
                case ShipType.Destroyer:
                    OriginalShipSize = DestroyerSize;
                    break;
            }
        }

        public Ship(Ship ship)
        {
            this.TypeOfShip = ship.TypeOfShip;
            this.OriginalShipSize = ship.OriginalShipSize;
            this.ShipPositions = ship.ShipPositions.Select(position => new Position(position)).ToList();
            this.Destroyed = ship.Destroyed;
        }

        public int GetCurrentShipSize()
        {
            return ShipPositions.Count;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            Ship other = obj as Ship;
            return (other != null)
                && TypeOfShip == other.TypeOfShip
                && Destroyed == other.Destroyed
                && OriginalShipSize == other.OriginalShipSize
                && ShipPositions.SequenceEqual(other.ShipPositions);
        }

        public override int GetHashCode()
        {
            return (TypeOfShip, ShipPositions, OriginalShipSize, Destroyed).GetHashCode();
        }
    }
}
