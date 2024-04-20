using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zmeyka_555.ViewModels;

namespace Zmeyka_555.Models
{
    internal class Snake
    {
        public Queue<CellVM> SnakeCells { get; } = new Queue<CellVM>();


        private List<List<CellVM>> _allCells;

        private CellVM _start;

        private Action _denerateFood;
        
        private MainWndVM _score;
        public Snake( List<List<CellVM>> allCels, CellVM start, Action denerateFood) 
        { 
            _start = start;
            _allCells = allCels;
            _denerateFood = denerateFood;
            _start.CellType = CellType.Snake;
            SnakeCells.Enqueue(_start);
        }

        public void Restart()
        {
            foreach (var cell in SnakeCells)
            {
                cell.CellType = CellType.None;
            }
            SnakeCells.Clear();
            _start.CellType = CellType.Snake;
            SnakeCells.Enqueue(_start);

        }

        public void Move(MoveDirection direction)
        { 
            var leaderCell = SnakeCells.Last();

            int nextRow = leaderCell.Row;
            int nextColomn = leaderCell.Column;

            switch (direction)
            {
                case MoveDirection.Up:
                    nextRow--;
                    break;
                case MoveDirection.Down:
                    nextRow++;
                    break;
                case MoveDirection.Left:
                    nextColomn--;
                    break;
                case MoveDirection.Right:
                    nextColomn++;
                    break;
                default: 
                    break;
            }

            try
            {

                var nextCell = _allCells[nextRow][nextColomn];

                switch (nextCell?.CellType)
                {
                    case CellType.None:
                        nextCell.CellType = CellType.Snake;
                        SnakeCells.Dequeue().CellType = CellType.None;
                        SnakeCells.Enqueue(nextCell);
                        break;
                    case CellType.Food:
                        nextCell.CellType = CellType.Snake;
                        SnakeCells.Enqueue(nextCell);
                        _denerateFood?.Invoke();
                        break;
                    default:
                        throw new Exception("GameOver lol!!! "); 
                }
            }
            catch (ArgumentOutOfRangeException )
            {
                throw new Exception("GameOver lol");
            }

        }

    }
}
