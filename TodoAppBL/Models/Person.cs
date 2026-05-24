namespace TodoAppBL.Models
{
    public class Person
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; } = DateTime.Today;

        public string ImageUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - BirthDate.Year;

                if (BirthDate.Date > today.AddYears(-age))
                    age--;

                return age;
            }
        }
    }
}