using TodoApp.ViewModels;

namespace TodoApp.Pages;

public partial class PersonDetailPage : ContentPage
{
    public PersonDetailPage(PersonDetailViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}