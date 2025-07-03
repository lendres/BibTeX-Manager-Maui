namespace BibTeXManager;

public partial class App : Application
{
	public static Window? Window { get; set; }

	public App()
	{
		InitializeComponent();
	}

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}