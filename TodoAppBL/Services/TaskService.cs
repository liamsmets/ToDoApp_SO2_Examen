using CommunityToolkit.Mvvm.Messaging;
using TodoAppBL.Interfaces;
using TodoAppBL.Messages;
using TodoAppBL.Models;
using TodoAppBL.Strategies;
using Task = TodoAppBL.Models.Task;

namespace TodoAppBL.Services
{
    public class TaskService
    {
        private ITaskRepo TaskRepo { get; }
        private IPersonRepo PersonRepo { get; }

        public TaskService(ITaskRepo taskRepo, IPersonRepo personRepo)
        {
            TaskRepo = taskRepo;
            PersonRepo = personRepo;
        }

        public List<Task> GetAll()
        {
            return TaskRepo.GetAll();
        }

        public Task? GetById(string id)
        {
            return TaskRepo.GetById(id);
        }

        public void AddTask(Task task)
        {
            ArgumentNullException.ThrowIfNull(task);

            Validate(task);

            var now = DateTime.Now;

            task.CreatedAt = now;
            task.UpdatedAt = now;

            TaskRepo.Add(task);

            WeakReferenceMessenger.Default.Send(new TaskSavedMessage(task));
        }

        public void UpdateTask(Task task)
        {
            ArgumentNullException.ThrowIfNull(task);

            Validate(task);

            task.UpdatedAt = DateTime.Now;

            TaskRepo.Update(task);

            WeakReferenceMessenger.Default.Send(new TaskSavedMessage(task));
        }

        public void ToggleCompleted(string id, bool isCompleted)
        {
            var task = TaskRepo.GetById(id);

            if (task == null)
                return;

            task.IsCompleted = isCompleted;
            task.UpdatedAt = DateTime.Now;

            TaskRepo.Update(task);

            WeakReferenceMessenger.Default.Send(new TaskSavedMessage(task));
        }

        private void Validate(Task task)
        {
            if (string.IsNullOrWhiteSpace(task.Title))
                throw new InvalidDataException("Titel is verplicht.");

            if (string.IsNullOrWhiteSpace(task.PersonId))
                throw new InvalidDataException("Er moet een persoon toegewezen zijn.");

            if (!PersonRepo.Exists(task.PersonId))
                throw new InvalidDataException("De toegewezen persoon bestaat niet.");
        }
    }
}