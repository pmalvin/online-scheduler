using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineScheduler.Pages
{
    public class RegisterResultModel : PageModel
    {
        public void OnGet(string username, RegisterResult result)
        {
            if (result == RegisterResult.Succeed)
            {
                ViewData["Message"] = "恭喜您，" + username + "，注册成功！";
            }
            else
            {
                ViewData["Message"] = "很抱歉，注册失败！";
            }
        }
    }
    public enum RegisterResult { Succeed, Fail }
}