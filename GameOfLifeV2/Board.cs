using System.Windows.Controls;

namespace GameOfLifeV2;

public class Board
{
    private Cell[,] _cells;

    private CellCoordinates[,] _cellCoordinates;

    private double _distanceX;
    private double _distanceY;

    public Board(Canvas plotArea)
    {
        _cells = new Cell[Constants.FieldsRows, Constants.FieldsColumns];
        _cellCoordinates = new CellCoordinates[Constants.FieldsRows, Constants.FieldsColumns];
        
        _distanceX = plotArea.ActualWidth / Constants.FieldsColumns;
        _distanceY = plotArea.ActualHeight / Constants.FieldsRows;
    }

    public void InitBoard(Canvas plotArea)
    {
        for (int iRows = 0; iRows < Constants.FieldsRows; iRows++)
        {
            for (int iColumns = 0; iColumns < Constants.FieldsColumns; iColumns++)
            {
                _cellCoordinates[iRows, iColumns].PosX = _distanceX * iColumns;
                _cellCoordinates[iRows, iColumns].Posy = _distanceY * iRows;

                _cells[iRows, iColumns] = new Cell(_cellCoordinates[iRows, iColumns].PosX,
                    _cellCoordinates[iRows, iColumns].Posy, Status.Dead, plotArea);
            }
        }
    }

    public void SearchNeighbours()
    {
        // First lines neighbour are also the last line (periodic continuation)
        for (int iRows = 0; iRows < Constants.FieldsRows; iRows++)
        {
            for (int iColumns = 0; iColumns < Constants.FieldsColumns; iColumns++)
            {
                // Check if Cell is in first or last Row. 
                int iTop = iRows - 1;
                if (iTop < 0) iTop = Constants.FieldsRows - 1;
                int iBottom = iRows + 1;
                if (iBottom >= Constants.FieldsRows) iBottom = 0;
                
                // Check if Cell in first or last Column
                int iLeft = iColumns - 1;
                if (iLeft < 0) iLeft = Constants.FieldsColumns - 1;
                int iRight = iColumns + 1;
                if (iRight >= Constants.FieldsColumns) iRight = 0;
                    
                // TOP LEFT
                if (_cells[iLeft, iTop].Status == Status.Alive) { _cells[iRows, iColumns].Neighbours++; }

                // TOP CENTER
                if (_cells[iColumns, iTop].Status == Status.Alive) { _cells[iRows, iColumns].Neighbours++; }
                
                // TOP RIGHT
                if (_cells[iRight, iTop].Status == Status.Alive) { _cells[iRows, iColumns].Neighbours++; }
                
                // CENTER LEFT
                if (_cells[iLeft, iRows].Status == Status.Alive) { _cells[iRows, iColumns].Neighbours++; }

                // CENTER RIGHT
                if (_cells[iRight, iRows].Status == Status.Alive) { _cells[iRows, iColumns].Neighbours++; }

                // BOTTOM LEFT
                if (_cells[iLeft, iBottom].Status == Status.Alive) { _cells[iRows, iColumns].Neighbours++; }

                // BOTTOM CENTER
                if (_cells[iColumns, iBottom].Status == Status.Alive) { _cells[iRows, iColumns].Neighbours++; }

                // BOTTOM RIGHT
                if (_cells[iRight, iBottom].Status == Status.Alive) { _cells[iRows, iColumns].Neighbours++; }
            }
        }
    }
}