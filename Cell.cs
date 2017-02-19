using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _15
{
    public class Cell
    {
        public int Row
        {
            get;
            private set;
        }
        
        public int Column
        {
            get;
            private set;
        }

        public int Number
        {
            get;
            set;
        }

        public Cell(int row, int column,int number)
        {
            Row = row;
            Column = column;
            Number = number;
        }
    }
}
