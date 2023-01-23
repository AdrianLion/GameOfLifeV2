using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameOfLifeV2;

public enum Status
{
    Dead, // Brushes.aqua
    Alive // Brushes.coral
}


public class Cell
{
    public double Width { get; set; }
    public double Height { get; set; }
    
    public double PosX { get; set; }
    public double PosY { get; set; }

    private Rectangle _shape;
    private TextBlock _text;

    private Status _status;
    
    public Status Status
    {
        get => _status;
        set
        {
            if (_status != value)
            {
                _status = value;
                SetBrush(_status);
            }
        }
    }

    private int _neighbours;
    
    public int Neighbours
    {
        get => _neighbours;
        set
        {
            _neighbours = value;
            SetNeighbours();
        }
    }
    public Cell(double posX, double posY, Status status, Canvas cv)
    {
        this.Width = cv.ActualWidth / Constants.FieldsColumns - 2.0;
        this.Height = cv.ActualHeight / Constants.FieldsRows - 2.0;

        this.PosX = posX;
        this.PosY = posY;
        this.Status = status;

        // Rectangle
        _shape = new Rectangle();
        SetBrush(status);
        _shape.Width = Width;
        _shape.Height = Height;
        cv.Children.Add(_shape);
        Canvas.SetTop(_shape, PosY);
        Canvas.SetLeft(_shape, PosX);

        // Text in Rectangle
        _text = new TextBlock();
        _text.FontSize = 10;
        SetNeighbours();
        
        cv.Children.Add(_text);
        Canvas.SetTop(_text, PosY);
        Canvas.SetLeft(_text, PosX);
        
        _shape.MouseDown += ShapeOnMouseDown;
    }

    private void ShapeOnMouseDown(object sender, MouseButtonEventArgs e)
    {
        Status = (Status == Status.Alive) ? Status.Dead : Status.Alive;
        SetNeighbours();
    }

    private void SetBrush(Status status)
    {
        if (_shape != null)
        {
            _shape.Fill = (status == Status.Alive) ? Brushes.Coral : Brushes.Aqua;
        }
    }

    private void SetNeighbours()
    {
        _text.Text = _neighbours.ToString();
    }
}