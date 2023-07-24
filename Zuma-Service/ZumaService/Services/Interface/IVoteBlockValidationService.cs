using ZumaService.Models;

namespace ZumaService.Services.Interface
{
    public interface IVoteBlockValidationService
    {
        /// <summary>
        /// Validates a mine block and returns bool indicating 
        /// </summary>
        Task<bool> Validate(VoteBlock minedVoteBlock);
    }
}
