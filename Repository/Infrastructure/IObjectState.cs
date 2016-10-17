using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Infrastructure
{
    public interface IObjectState
    {
        [NotMapped]
        ObjectState ObjectState { get; set; }
    }
}