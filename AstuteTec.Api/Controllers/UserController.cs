using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sheng.Web.Infrastructure;
using AstuteTec.Core;
using AstuteTec.Infrastructure;
using AstuteTec.Models;
using AstuteTec.Models.Dto;

namespace AstuteTec.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : AstuteTecControllerBase
    {
        private readonly UserManager _userManager;

        public UserController(UserManager userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetUser")]
        public NormalResult<UserOutDto> GetUser(Guid id)
        {
            NormalResult<User> getOrganizationResult = _userManager.GetUser(id);
            if (getOrganizationResult.Successful == false)
            {
                return new NormalResult<UserOutDto>(getOrganizationResult.Message);
            }

            UserOutDto userOutDto = Mapper.Map<User, UserOutDto>(getOrganizationResult.Data);
            return new NormalResult<UserOutDto>()
            {
                Data = userOutDto
            };
        }

        /// <summary>
        /// 创建用户
        /// 密码需MD5加密
        /// </summary>
        /// <returns></returns>
        [HttpPost("CreateUser")]
        public NormalResult CreateUser(UserInDto args)
        {
            if (args == null)
            {
                return new NormalResult("参数不能为空。");
            }

            User user = Mapper.Map<User>(args);
            user.CreateUserId = this.UserContext.UserId;
            return _userManager.CreateUser(user);
        }

        /// <summary>
        /// 更新用户
        /// 可以更新密码
        /// 也可以 Password 留空表示不更新密码
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateUser")]
        public NormalResult UpdateUser(UserInDto args)
        {
            if (args == null)
            {
                return new NormalResult("参数不能为空。");
            }

            User user = Mapper.Map<User>(args);
            return _userManager.UpdateUser(user);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        [HttpPost("RemoveUser")]
        public NormalResult RemoveUser(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new NormalResult("用户Id不能为空。");
            }

            return _userManager.RemoveUser(id);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="args">
        /// 【支持的查询条件】
        /// Keyword：关键词，模糊查询一些字符字段
        /// </param>
        /// <returns></returns>
        [HttpPost("GetUserList")]
        public NormalResult<GetListDataResult<UserOutDto>> GetUserList(GetListDataArgs args)
        {
            NormalResult<GetListDataResult<User>> userList = _userManager.GetUserList(args, this.User);

            if (userList.Successful)
            {
                GetListDataResult<UserOutDto> result = new GetListDataResult<UserOutDto>();
                result.PagingInfo = userList.Data.PagingInfo;
                result.Data = Mapper.Map<List<User>, List<UserOutDto>>(userList.Data.Data);

                return new NormalResult<GetListDataResult<UserOutDto>>()
                {
                    Data = result
                };
            }
            else
            {
                return new NormalResult<GetListDataResult<UserOutDto>>(userList.Message);
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost("UpdatePassword")]
        public NormalResult UpdatePassword(UpdatePasswordArgs args)
        {
            if (args == null)
            {
                return new NormalResult("参数不能为空。");
            }

            return _userManager.UpdatePassword(args);
        }
    }
}