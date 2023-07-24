namespace ZumaService.Services.Implementation
{
    using Microsoft.Extensions.Caching.Memory;
    using Newtonsoft.Json;
    using ZumaService.Constants;
    using ZumaService.Models;
    using ZumaService.Services.Interface;

    public class ElectionReportService : IElectionReportService
    {

        private readonly IVoteBlockService _voteBlockService;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _config;

        public ElectionReportService(IMemoryCache memoryCache,
            IVoteBlockService voteBlockService,
             IConfiguration config)
        {
            _voteBlockService = voteBlockService;
            _memoryCache = memoryCache;
            _config = config;
            this.CacheBlockChain();
        }

        public Task<ElectionReport> GetPresidentialReportAsync()
        {
            Task<ElectionReport> task = Task.Run(() =>
            {
                var voteBlocks = _memoryCache.Get<List<VoteBlock>>(CacheItems.VotesBlockChain);

                var votePacks = new List<VotePack>();

                foreach (var voteBlock in voteBlocks!)
                {
                    if (voteBlock.VoteBlockGuid != CacheItems.Genesis)
                    {
                        var blockvotePacks = JsonConvert.DeserializeObject<List<VotePack>>(voteBlock.VotePacks!);
                        votePacks.AddRange(blockvotePacks!);
                    }
                }

                return new ElectionReport()
                {
                    Delineation = "NATIONWIDE",
                    ElectionType = "PRESIDENT",
                    PartyVoteCounts = this.GetPartyVoteCounts(votePacks, VoteTypes.President)
                };
            });

            return task;
        }

        public Task<ElectionReport> GetSenatorialReportAsync(string district)
        {
            var LGs = _config.GetSection($"Elections:Senate:{district}").Get<List<int>>();

            Task<ElectionReport> task = Task.Run(() =>
            {
                var voteBlocks = _memoryCache.Get<List<VoteBlock>>(CacheItems.VotesBlockChain);

                var votePacks = new List<VotePack>();

                foreach (var voteBlock in voteBlocks!)
                {
                    if (voteBlock.VoteBlockGuid != CacheItems.Genesis)
                    {
                        var blockvotePacks = JsonConvert.DeserializeObject<List<VotePack>>(voteBlock.VotePacks!);
                        votePacks.AddRange(blockvotePacks!);
                    }
                }

                return new ElectionReport()
                {
                    Delineation = "BENUE NORTH",
                    ElectionType = "SENATE",
                    PartyVoteCounts = this.GetPartyVoteCounts(votePacks, VoteTypes.Senate)
                };
            });

            return task;
        }

        public Task<ElectionReport> GetRepresentativeReportAsync(string constituency)
        {
            var LGs = _config.GetSection($"Elections:Representative:{constituency}").Get<List<int>>();

            Task<ElectionReport> task = Task.Run(() =>
            {
                var voteBlocks = _memoryCache.Get<List<VoteBlock>>(CacheItems.VotesBlockChain);

                var votePacks = new List<VotePack>();

                foreach (var voteBlock in voteBlocks!)
                {
                    if (voteBlock.VoteBlockGuid != CacheItems.Genesis)
                    {
                        var blockvotePacks = JsonConvert.DeserializeObject<List<VotePack>>(voteBlock.VotePacks!);
                        votePacks.AddRange(blockvotePacks!);
                    }
                }

                return new ElectionReport()
                {
                    Delineation = "NATIONWIDE",
                    ElectionType = "REPRESENTATIVE",
                    PartyVoteCounts = this.GetPartyVoteCounts(votePacks, VoteTypes.Representive)
                };
            });

            return task;
        }

        public Task<ElectionReport> GetGubernatorialReportAsync(string state)
        {
            Task<ElectionReport> task = Task.Run(() =>
            {
                var voteBlocks = _memoryCache.Get<List<VoteBlock>>(CacheItems.VotesBlockChain);

                var votePacks = new List<VotePack>();

                foreach (var voteBlock in voteBlocks!)
                {
                    if (voteBlock.VoteBlockGuid != CacheItems.Genesis)
                    {
                        var blockvotePacks = JsonConvert.DeserializeObject<List<VotePack>>(voteBlock.VotePacks!);
                        votePacks.AddRange(blockvotePacks!);
                    }
                }
                return new ElectionReport()
                {
                    Delineation = "NATIONWIDE",
                    ElectionType = "GUBERNATORIAL",
                    PartyVoteCounts = this.GetPartyVoteCounts(votePacks, VoteTypes.Governor)
                };
            });

            return task;
        }

        public Task<ElectionReport> GetAssemblyReportAsync(string constituency)
        {
            var LGs = _config.GetSection($"Elections:Assembly:{constituency}").Get<List<int>>();

            Task<ElectionReport> task = Task.Run(() =>
            {
                var voteBlocks = _memoryCache.Get<List<VoteBlock>>(CacheItems.VotesBlockChain);

                var votePacks = new List<VotePack>();

                foreach (var voteBlock in voteBlocks!)
                {
                    if (voteBlock.VoteBlockGuid != CacheItems.Genesis)
                    {
                        var blockvotePacks = JsonConvert.DeserializeObject<List<VotePack>>(voteBlock.VotePacks!);
                        votePacks.AddRange(blockvotePacks!);
                    }
                }

                return new ElectionReport()
                {
                    Delineation = "NATIONWIDE",
                    ElectionType = "ASSEMBLY",
                    PartyVoteCounts = this.GetPartyVoteCounts(votePacks, VoteTypes.Assembly)
                };
            });

            return task;
        }

        /// <summary>
        /// Caches the block chain in memory
        /// </summary>
        private void CacheBlockChain()
        {
            if (_voteBlockService.BlockChainInitiated().Result)
            {
                var options = new MemoryCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromHours(1)
                };
                _memoryCache.Set(CacheItems.VotesBlockChain, _voteBlockService.GetMinedBlocks().Result, options);
            }
        }

        /// <summary>
        /// Gets vote counts for each party from vote packs relating a given election type
        /// </summary>
        private List<PartyVoteCount> GetPartyVoteCounts(List<VotePack> votePacks, string voteType)
        {
            var partyVoteCounts = new List<PartyVoteCount>();
            var partyVote = typeof(VotePack).GetProperty(voteType);

            foreach (var party in Parties.Codes)
            {
                var partyVoteCount = new PartyVoteCount { Party = party };
                var partyTotalCount = votePacks.Where(v => partyVote!.GetValue(v)!.ToString() == party);
                partyVoteCount.TotalVote = partyTotalCount.Count();

                partyVoteCount.MaleVote = partyTotalCount.Count(v => v.VoterGender == "M");
                partyVoteCount.FemaleVote = partyVoteCount.TotalVote - partyVoteCount.MaleVote;
                partyVoteCounts.Add(partyVoteCount);
            }

            return partyVoteCounts;
        }
    }
}
