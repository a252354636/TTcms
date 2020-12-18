using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTcms.Infrastructure.Core.Authorize;
using static TTcms.Web.Setting;

namespace TTcms.Web.Controllers
{
    public class TokenClientController : Controller
    {
        private const string _serverUrl = "http://localhost:8550";

        private const string _serverTicketMessageUrl = _serverUrl + "/TicketUser/GetTicketMessage";
        // GET: TokenClient/ShowUser
        [ValidateInput(false)]
        public ActionResult ShowUser()
        {
            HttpCookie login = Request.Cookies["TTcmsSSOLoginData"]; //获取客户端返回的Cookies中名称为Login的Cookie对象
            if (login != null)
            {

            }
            else
            {
  
            }
            ViewBag.ServerTicketMessageUrl = _serverTicketMessageUrl;
            return View();
        }
    }
}