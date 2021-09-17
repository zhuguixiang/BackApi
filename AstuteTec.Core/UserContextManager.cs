using Microsoft.Extensions.Options;
using Sheng.Kernal;
using Sheng.Web.Infrastructure;
using AstuteTec.Infrastructure;
using AstuteTec.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Sheng.Kernal.Core;

namespace AstuteTec.Core
{
    public class UserContextManager
    {
        private CachingService _cachingService;

        private AppSettings _appSettings;

        public UserContextManager(IOptions<AppSettings> settings, CachingService cachingService)
        {
            _appSettings = settings.Value;
            _cachingService = cachingService;
        }

        public NormalResult<UserContext> Login(UserLoginArgs args)
        {
            if (String.IsNullOrEmpty(args.Account) || String.IsNullOrEmpty(args.Password))
            {
                return new NormalResult<UserContext>("用户名或密码不能为空。");
            }

            using (Entities db = Entities.CreateContext())
            {
                args.Password = args.Password.ToUpper();

                User user = db.User
                    .FirstOrDefault(e => e.Account == args.Account
                        && e.Password == args.Password
                        && e.Removed == false);
                if (user == null)
                {
                    return new NormalResult<UserContext>("用户名或密码无效。");
                }

                UserContext userContext = new UserContext()
                {
                    LoginTime = DateTime.Now,
                    Token = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                };
                NormalResult<UserContext> result = new NormalResult<UserContext>
                {
                    Data = userContext
                };

                _cachingService.Set<UserContext>(userContext.Token, userContext);
                return result;
            }
        }

        public void Logout(string token)
        {
            if (String.IsNullOrEmpty(token))
                return;

            _cachingService.Remove(token);
        }

        public UserContext GetUserContext(string token)
        {
            UserContext userContext = _cachingService.Get<UserContext>(token);
            return userContext;
        }
    }
}
