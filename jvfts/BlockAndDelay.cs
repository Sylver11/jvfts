using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Caching;



namespace jvfts
{
    class BlockAndDelay
    {
        private readonly MemoryCache _memCache;
        private readonly CacheItemPolicy _cacheItemPolicy;
        private const int CacheTimeMilliseconds = 1000;

        //private string path = "/Users/justus/source";

        public BlockAndDelay(string path)
        {
            _memCache = MemoryCache.Default;

            var watcher = new FileSystemWatcher()
            {
                Path = path,
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = "*.txt"
            };

            _cacheItemPolicy = new CacheItemPolicy()
            {
                RemovedCallback = OnRemovedFromCache
            };

            watcher.Changed += OnChanged;
            watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            _cacheItemPolicy.AbsoluteExpiration =
                DateTimeOffset.Now.AddMilliseconds(CacheTimeMilliseconds);

            _memCache.AddOrGetExisting(e.Name, e, _cacheItemPolicy);
        }

        private void OnRemovedFromCache(CacheEntryRemovedArguments args)
        {
            if (args.RemovedReason != CacheEntryRemovedReason.Expired) return;
            var e = (FileSystemEventArgs)args.CacheItem.Value;
            Console.WriteLine("Changed item: '{0}'.", e.FullPath);
            var e_ = new FileCopy(e.FullPath);
        }
    }
}
