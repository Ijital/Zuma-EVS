namespace ZumaService.Services.Implementation
{
    using Microsoft.Extensions.Caching.Memory;
    using ZumaService.Models;
    using ZumaService.Repositories.Interface;
    using ZumaService.Services.Interface;

    public class VoteBlockValidationService : IVoteBlockValidationService
    {

        private readonly IVoteBlockRepository _voteBlockRepository;
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// Initialises an instance of Block validation service
        /// </summary>
        public VoteBlockValidationService(IVoteBlockRepository voteBlockDto,
            IMemoryCache memoryCache)
        {
            _voteBlockRepository = voteBlockDto;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Validates a mined vote blockToMine
        /// </summary>
        public async Task<bool> Validate(VoteBlock blockToMine)
        {
            var pendingBlock = await _voteBlockRepository.GetPendingVoteBlockById(blockToMine.VoteBlockGuid!);
            return pendingBlock != null;
        }
    }
}
