using LiteDB;

namespace TodoAppDL
{
    public class LiteDatabaseConnection : IDisposable
    {
        private ILiteDatabase LiteDatabase = new LiteDatabase("todoapp.litedb");

        public ILiteCollection<T> GetCollection<T>()
        {
            return LiteDatabase.GetCollection<T>();
        }

        public void Dispose()
        {
            LiteDatabase?.Dispose();
        }
    }
}