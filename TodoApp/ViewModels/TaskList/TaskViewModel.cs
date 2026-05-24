using TodoApp.ViewModels.Base;
using TaskModel = TodoAppBL.Models.Task;

namespace TodoApp.ViewModels.TaskList
{
    public class TaskViewModel : ViewModel
    {
        private readonly Action<TaskViewModel, bool> _completedChanged;
        private bool _isUpdatingFromModel;

        public string Id { get; set; } = string.Empty;

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                NotifyPropertyChanged();
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                NotifyPropertyChanged();
            }
        }

        private bool _isCompleted;
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted == value)
                    return;

                _isCompleted = value;
                NotifyPropertyChanged();

                if (!_isUpdatingFromModel)
                    _completedChanged(this, value);
            }
        }

        public TaskViewModel(TaskModel task, Action<TaskViewModel, bool> completedChanged)
        {
            _completedChanged = completedChanged;
            UpdateFrom(task);
        }

        public void UpdateFrom(TaskModel task)
        {
            _isUpdatingFromModel = true;

            Id = task.Id;
            Title = task.Title;
            Description = task.Description;
            IsCompleted = task.IsCompleted;

            _isUpdatingFromModel = false;
        }
    }
}