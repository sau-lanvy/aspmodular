using System.ComponentModel.DataAnnotations;

namespace AspModular.Data.Models.Abstractions
{
    public abstract class EntityWithTypedId<TId> : ValidatableObject, IEntityWithTypedId<TId>
    {
        [Key]
        public TId Id { get; protected set; }
    }
}
