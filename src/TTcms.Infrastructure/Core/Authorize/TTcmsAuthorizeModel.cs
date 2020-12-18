using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTcms.Infrastructure.Core.Authorize
{
    public class TTcmsAuthorizeModel
    {
        public int UserID
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string PrincipalRole
        {
            get;
            set;
        }

        public string NodeRole
        {
            get;
            set;
        }

        public string NodeName
        {
            get;
            set;
        }

        public string ActionName
        {
            get;
            set;
        }

    }
}
