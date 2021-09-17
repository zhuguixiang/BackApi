using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sheng.Web.Infrastructure;
using AstuteTec.Core;
using AstuteTec.Models;
using AstuteTec.Models.Dto;
using Dictionary = AstuteTec.Models.Dictionary;
using AutoMapper;

namespace AstuteTec.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictionaryController : AstuteTecControllerBase
    {
        private readonly DictionaryManager _dictionaryManager;

        public DictionaryController(DictionaryManager dictionaryManager)
        {
            _dictionaryManager = dictionaryManager;
        }

        #region Dictionary

        /// <summary>
        /// 获取字典
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetDictionary")]
        public NormalResult<DictionaryOutDto> GetDictionary(Guid id)
        {
            NormalResult<Dictionary> getResult = _dictionaryManager.GetDictionary(id);
            if (getResult.Successful == false)
            {
                return new NormalResult<DictionaryOutDto>(getResult.Message);
            }
            else
            {
                return new NormalResult<DictionaryOutDto>()
                {
                    Data = Mapper.Map<Dictionary, DictionaryOutDto>(getResult.Data)
                };
            }
        }

        /// <summary>
        /// 获取字典和所属的项目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetDictionaryWithItems")]
        public NormalResult<DictionaryWithItemOutDto> GetDictionaryWithItems(Guid id)
        {
            NormalResult<Dictionary> getOrganizationResult = _dictionaryManager.GetDictionaryWithItems(id);
            if (getOrganizationResult.Successful == false)
            {
                return new NormalResult<DictionaryWithItemOutDto>(getOrganizationResult.Message);
            }
            else
            {
                return new NormalResult<DictionaryWithItemOutDto>()
                {
                    Data = Mapper.Map<Dictionary, DictionaryWithItemOutDto>(getOrganizationResult.Data)
                };
            }
        }

        /// <summary>
        /// 获取字典和所属的项目
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost("GetDictionaryWithItemsByKey")]
        public NormalResult<DictionaryWithItemOutDto> GetDictionaryWithItemsByKey(string key)
        {
            NormalResult<Dictionary> getOrganizationResult = _dictionaryManager.GetDictionaryWithItemsByKey(key);
            if (getOrganizationResult.Successful == false)
            {
                return new NormalResult<DictionaryWithItemOutDto>(getOrganizationResult.Message);
            }
            else
            {
                return new NormalResult<DictionaryWithItemOutDto>()
                {
                    Data = Mapper.Map<Dictionary, DictionaryWithItemOutDto>(getOrganizationResult.Data)
                };
            }
        }

        /// <summary>
        /// 获取字典和所属的项目
        /// </summary>
        /// <param name="keyList"></param>
        /// <returns></returns>
        [HttpPost("GetDictionaryWithItemsBundle")]
        public NormalResult<List<DictionaryWithItemOutDto>> GetDictionaryWithItemsBundle(List<string> keyList)
        {
            NormalResult<List<Dictionary>> getOrganizationResult = _dictionaryManager.GetDictionaryWithItemsBundle(keyList);
            if (getOrganizationResult.Successful == false)
            {
                return new NormalResult<List<DictionaryWithItemOutDto>>(getOrganizationResult.Message);
            }
            else
            {
                return new NormalResult<List<DictionaryWithItemOutDto>>()
                {
                    Data = Mapper.Map<List<Dictionary>, List<DictionaryWithItemOutDto>>(getOrganizationResult.Data)
                };
            }
        }

        /// <summary>
        /// 创建字典
        /// </summary>
        /// <returns></returns>
        [HttpPost("CreateDictionary")]
        public NormalResult CreateDictionary(DictionaryInDto args)
        {
            Dictionary dictionary = Mapper.Map<Dictionary>(args);
            return _dictionaryManager.CreateDictionary(dictionary);
        }

        /// <summary>
        /// 更新字典
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateDictionary")]
        public NormalResult UpdateDictionary(DictionaryInDto args)
        {
            Dictionary dictionary = Mapper.Map<Dictionary>(args);
            return _dictionaryManager.UpdateDictionary(dictionary);
        }

        /// <summary>
        /// 删除字典
        /// </summary>
        /// <returns></returns>
        [HttpPost("RemoveDictionary")]
        public NormalResult RemoveDictionary(Guid id)
        {
            return _dictionaryManager.RemoveDictionary(id);
        }

        /// <summary>
        /// 获取字典列表
        /// </summary>
        /// <param name="args">
        /// 【支持的查询条件】
        /// domainId：必选，字典所在域
        /// keyword：关键词，模糊查询一些字符字段
        /// </param>
        /// <returns></returns>
        [HttpPost("GetDictionaryList")]
        public NormalResult<GetListDataResult<DictionaryOutDto>> GetDictionaryList(GetListDataArgs args)
        {

            NormalResult<GetListDataResult<Dictionary>> dictionaryList = _dictionaryManager.GetDictionaryList(args);

            if (dictionaryList.Successful)
            {
                GetListDataResult<DictionaryOutDto> result = new GetListDataResult<DictionaryOutDto>();
                result.PagingInfo = dictionaryList.Data.PagingInfo;
                result.Data = Mapper.Map<List<Dictionary>, List<DictionaryOutDto>>(dictionaryList.Data.Data);

                return new NormalResult<GetListDataResult<DictionaryOutDto>>()
                {
                    Data = result
                };
            }
            else
            {
                return new NormalResult<GetListDataResult<DictionaryOutDto>>(dictionaryList.Message);
            }
        }

        #endregion

        #region DictionaryItem

        /// <summary>
        /// 获取字典项
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetDictionaryItem")]
        public NormalResult<DictionaryItemOutDto> GetDictionaryItem(Guid id)
        {
            NormalResult<DictionaryItem> getOrganizationResult = _dictionaryManager.GetDictionaryItem(id);
            if (getOrganizationResult.Successful == false)
            {
                return new NormalResult<DictionaryItemOutDto>(getOrganizationResult.Message);
            }
            else
            {
                return new NormalResult<DictionaryItemOutDto>()
                {
                    Data = Mapper.Map<DictionaryItem, DictionaryItemOutDto>(getOrganizationResult.Data)
                };
            }
        }

        /// <summary>
        /// 创建字典项
        /// </summary>
        /// <returns></returns>
        [HttpPost("CreateDictionaryItem")]
        public NormalResult CreateDictionaryItem(DictionaryItemInDto args)
        {
            DictionaryItem dictionaryItem = Mapper.Map<DictionaryItem>(args);
            return _dictionaryManager.CreateDictionaryItem(dictionaryItem);
        }

        /// <summary>
        /// 更新字典项
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateDictionaryItem")]
        public NormalResult UpdateDictionaryItem(DictionaryItemInDto args)
        {
            DictionaryItem dictionaryItem = Mapper.Map<DictionaryItem>(args);
            return _dictionaryManager.UpdateDictionaryItem(dictionaryItem);
        }

        /// <summary>
        /// 删除字典项
        /// </summary>
        /// <returns></returns>
        [HttpPost("RemoveDictionaryItem")]
        public NormalResult RemoveDictionaryItem(Guid id)
        {
            return _dictionaryManager.RemoveDictionaryItem(id);
        }

        /// <summary>
        /// 获取字典项列表
        /// </summary>
        /// <param name="args">
        /// 【支持的查询条件】
        /// dictionaryId：字典Id
        /// keyword：关键词，模糊查询一些字符字段
        /// </param>
        /// <returns></returns>
        [HttpPost("GetDictionaryItemList")]
        public NormalResult<GetListDataResult<DictionaryItemOutDto>> GetDictionaryItemList(GetListDataArgs args)
        {
            NormalResult<GetListDataResult<DictionaryItem>> dictionaryItemList = _dictionaryManager.GetDictionaryItemList(args);

            if (dictionaryItemList.Successful)
            {
                GetListDataResult<DictionaryItemOutDto> result = new GetListDataResult<DictionaryItemOutDto>();
                result.PagingInfo = dictionaryItemList.Data.PagingInfo;
                result.Data = Mapper.Map<List<DictionaryItem>, List<DictionaryItemOutDto>>(dictionaryItemList.Data.Data);

                return new NormalResult<GetListDataResult<DictionaryItemOutDto>>()
                {
                    Data = result
                };
            }
            else
            {
                return new NormalResult<GetListDataResult<DictionaryItemOutDto>>(dictionaryItemList.Message);
            }
        }

        #endregion
    }
}