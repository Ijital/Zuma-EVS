namespace Zuma.Web.Services.Implementation
{
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using ZumaService.Constants;
    using ZumaService.Models;
    using ZumaService.Services.Interface;

    public class VoteBlockMineService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMemoryCache _memoryCache;
        private readonly List<string> _networkNodes;
        private Task? _serviceTask;


        /// <summary>
        /// Initialises an instance of the hosted service
        /// </summary>
        public VoteBlockMineService(IServiceProvider serviceProvider,
            IConfiguration config,
            IMemoryCache memoryCache)
        {
            _serviceProvider = serviceProvider;
            _memoryCache = memoryCache;
            _networkNodes = config.GetSection("NetworkNodes").Get<List<string>>();
        }

        /// <summary>
        /// Starts the hosted service. This method will only be used by INEC node
        /// </summary>
        public Task StartAsync(CancellationToken cancellationToken)
        {

            _serviceTask = Task.Run(() =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var voteBlockService = scope.ServiceProvider
                    .GetRequiredService<IVoteBlockService>();

                    if (!voteBlockService.BlockChainInitiated().Result)
                    {
                        var genesisBlock = new VoteBlock
                        {
                            VoteBlockGuid = CacheItems.Genesis,
                            VoteBlockIndex = 0,
                            VotePacks = string.Empty,
                            VoteBlockHash = string.Empty,
                            LastBlockHash = string.Empty,
                        };

                        voteBlockService.SaveMinedBlock(genesisBlock);
                        var cacheEntryOptions = new MemoryCacheEntryOptions();
                        cacheEntryOptions.AbsoluteExpirationRelativeToNow= TimeSpan.FromHours(1);

                        _memoryCache.Set(CacheItems.LastMinedBlock, genesisBlock, cacheEntryOptions);
                    }

                    // The While Loop below will only be run by the nominated 
                    // proof of stake Node in the block chain network.Naturaly this can be 
                    // the INEC Node

                    while (true)
                    {
                        try
                        {
                            Thread.Sleep(10000);
                            var blockToMine = voteBlockService.GetPendingBlock().Result;
                            var lastMinedBlock = _memoryCache.Get<VoteBlock>(CacheItems.LastMinedBlock);

                            if(lastMinedBlock == null)
                            {
                                var cacheEntryOptions = new MemoryCacheEntryOptions();
                                cacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                                _memoryCache.Set(CacheItems.LastMinedBlock, voteBlockService.GetLastMinedBlock().Result, cacheEntryOptions);
                            }

                            if (blockToMine != null)
                            {
                                MineVoteBlock(blockToMine);
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }

                    }
                }
            });
            return _serviceTask.IsCompleted ? _serviceTask : Task.CompletedTask;
        }

        /// <summary>
        /// Stops the hosted service
        /// </summary>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Mines a new block on to the block chain network
        /// </summary>
        private void MineVoteBlock(PendingVoteBlock blockToMine)
        {
            var minedBlockContent = JsonConvert.SerializeObject(blockToMine);
            var payLoad = new StringContent(minedBlockContent, System.Text.Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                foreach (var node in _networkNodes)
                {
                    var uri = new Uri($"{node}/voteblocks/minevoteblock");
                    var result = client.PostAsync(uri, payLoad).Result;
                }
            }
        }
    }
}