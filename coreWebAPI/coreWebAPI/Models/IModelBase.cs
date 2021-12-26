using System;
using System.Collections.Generic;
using System.Text;

namespace MOM.Core.WebAPI.Models
{
    interface IModelBase<T>
    {
        long Id { get; set; }
        bool IsDeleted { get; }

        T GetByPk(long id);
    }
}
