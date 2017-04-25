using AspModular.Data.Models.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspModular.Data.Abstractions
{
    public interface IRepository<T> : IRepositoryWithTypedId<T, long>
        where T : class, IEntityWithTypedId<long>
    {
    }
}
