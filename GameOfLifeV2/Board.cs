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

    // Zählt die benachbarten lebenden Zellen von jeder Zelle.
    public void CountNeighbours()
    {
        for (int x = 0; x < Constants.FieldsRows; x++)
        {
            for (int y = 0; y < Constants.FieldsColumns; y++)
            {
                int count = 0;
                for (int offsetX = -1; offsetX <= 1; offsetX++)
                {
                    for (int offsetY = -1; offsetY <= 1; offsetY++)
                    {
                        int XtoCheck = x + offsetX;
                        int yToCheck = y + offsetY;
                        // Wenn Koordinate ausserhalb vom Wertebereich ist überspringe den aktuellen Schleifen-Schritt
                        if (XtoCheck < 0 || XtoCheck > Constants.FieldsRows - 1)
                            continue;
                        // Wenn Koordinate ausserhalb vom Wertebereich ist überspringe den aktuellen Schleifen-Schritt
                        if (yToCheck < 0 || yToCheck > Constants.FieldsColumns - 1)
                            continue;
                        if (_cells[XtoCheck, yToCheck].Status == Status.Alive)
                            count++;
                    }
                }
                // Zählt sich selbst nicht mit
                if (_cells[x, y].Status == Status.Alive)
                    _cells[x, y].Neighbours = count - 1;
                else
                    _cells[x, y].Neighbours = count;
            }
        }
    }
    // Zählt die benachbarten lebenden Zellen von jeder Zelle. (über den rand hinweg)
    public void CountNeighboursNoBorders()
    {
        for (int x = 0; x < Constants.FieldsRows; x++)
        {
            for (int y = 0; y < Constants.FieldsColumns; y++)
            {
                int count = 0;
                for (int offsetX = -1; offsetX <= 1; offsetX++)
                {
                    for (int offsetY = -1; offsetY <= 1; offsetY++)
                    {
                        int XtoCheck = x + offsetX;
                        int yToCheck = y + offsetY;
                        // wenn x = -1 ist setze x = unteren Rand
                        if (XtoCheck < 0)
                            XtoCheck = Constants.FieldsRows - 1;
                        // wenn x = unterer Rand ist setze x = 0
                        if (XtoCheck > Constants.FieldsRows - 1)
                            XtoCheck = 0;
                        // wenn y = -1 ist setze y = rechten Rand
                        if (yToCheck < 0)
                            yToCheck = Constants.FieldsColumns - 1;
                        // wenn y = rechten Rand ist setze y = 0
                        if (yToCheck > Constants.FieldsColumns - 1)
                            yToCheck = 0;
                        if (_cells[XtoCheck, yToCheck].Status == Status.Alive)
                            count++;
                    }
                }
                // Zählt sich selbst nicht mit
                if (_cells[x, y].Status == Status.Alive)
                    _cells[x, y].Neighbours = count - 1;
                else
                    _cells[x, y].Neighbours = count;
            }
        }
    }
    public void SetNewStatus()
    {
        foreach (var cell in _cells)
        {
            cell.Status = ToNewStatus(cell.Neighbours, cell.Status);
        }
    }
    /// <summary>
    /// Wenn eine Lebende Zelle 2 oder 3 lebende Nachbarn hat bleibt sie am leben
    /// Wenn eine Tote Zelle genau 3 lebende nachbarn hat wird sie wiederbelebt
    /// _ bedeutet (default), also in allen anderen Fällen -> Status.Dead
    /// *** das ganze nennt sich switch expression. normale switch case kann nur auf einen genauen wert prüfen,
    /// *** eine switch expression kann Wertebereiche abdecken.
    /// </summary>
    /// <param name="neighbours"> Anzahl der Nachbarn einer Zelle</param>
    /// <param name="status">Status einer Zelle</param>
    /// <returns></returns>
    private Status ToNewStatus(int neighbours, Status status) => (neighbours, status) switch
    {
        ( >= 2 and <= 3, Status.Alive) => Status.Alive,
        (3, Status.Dead) => Status.Alive,
        _ => Status.Dead
    };
}