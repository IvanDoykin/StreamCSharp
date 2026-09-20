using Spectre.Console;

public class Button
{
    private bool _isSelected;
    public bool IsSelected => _isSelected;

    private Action _onClick;

    public Button(Action onClick)
    {
        _onClick = onClick;
    }

    public void Select()
    {
        _isSelected = true;
    }

    public void Deselect()
    {
        _isSelected = false;
    }

    public void Click()
    {
        if (_isSelected)
        {
            _onClick?.Invoke();
        }

        else
        {
            throw new Exception("Button tried be clicked without select.");
        }
    }
}
