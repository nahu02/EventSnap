namespace EventSnap.Views;

/// <summary>
/// Represents a page that displays a spinning activity indicator and a message (customizable).
/// </summary>
public partial class LoadingPage : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoadingPage"/> class with an optional loading message.
    /// </summary>
    /// <param name="message">The loading message to display. Default value is "Loading".</param>
    public LoadingPage(string message = "Loading")
    {
        InitializeComponent();
        LoadingLabel.Text = message;
    }
}