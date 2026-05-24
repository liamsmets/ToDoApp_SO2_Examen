namespace TodoAppBL.Messages
{
    public class PersonDeletedMessage
    {
        public string Id { get; }

        public PersonDeletedMessage(string id)
        {
            Id = id;
        }
    }
}