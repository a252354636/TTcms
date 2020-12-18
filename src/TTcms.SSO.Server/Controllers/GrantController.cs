using TTcms.Infrastructure.Core.Authorize;
using TTcms.Infrastructure.Exceptions;
using TTcms.Infrastructure.Http;
using TTcms.Infrastructure.Mvc;
using TTcms.Infrastructure.OAuth;
using TTcms.Infrastructure.TypeUtilities.TypeAdapter;
using Microsoft.AspNet.Identity;
using TTcms.Application.System;
using TTcms.DTO.System;
using System.Linq;
using System.Web.Mvc;
using static TTcms.SSO.Server.Setting;

namespace TTcms.SSO.Server.Controllers
{
    [TTcmsMvcAuthorize(Roles = RoleConfig.InUserAdminRole)]
    public class GrantController : BaseMvcController
    {
        private AppService _appService;
        private ITypeAdapter _typeAdapter;
        private ITicketStore _ticketStore;

        public GrantController(AppService appService, ITypeAdapter typeAdapter, ITicketStore ticketStore)
        {
            _appService = appService;
            _typeAdapter = typeAdapter;
            _ticketStore = ticketStore;
        }

        // GET: OAuth/Grant
        public ActionResult Index()
        {
            FormStringControl queryControl = new FormStringControl(Request.Url.Query);
            ViewBag.Scope = queryControl.GetParamValue("Scope"); ;
            var clientIDStr = queryControl.GetParamValue("client_id");
            int clientID;
            if (string.IsNullOrEmpty(clientIDStr) || !int.TryParse(clientIDStr, out clientID))
            {
                throw new BusinessException("client_id不存在.");
            }
            var app = _appService.Query(new AppDTO
            {
                ClientID = clientID
            }, null).ToList().FirstOrDefault();
            if (app == null)
            {
                throw new BusinessException("该client_id不存在应用.");
            }
            ViewBag.AccessUrl = Url.Content("~/") + EndPointConfig.AuthorizeGrantAccess + Request.Url.Query;
            return View(_typeAdapter.Adapt<AppDTO>(app));
        }

        /// <summary>
        /// 授权
        /// </summary>
        /// <returns></returns>
        public ActionResult Access()
        {
            FormStringControl queryControl = new FormStringControl(Request.Url.Query);
            var clientID = queryControl.GetParamValue("client_id");
            var userID = User.Identity.GetUserId();
            _ticketStore.Set("TemporaryAuthorization" + clientID + "$" + userID, "", 2);
            return Redirect(Url.Content("~/") + EndPointConfig.AuthorizeEndpointPath.TrimStart('/') + Request.Url.Query);
        }
    }
}