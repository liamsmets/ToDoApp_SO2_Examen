using TodoAppBL.Models;

namespace TodoAppBL.Messages
{
    public class PersonSavedMessage
    {
        public Person Person { get; }

        public PersonSavedMessage(Person person)
        {
            Person = person;
        }
    }
}