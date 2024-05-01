namespace EventSnap.Views;

public partial class LoadingPage : ContentPage
{
    public LoadingPage(string message = "Loading")
    {
        InitializeComponent();
        LoadingLabel.Text = message;
    }
}