using System;
using TTcms.Domain.System;
using System.Web;

namespace TTcms.Application.Core
{
    public class CurrentContext : ICurrentContext
    {
        private HttpContextBase _httpContext;

        public CurrentContext(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        public User User
        {
            get
            {
                if (_httpContext != null)
                {
                    return new User {
                        Name = _httpContext.User.Identity.Name
                    };
                }
                return null;
            }
        }

    }
}