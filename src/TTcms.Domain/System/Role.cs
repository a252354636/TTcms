﻿using TTcms.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTcms.Domain.System
{
    public class Role : EntityBase
    {
        public string Name
        {
            get;
            set;
        }

        public virtual ICollection<User> Users
        {
            get;
            set;
        }

    }
}
