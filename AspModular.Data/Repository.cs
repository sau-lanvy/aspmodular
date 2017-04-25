using AspModular.Data.Abstractions;
using AspModular.Data.Models.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspModular.Data
{
    public class Repository<T> : RepositoryWithTypedId<T, long>, IRepository<T>
       where T : class, IEntityWithTypedId<long>
    {
        public Repository(StorageContext context) : base(context)
        {
        }
    }
}
