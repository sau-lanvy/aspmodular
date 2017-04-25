using System;
using System.Collections.Generic;
using System.Text;

namespace AspModular.Data.Models.Abstractions
{
    /// <summary>
    ///     Gets the ID which uniquely identifies the entity instance within its type's bounds.
    /// </summary>
    public interface IEntityWithTypedId<TId>
    {
        TId Id { get; }
    }
}
