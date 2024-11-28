public class LoginStateService
{
    private bool _isLoggedIn;
    public event Action? OnLoginStateChanged;

    public bool IsLoggedIn
    {
        get => _isLoggedIn;
        set
        {
            if (_isLoggedIn != value)
            {
                _isLoggedIn = value;
                NotifyLoginStateChanged();
            }
        }
    }

    private void NotifyLoginStateChanged()
    {
        OnLoginStateChanged?.Invoke();
    }
}
