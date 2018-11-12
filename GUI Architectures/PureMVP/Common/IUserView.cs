using System;

namespace Library
{

    public interface IUserView
    {
        event EventHandler DataChanged;
        event EventHandler Save;
        string UserName { get; set; }
        string Email { get; set; }
        void Show();
    }
}
