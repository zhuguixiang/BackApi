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
    public class DictionaryManager
    {
        private CachingService _cachingService;

        private AppSettings _appSettings;

        public DictionaryManager(IOptions<AppSettings> settings, CachingService cachingService)
        {
            _appSettings = settings.Value;
            _cachingService = cachingService;
        }

        #region Dictionary

        public NormalResult<Dictionary> GetDictionary(Guid id)
        {
            using (Entities db = Entities.CreateContext())
            {
                Dictionary dictionary = db.Dictionary
                    .AsNoTracking()
                    .FirstOrDefault(e => e.Id == id);
                return new NormalResult<Dictionary>()
                {
                    Data = dictionary
                };
            }
        }

        public NormalResult<Dictionary> GetDictionaryWithItems(Guid id)
        {
            using (Entities db = Entities.CreateContext())
            {
                Dictionary dictionary = db.Dictionary
                    .Include(c => c.DictionaryItem)
                    .AsNoTracking()
                    .FirstOrDefault(e => e.Id == id);
                return new NormalResult<Dictionary>()
                {
                    Data = dictionary
                };
            }
        }

        public NormalResult<Dictionary> GetDictionaryWithItemsByKey(string key)
        {
            using (Entities db = Entities.CreateContext())
            {
                Dictionary dictionary = db.Dictionary
                    .Include(c => c.DictionaryItem)
                    .AsNoTracking()
                    .FirstOrDefault(e => e.Key == key);
                return new NormalResult<Dictionary>()
                {
                    Data = dictionary
                };
            }
        }

        public NormalResult<List<Dictionary>> GetDictionaryWithItemsBundle(List<string> keyList)
        {
            List<Dictionary> result = new List<Dictionary>();
            using (Entities db = Entities.CreateContext())
            {
                IQueryable<Dictionary> queryable = db.Dictionary
                    .Include(c => c.DictionaryItem)
                    .AsNoTracking();

                if (keyList != null && keyList.Count > 0)
                {
                    queryable = queryable.Where(c => keyList.Contains(c.Key));
                }

                return new NormalResult<List<Dictionary>>()
                {
                    Data = queryable.ToList()
                };
            }
        }

        public NormalResult<Guid> CreateDictionary(Dictionary dictionary)
        {
            if (dictionary.Id == Guid.Empty)
            {
                dictionary.Id = Guid.NewGuid();
            }

            using (Entities db = Entities.CreateContext())
            {
                if (db.Dictionary.Any(s => s.Key == dictionary.Key))
                {
                    return new NormalResult<Guid>("指定的 Key 已被占用。");
                }

                if (db.Dictionary.Any(s => s.Name == dictionary.Name))
                {
                    return new NormalResult<Guid>("指定的名称已被占用。");
                }

                db.Dictionary.Add(dictionary);
                db.SaveChanges();
            }

            return new NormalResult<Guid>()
            {
                Data = dictionary.Id
            };
        }

        public NormalResult UpdateDictionary(Dictionary dictionary)
        {
            using (Entities db = Entities.CreateContext())
            {
                if (db.Dictionary.Any(s => s.Key == dictionary.Key && s.Id != dictionary.Id))
                {
                    return new NormalResult("指定的 Key 已被占用。");
                }

                if (db.Dictionary.Any(s => s.Name == dictionary.Name && s.Id != dictionary.Id))
                {
                    return new NormalResult("指定的名称已被占用。");
                }

                IQueryable<Dictionary> queryable = db.Dictionary;

                Dictionary dbDictionary = queryable.FirstOrDefault(e => e.Id == dictionary.Id);
                if (dbDictionary == null)
                    return new NormalResult("指定的数据不存在。");

                ShengMapper.SetValuesSkipVirtual(dictionary, dbDictionary);

                db.SaveChanges();
            }

            return new NormalResult();
        }

        public NormalResult RemoveDictionary(Guid id)
        {
            using (Entities db = Entities.CreateContext())
            {
                Dictionary dictionary = db.Dictionary.FirstOrDefault(e => e.Id == id);
                if (dictionary != null)
                {
                    db.Dictionary.Remove(dictionary);
                    db.SaveChanges();
                }
            }

            return new NormalResult();
        }

        public NormalResult<GetListDataResult<Dictionary>> GetDictionaryList(GetListDataArgs args)
        {
            GetListDataResult<Dictionary> result = new GetListDataResult<Dictionary>();
            using (Entities db = Entities.CreateContext())
            {
                IQueryable<Dictionary> queryable = db.Dictionary
                    .AsNoTracking();


                if (args.Parameters.IsNullOrEmpty("keyword") == false)
                {
                    string keyword = args.Parameters.GetValue<string>("keyword");
                    queryable = queryable.Where(c => c.Name.Contains(keyword));
                }

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

            return new NormalResult<GetListDataResult<Dictionary>>()
            {
                Data = result
            };
        }


        #endregion

        #region DictionaryItem

        public NormalResult<DictionaryItem> GetDictionaryItem(Guid id)
        {
            using (Entities db = Entities.CreateContext())
            {
                DictionaryItem dictionaryItem = db.DictionaryItem
                    .AsNoTracking()
                    .FirstOrDefault(e => e.Id == id);
                return new NormalResult<DictionaryItem>()
                {
                    Data = dictionaryItem
                };
            }
        }


        public NormalResult<Guid> CreateDictionaryItem(DictionaryItem dictionaryItem)
        {
            if (dictionaryItem.Id == Guid.Empty)
            {
                dictionaryItem.Id = Guid.NewGuid();
            }

            using (Entities db = Entities.CreateContext())
            {
                if (db.DictionaryItem.Any(s => s.DictionaryId == dictionaryItem.DictionaryId && s.Text == dictionaryItem.Text))
                {
                    return new NormalResult<Guid>("指定的名称已被占用。");
                }

                db.DictionaryItem.Add(dictionaryItem);
                db.SaveChanges();
            }

            return new NormalResult<Guid>()
            {
                Data = dictionaryItem.Id
            };
        }

        public NormalResult UpdateDictionaryItem(DictionaryItem dictionaryItem)
        {
            using (Entities db = Entities.CreateContext())
            {
                if (db.DictionaryItem.Any(s => s.DictionaryId == dictionaryItem.DictionaryId && s.Text == dictionaryItem.Text && s.Id != dictionaryItem.Id))
                {
                    return new NormalResult("指定的名称已被占用。");
                }

                IQueryable<DictionaryItem> queryable = db.DictionaryItem;

                DictionaryItem dbDictionaryItem = queryable.FirstOrDefault(e => e.Id == dictionaryItem.Id);
                if (dbDictionaryItem == null)
                    return new NormalResult("指定的数据不存在。");

                ShengMapper.SetValuesSkipVirtual(dictionaryItem, dbDictionaryItem);

                db.SaveChanges();
            }

            return new NormalResult();
        }

        public NormalResult RemoveDictionaryItem(Guid id)
        {
            using (Entities db = Entities.CreateContext())
            {
                DictionaryItem dictionaryItem = db.DictionaryItem.FirstOrDefault(e => e.Id == id);
                if (dictionaryItem != null)
                {
                    db.DictionaryItem.Remove(dictionaryItem);
                    db.SaveChanges();
                }
            }

            return new NormalResult();
        }

        public NormalResult<GetListDataResult<DictionaryItem>> GetDictionaryItemList(GetListDataArgs args)
        {
            GetListDataResult<DictionaryItem> result = new GetListDataResult<DictionaryItem>();
            using (Entities db = Entities.CreateContext())
            {
                IQueryable<DictionaryItem> queryable = db.DictionaryItem
                    .AsNoTracking();

                if (args.Parameters.IsNullOrEmpty("dictionaryId"))
                {
                    return new NormalResult<GetListDataResult<DictionaryItem>>("参数错误，必须指定 dictionaryId。");
                }

                Guid dictionaryId = args.Parameters.GetGuidValue("dictionaryId");
                queryable = queryable.Where(c => c.DictionaryId == dictionaryId);

                if (args.Parameters.IsNullOrEmpty("keyword") == false)
                {
                    string keyword = args.Parameters.GetValue<string>("keyword");
                    queryable = queryable.Where(c => c.Text.Contains(keyword));
                }

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

            return new NormalResult<GetListDataResult<DictionaryItem>>()
            {
                Data = result
            };
        }

        /// <summary>
        /// 根据字典父类ID获取字典明细
        /// </summary>
        /// <returns></returns>
        public NormalResult<List<DictionaryItem>> GetDictionaryItemListById(Guid categoryId)
        {
            using (Entities db = Entities.CreateContext())
            {
                List<DictionaryItem> dictionaryItemList = db.DictionaryItem
                        .AsNoTracking()
                        .Where(x => x.DictionaryId == categoryId)
                        .OrderBy(x => x.NumericalOrder)
                        .ToList();

                return new NormalResult<List<DictionaryItem>>() { Data = dictionaryItemList };
            }
        }

        #endregion
    }
}
