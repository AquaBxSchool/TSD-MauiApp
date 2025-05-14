using Task2_3.Views;

namespace Task2_3;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(NotePage), typeof(NotePage));
    }
}