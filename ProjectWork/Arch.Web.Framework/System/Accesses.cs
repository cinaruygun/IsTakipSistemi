using System.Collections.Generic;
using System.Linq;
using Arch.Service;
using Arch.Service.Interfaces;
using Arch.Service.Services;
using Arch.Core;
using System.Dynamic;
using Newtonsoft.Json;
using Arch.Dto.SingleDto;
using Arch.Utilities.Manager;
using System.Web.Security;
using Arch.Core.Constants;
using Arch.Web.Framework.Enums;

namespace System.Web.Mvc
{
    public static class Accesses
    {
        public static List<string> CookieKeys = new List<string> { "_initials", "_email", "_fullname", "_ui" };
        public static ForbiddenAccessTypes IsLogin()
        {
            try
            {
                var Cookies = HttpContext.Current.Request.Cookies;
                if (CookieKeys.Any(p => Cookies[p] == null || string.IsNullOrEmpty(Cookies[p].Value) || string.IsNullOrEmpty(UtilityManager.Base64Decode(Cookies[p].Value))))
                    return ForbiddenAccessTypes.IsLogout;
                var forbiddenType = ForbiddenAccessTypes.PersonId;
                try
                {
                    var personId = JsonConvert.DeserializeObject<string>(EnDeCode.Decrypt(FormsAuthentication.Decrypt(HttpContext.Current.Request.Cookies[".u"].Value).Name, StaticParams.SifrelemeParametresi));
                    forbiddenType = ForbiddenAccessTypes.UnForbidden;
                    return forbiddenType;
                }
                catch (Exception ex) { return forbiddenType; }
            }
            catch (Exception ex) { return ForbiddenAccessTypes.IsLogout; }
        }
        public static string Initials { get { try { return UtilityManager.Base64Decode(HttpContext.Current.Request.Cookies["_initials"].Value); } catch (Exception) { return null; } } }
        public static string FullName { get { try { return UtilityManager.Base64Decode(HttpContext.Current.Request.Cookies["_fullname"].Value); } catch (Exception) { return null; } } }
        public static string Email { get { try { return UtilityManager.Base64Decode(HttpContext.Current.Request.Cookies["_email"].Value); } catch (Exception) { return null; } } }
        public static int UnitId { get { try { return int.Parse(UtilityManager.Base64Decode(HttpContext.Current.Request.Cookies["_ui"].Value)); } catch (Exception) { return 0; } } }
        public static int PersonId { get { try { return Convert.ToInt32(JsonConvert.DeserializeObject<string>(EnDeCode.Decrypt(FormsAuthentication.Decrypt(HttpContext.Current.Request.Cookies[".u"].Value).Name, StaticParams.SifrelemeParametresi))); } catch (Exception) { return 0; } } }
    }
}