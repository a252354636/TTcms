
using System.Web.Http;
using System.Linq;
using TTcms.Infrastructure.Core.Authorize;
using TTcms.Application.System;
using TTcms.Infrastructure.TypeUtilities.TypeAdapter;
using TTcms.DTO.System;
using static TTcms.SSO.Server.Setting;

namespace TTcms.SSO.Server.Controllers
{
    [TTcmsApiAuthorize(Roles = RoleConfig.InAppUserBaseRole + "," + RoleConfig.AppRole)]
    [RoutePrefix("TicketUser")]
    public class TicketUserController : ApiController
    {
        private UserService _userService;
        private AppService _appService;
        private ITypeAdapter _typeAdapter;

        public TicketUserController(UserService userService, AppService appService, ITypeAdapter typeAdapter)
        {
            _userService = userService;
            _typeAdapter = typeAdapter;
            _appService = appService;
        }

        [Route("GetTicketMessage")]
        [HttpGet]
        public IHttpActionResult GetTicketMessage()
        {
            if (User.IsInRole(RoleConfig.AppRole))
            {
                var app = _appService.Query(new AppDTO { Name = User.Identity.Name }, null).FirstOrDefault();
                return Json(
                    new
                    {
                        AppID = app.ID,
                        AppName = app.Name,
                        RedirectUri = app.ReturnUrl
                    });
            }
            var userData = _userService.GetUserByName(User.Identity.Name);
            var result = _typeAdapter.Adapt<UserDTO>(userData);
            return Json(new
            {
                UserID = result.UserID,
                UserName = result.Name,
                Mobile = result.Mobile,
                Email = result.Email
            });
        }

    }
}
