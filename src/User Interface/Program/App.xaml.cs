namespace BibTexManager;

public partial class App : Application
{
	public static Window? Window { get; set; }

	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		Window = new Window(MainPage!);   
		return Window;
	}
}