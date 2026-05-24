using TodoApp.ViewModels;

namespace TodoApp.Pages;

public partial class TaskListPage : ContentPage
{
    public TaskListPage(TaskListViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}