using System;
using System.Collections.Generic;
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

        public int ShipSize { get; private set; }

        public Ship(ShipType shipType, List<Position> shipPositions)
        {
            TypeOfShip = shipType;
            ShipPositions = new List<Position>();
            foreach (var position in shipPositions)
            {
                if (IsShipPositionValid(position))
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
                    ShipSize = AircraftCarrierSize;
                    break;
                case ShipType.Battleship:
                    ShipSize = BattleshipSize;
                    break;
                case ShipType.Cruiser:
                    ShipSize = CruiserSize;
                    break;
                case ShipType.Submarine:
                    ShipSize = SubmarineSize;
                    break;
                case ShipType.Destroyer:
                    ShipSize = DestroyerSize;
                    break;
            }
        }

        private bool IsShipPositionValid(Position shipPosition)
        {
            return shipPosition.Row >= 0
                && shipPosition.Column >= 0
                && shipPosition.Row < BattleshipGame.BoardSize
                && shipPosition.Column < BattleshipGame.BoardSize;
        }

    }
}
