using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Repository.Infrastructure
{
    public abstract class EntityBaseEF<TKey> : IEntity<TKey>, IObjectState
    {
        [BsonIgnoreIfDefault]
        public virtual TKey Id { get; set; }

        [NotMapped]
        public virtual int TotalCount { get; set; }

        [NotMapped]
        public virtual ObjectState ObjectState { get; set; }
    }

    public abstract class EntityBase<Tkey> : IEntity<string>, IObjectState
    {
        private string internalID { get; set; }

        [BsonId]
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id
        {
            get { return internalID; }
            set { internalID = value; }
        }

        [BsonIgnore]
        [NotMapped]
        public virtual int TotalCount { get; set; }

        [BsonIgnore]
        [NotMapped]
        public virtual ObjectState ObjectState { get; set; }
    }
}