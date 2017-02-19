using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;

namespace _15
{
    public class Field
    {
        private int rowCount = 4;
        private int colCount = 4;
        private int zeroColumn = 0, zeroRow = 0;
        public int minStepsRequired
        {
            get;
            private set;
        }
        public int RowCount
        {
            get
            {
                return rowCount;
            }
        }

        public int ColCount
        {
            get
            {
                return colCount;
            }
        }
        /// <summary>
        /// Индексатор для удобного доступа
        /// </summary>
        /// <param name="i">Индекс строки</param>
        /// <param name="j">Индекс столбца</param>
        /// <returns></returns>
        public Cell this[int i, int j]
        {
            get
            {
                int index = i * colCount + j;
                return cells[index];
            }
            private set
            {
                int index = i * colCount + j;
                cells[index] = value;
            }
        }

        public bool isSolved
        {
            get
            {
                for(int i=0;i<cells.Count-1;i++)
                {
                    if (cells[i + 1].Number < cells[i].Number)
                    {
                        if (i + 1 == cells.Count - 1&&cells[i+1].Number==0)//если дошли до предпослетней клетки, а последняя пустая
                            return true;
                        return false;
                    } 
                }
                return true;
            }
        }

        private ObservableCollection<Cell> cells = new ObservableCollection<Cell>();
        public ObservableCollection<Cell> Cells
        {
            get
            {
                return cells;
            }
        }
        /// <summary>
        /// Меняет местами костяшку с нулевой костяшкой
        /// </summary>
        public void MoveCell(Cell cell)
        {
            int row = cell.Row;
            int col = cell.Column;
            int number = cell.Number;
            this[row, col] = new Cell(row,col,0);
            this[zeroRow, zeroColumn] = new Cell(zeroRow, zeroColumn, number);
            zeroColumn = col;
            zeroRow = row;
        }

       public Point MovingDirection(Cell cell)
       {
            int row = cell.Row, col = cell.Column;
            Point result = new Point(0, 0);
            if (row - 1 >= 0 && this[row - 1, col].Number == 0)//ячейка сверху
            {
                result = new Point(0, -1);
            }
            else if (col - 1 >= 0 && this[row, col - 1].Number == 0)//ячейка слева
                result = new Point(-1, 0);
            else if (row + 1 < rowCount && this[row + 1, col].Number == 0)//ячейка снизу
                result = new Point(0, 1);
            else if (col + 1 < colCount && this[row, col + 1].Number == 0)//ячейка справа
                result = new Point(1, 0);
            return result;
       }



        /// <summary>
        /// Перемешивает, делая случайные ходы
        /// </summary>
        /// <param name="iterations">Количество ходов</param>
        public void Shuffle(int iterations = 20)
        {
            //Cell prevCell = new Cell(-1, -1, -1);
            int prevNumber = -1;
            Point[] coords = new Point[4] { new Point(-1, 0), new Point(0, -1), new Point(1, 0), new Point(0, 1) };//приращения координат в 4 стороны
            Random r = new Random();
            for (int i=0;i<iterations;i++)
            {
                List<Cell> cellsAroundZeroCell = new List<Cell>();//костяшки, из которых будут выбираться перемещаемые
                foreach(Point pt in coords)
                {
                    int rowIndex = (int)pt.Y + zeroRow;
                    int colIndex = (int)pt.X + zeroColumn;
                    if(rowIndex>=0&&rowIndex<RowCount&&colIndex>=0&&colIndex<ColCount)
                    {
                        if (this[rowIndex, colIndex].Number != prevNumber)//чтобы не двигать одну костяшку туда и обратно
                            cellsAroundZeroCell.Add(this[rowIndex, colIndex]);
                    }
                }
                int rndIndex = r.Next(cellsAroundZeroCell.Count);
                prevNumber = cellsAroundZeroCell[rndIndex].Number;
                MoveCell(cellsAroundZeroCell[rndIndex]);
            }
        }

        public Field(int minShuffleIterations,int maxShuffleIterations)
        {
            for (int i = 0; i < rowCount * colCount; i++)
            {
                int row = i / RowCount;
                int col = i - (ColCount * row);
                cells.Add(new Cell(row,col,i));
            }
            int iters = new Random().Next(minShuffleIterations,maxShuffleIterations);
            Shuffle(iters);
        }
    }
}
