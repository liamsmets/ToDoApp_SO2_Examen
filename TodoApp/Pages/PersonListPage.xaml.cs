using TodoApp.ViewModels;

namespace TodoApp.Pages;

public partial class PersonListPage : ContentPage
{
    public PersonListPage(PersonListViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}