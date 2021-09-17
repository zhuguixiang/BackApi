using AstuteTec.Infrastructure;
using AstuteTec.Models;
using AstuteTec.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sheng.Kernal;
using Sheng.Kernal.Core;
using Sheng.Web.Infrastructure;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Transactions;

namespace AstuteTec.Core
{
    public class UserManager
    {
        private CachingService _cachingService;

        private AppSettings _appSettings;

        public UserManager(IOptions<AppSettings> settings, CachingService cachingService)
        {
            _appSettings = settings.Value;
            _cachingService = cachingService;
        }

        public Task<NormalResult<User>> GetUserAsync(Guid id)
        {
            return System.Threading.Tasks.Task.Run(() => { return GetUser(id); });
        }

        public NormalResult<User> GetUser(Guid id)
        {
            using (Entities db = Entities.CreateContext())
            {
                User user = db.User
                    //.Include(c => c.Organization)
                    //.Include(c => c.RoleUser).ThenInclude(c => c.Role).ThenInclude(c => c.RoleAuthorization)
                    .AsNoTracking()
                    .FirstOrDefault(e => e.Id == id && e.Removed == false);
                if (user == null)
                {
                    return new NormalResult<User>("未找到用户信息");
                }
                return new NormalResult<User>()
                {
                    Data = user
                };
            }
        }

        public NormalResult CreateUser(User user)
        {
            if (String.IsNullOrEmpty(user.Account))
            {
                return new NormalResult("需指定账号。");
            }
            if (String.IsNullOrEmpty(user.Password))
            {
                return new NormalResult("需指定密码。");
            }

            using (Entities db = Entities.CreateContext())
            {
                if (db.User.Any(s => s.Account == user.Account && s.Removed == false))
                {
                    return new NormalResult("指定的登录账户已被占用。");
                }

                user.Id = Guid.NewGuid();
                user.Password = user.Password.ToUpper();
                user.CreateTime = DateTime.Now;
                db.User.Add(user);
                db.SaveChanges();
            }

            return new NormalResult();
        }

        public NormalResult UpdateUser(User user)
        {
            if (user.Id == Guid.Empty)
            {
                return new NormalResult("用户Id不能为空。");
            }
            if (String.IsNullOrEmpty(user.Account))
            {
                return new NormalResult("需指定账号。");
            }

            using (Entities db = Entities.CreateContext())
            {
                //执行事务
                using (TransactionScope scope = new TransactionScope())
                {
                    User dbUser = db.User.FirstOrDefault(e => e.Id == user.Id && e.Removed == false);
                    if (dbUser == null)
                        return new NormalResult("指定的数据不存在。");

                    if (db.User.Any(s => s.Account == user.Account && s.Id != user.Id && s.Removed == false))
                    {
                        return new NormalResult("指定的登录账户已被占用。");
                    }

                    //不更新密码
                    if (String.IsNullOrEmpty(user.Password))
                    {
                        ShengMapper.SetValuesWithoutProperties(user, dbUser, new string[] { "Password", "CreateUserId", "CreateTime", "Removed" }, true);
                    }
                    else
                    {
                        user.Password = user.Password.ToUpper();
                        ShengMapper.SetValuesWithoutProperties(user, dbUser, new string[] { "CreateTime", "CreateUserId", "Removed" }, true);
                    }
                    db.SaveChanges();
                    scope.Complete();
                }
            }
            return new NormalResult();
        }

        public NormalResult RemoveUser(Guid id)
        {
            using (Entities db = Entities.CreateContext())
            {
                User user = db.User.FirstOrDefault(e => e.Id == id && e.Removed == false);
                if (user != null)
                {
                    user.Removed = true;
                    db.SaveChanges();
                }
            }

            return new NormalResult();
        }

        public NormalResult<GetListDataResult<User>> GetUserList(GetListDataArgs args, UserOutDto userOutDto)
        {
            GetListDataResult<User> result = new GetListDataResult<User>();
            using (Entities db = Entities.CreateContext())
            {
                IQueryable<User> queryable = db.User
                    .Where(c => c.Removed == false)
                    .AsNoTracking();

                #region 查询条件
                if (args.Parameters.IsNullOrEmpty("Keyword") == false)
                {
                    string keyword = args.Parameters.GetValue<string>("Keyword");
                    queryable = queryable.Where(c => c.Account.Contains(keyword) || c.Name.Contains(keyword)
                    || c.Email.Contains(keyword) || c.Cellphone.Contains(keyword));
                }
                #endregion
                
                result.PagingInfo = new ResultPagingInfo(args.PagingInfo);
                int totalCount = queryable.Count();
                result.PagingInfo.UpdateTotalCount(totalCount);

                if (String.IsNullOrEmpty(args.OrderBy) == false)
                {
                    queryable = queryable.OrderBy(args.OrderBy);
                }

                result.Data = queryable
                    .Skip((result.PagingInfo.CurrentPage - 1) * result.PagingInfo.PageSize)
                    .Take(result.PagingInfo.PageSize).ToList();
            }

            return new NormalResult<GetListDataResult<User>>()
            {
                Data = result
            };
        }

        public NormalResult UpdatePassword(UpdatePasswordArgs args)
        {
            if (args.UserId == Guid.Empty)
            {
                return new NormalResult("用户不能为空。");
            }
            if (String.IsNullOrEmpty(args.OldPassword))
            {
                return new NormalResult("需指定旧密码。");
            }
            if (String.IsNullOrEmpty(args.NewPassword))
            {
                return new NormalResult("需指定新密码。");
            }

            using (Entities db = Entities.CreateContext())
            {
                args.OldPassword = args.OldPassword.ToUpper();

                User dbUser = db.User
                    .FirstOrDefault(e => e.Id == args.UserId
                        && e.Password == args.OldPassword
                        && e.Removed == false);
                if (dbUser == null)
                    return new NormalResult("旧密码不正确。");

                dbUser.Password = args.NewPassword.ToUpper();
                db.SaveChanges();
            }

            return new NormalResult();
        }
    }
}
