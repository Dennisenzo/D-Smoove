using LiteDB;

namespace Denga.Dsmoove.Engine.Data.Entities
{
    public class BaseEntity
    {
        [BsonId]
        public int Id { get; set; }
    }
}