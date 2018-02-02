using System.Collections.Generic;
//using System.Security;
//using Microsoft.Practices.EnterpriseLibrary.Caching;
//using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace WeighBridgeApplication.Util
{
    public static class CachedDataClass<T> where T : class
    {
        //private static readonly ICacheManager _cacheManager = CacheFactory.GetCacheManager();

        //public static void AddToCache(string key, List<T> entityOList)
        //{
        //    _cacheManager.Add(key, entityOList, CacheItemPriority.High, null, new NeverExpired());
        //}

        //public static bool IsInCache(string key)
        //{
        //    return _cacheManager.Contains(key);
        //}

        //public static List<T> GetFromCache(string key)
        //{
        //    return (List<T>)_cacheManager.GetData(key);
        //}

        //public static void AddUserToCache(string userName, string password)
        //{
        //    _cacheManager.Add(userName, password, CacheItemPriority.High, null, new NeverExpired());
        //}

        //public static bool IsUserValidUserToCache(string userName, string password)
        //{
        //    var cachedPassword = _cacheManager.GetData(userName);

        //    return string.Equals(password, cachedPassword);
        //}
    }
}