using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GameOfLifeInColor.Core.Interfaces;

namespace GameOfLifeInColor.Core.Models
{
    public class CellCollection: IEnumerable<Cell>, IEnumerator<Cell>
    {
        public event EventHandler CollectionUpdated;

        private Cell[,] _grid;
        
        public int Columns { get; private set; }
        public int Rows { get; private set; }
        
        public Cell this[int x, int y]
        {
            get { return _grid[x, y]; }
            set { _grid[x, y] = value; }
        }



        public CellCollection(int columns, int rows)
        {
            Columns = columns;
            Rows = rows;
            
            _grid = new Cell[Columns,Rows];
            Reset();
        }



        public void Reset()
        {
            _grid.Initialize();
        }

        public void CollectionUpdate(Cell[,] array)
        {
            if (array.GetLength(0) == Columns || array.GetLength(1) == Rows)
            {
                _grid = array;
            }
        }

        public void Resize(int newColumns, int newRows)
        {
            ResizeColumns(newColumns);
            ResizeRows(newRows);
        }


        public void ResizeColumns(int newColumns)
        {
            if (newColumns > Columns)
            {
                var offset = (newColumns - Columns) / 2;
                _grid  = ResizeColumns(newColumns, newOffset: offset);
            }
            if (newColumns < Columns)
            {
                var offset = (Columns - newColumns) / 2;
                _grid = ResizeColumns(newColumns, oldOffset: offset);
            }

            Columns = newColumns;
        }

        private Cell[,] ResizeColumns(int newColumns, int newOffset = 0, int oldOffset = 0)
        {
            var newGrid = new Cell[newColumns, Rows];
            newGrid.Initialize();

            var xIterator = (newColumns < Columns) ? newColumns : Columns;

            for (int x = 0; x < xIterator; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    newGrid[x + newOffset, y] = _grid[x + oldOffset, y];
                }
            }
            return newGrid;
        }


        public void ResizeRows(int newRows)
        {
            if (newRows > Rows)
            {
                var centerOffset = (newRows - Rows) / 2;
                _grid = ResizeRows(newRows, newOffset: centerOffset);
            }
            if (newRows < Rows)
            {
                var centerOffset = (Rows - newRows) / 2;
                _grid = ResizeRows(newRows, oldOffset: centerOffset);
            }

            Rows = newRows;
        }

        private Cell[,] ResizeRows(int newRows, int newOffset = 0, int oldOffset = 0)
        {
            var newGrid = new Cell[Columns, newRows];
            newGrid.Initialize();

            var yIterator = (newRows < Rows) ? newRows : Rows;

            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < yIterator; y++)
                {
                    newGrid[x, y + newOffset] = _grid[x, y + oldOffset];
                }
            }
            return newGrid;
        }

        
        public void OnCollectionUpdated()
        {
            if (CollectionUpdated != null) CollectionUpdated(this, new EventArgs());
        }


        public Cell Current
        {
            get { return (Cell)(this as IEnumerator).Current; }
        }
        object IEnumerator.Current
        {
            get { return _grid.GetEnumerator().Current; }
        }

        public IEnumerator GetEnumerator()
        {
            return _grid.GetEnumerator();
        }

        IEnumerator<Cell> IEnumerable<Cell>.GetEnumerator()
        {
            return (IEnumerator<Cell>)GetEnumerator();
        }

        public bool MoveNext()
        {
            return GetEnumerator().MoveNext();
        }

        public void Dispose() {/*No need to implement*/}
    }
}
