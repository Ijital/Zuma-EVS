namespace ZumaService.Services.Implementation
{
    using Microsoft.Extensions.Caching.Memory;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ZumaService.Constants;
    using ZumaService.Models;
    using ZumaService.Repositories.Interface;
    using ZumaService.Services.Interface;

    public class VoteBlockService : IVoteBlockService
    {
        private readonly IVoteBlockRepository _voteBlockRepository;

        private readonly IVoteBlockValidationService _validationService;

        private readonly IMemoryCache _memoryCache;

        public VoteBlockService(IVoteBlockRepository voteBlockRepository,
            IMemoryCache memoryCache,
            IVoteBlockValidationService validationService)
        {
            _voteBlockRepository = voteBlockRepository;
            _memoryCache = memoryCache;
            _validationService = validationService;
        }
        public async Task<bool> BlockChainInitiated()
        {
            return await _voteBlockRepository.MinedVoteBlockRecordsExist();
        }

        public async Task DeletePendingBlock(string blockGuid)
        {
            await _voteBlockRepository.DeletePendingVoteBlock(blockGuid);
        }

        public async Task<VoteBlock> GetLastMinedBlock()
        {
            return await _voteBlockRepository.GetLastMinedVoteBlockAsync();
        }

        public async Task<IEnumerable<VoteBlock>> GetMinedBlocks()
        {
            return await _voteBlockRepository.GetAllMinedVoteBlockAsync();
        }

        public async Task<PendingVoteBlock> GetPendingBlock()
        {
            return await _voteBlockRepository.GetPendingVoteBlock();
        }

        public async Task<bool> SaveMinedBlock(VoteBlock pendingBlock)
        {
            if (pendingBlock.VoteBlockGuid == CacheItems.Genesis)
            {
                await _voteBlockRepository.SaveMinedVoteBlockAsync(pendingBlock);
                _memoryCache.Set(CacheItems.LastMinedBlock, pendingBlock);
                return true;
            }

            bool isVoteValid = await _validationService.Validate(pendingBlock);
            var lastMinedBlock = _memoryCache.Get<VoteBlock>(CacheItems.LastMinedBlock);

            if (isVoteValid && lastMinedBlock != null)
            {
                pendingBlock.VoteBlockIndex = lastMinedBlock.VoteBlockIndex + 1;
                pendingBlock.LastBlockHash = lastMinedBlock.VoteBlockHash;

                pendingBlock.Id = default(int);
                await _voteBlockRepository.SaveMinedVoteBlockAsync(pendingBlock);

                var cacheEntryOptions = new MemoryCacheEntryOptions();
                cacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

                _memoryCache.Set(CacheItems.LastMinedBlock, pendingBlock, cacheEntryOptions);
                await _voteBlockRepository.DeletePendingVoteBlock(pendingBlock.VoteBlockGuid!);

            }
            return isVoteValid;
        }

        public async Task SavePendingBlock(PendingVoteBlock pendingBlock)
        {
            await _voteBlockRepository.SavePendingVoteBlock(pendingBlock);
        }
    }
}
