using TodoApp.ViewModels.Base;
using TodoAppBL.Models;

namespace TodoApp.ViewModels.TaskDetail
{
    public class PersonPickerViewModel : ViewModel
    {
        public string Id { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public PersonPickerViewModel(Person person)
        {
            Id = person.Id;
            FullName = $"{person.FirstName} {person.LastName}".Trim();
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}