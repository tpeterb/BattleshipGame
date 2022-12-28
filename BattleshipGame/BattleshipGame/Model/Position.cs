using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace BattleshipGame.Model
{
    public class Position : IEquatable<Position>
    {
        public int Row { get; set; }

        public int Column { get; set; }

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public Position(Position position)
        {
            this.Row = position.Row;
            this.Column = position.Column;
        }

        public Position GetPositionInDirection(Direction direction)
        {
            return direction switch
            {
                Direction.Up => new Position(Row - 1, Column),
                Direction.Down => new Position(Row + 1, Column),
                Direction.Right => new Position(Row, Column + 1),
                Direction.Left => new Position(Row, Column - 1),
                _ => new Position(-1, -1)
            };
        }

        public static bool operator ==(Position a, Position b)
        {
            return a.Row == b.Row && a.Column == b.Column;
        }

        public static bool operator !=(Position a, Position b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Position);
        }

        public bool Equals(Position other)
        {
            if (other is null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Row == other.Row && Column == other.Column;
        }

        public override int GetHashCode()
        {
            return (Row, Column).GetHashCode();
        }

        public override string ToString()
        {
            return "["+Row.ToString() + "-" + Column.ToString()+"]";
        }
    }
}
