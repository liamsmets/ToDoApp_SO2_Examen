using TodoApp.ViewModels;

namespace TodoApp.Pages;

public partial class TaskDetailPage : ContentPage
{
    public TaskDetailPage(TaskDetailViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}