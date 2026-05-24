namespace TodoApp
{
    public class NavigationService
    {
        public async Task GoToAsync(string route, ShellNavigationQueryParameters? parameters = null)
        {
            if (parameters == null)
                await Shell.Current.GoToAsync(route);
            else
                await Shell.Current.GoToAsync(route, parameters);
        }

        public async Task ShowAlertAsync(string title, string message)
        {
            await Shell.Current.DisplayAlert(title, message, "OK");
        }
    }
}