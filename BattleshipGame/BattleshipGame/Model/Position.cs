using System;
using System.Collections.Generic;
using System.Text;

namespace BattleshipGame.Model
{
    public class Position
    {
        public int Row { get; set; }

        public int Column { get; set; }

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public static bool operator==(Position a, Position b)
        {
            return a.Row == b.Row && a.Column == b.Column;
        }

        public static bool operator!=(Position a, Position b)
        {
            return !(a == b);
        }
    }
}
