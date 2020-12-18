﻿using TTcms.Infrastructure.Core.Authorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTcms.Web.Code.Authorize
{
    public class SSOAuthorize : TTcmsAuthorize
    {
        protected override bool IsAuthorized(TTcmsAuthorizeModel model)
        {
            return true;
        }
    }
}