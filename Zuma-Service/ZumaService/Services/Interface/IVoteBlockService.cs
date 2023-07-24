namespace ZumaService.Services.Interface
{
    using ZumaService.Models;
    public interface IVoteBlockService
    {
        /// <summary>
        /// Saves a new mined vote block
        /// </summary>
        Task<bool> SaveMinedBlock(VoteBlock block);

        /// <summary>
        /// Gets all mined vote blocks
        /// </summary>
        Task<IEnumerable<VoteBlock>> GetMinedBlocks();

        /// <summary>
        /// Gets the last mined vote block on the block chain
        /// </summary>
        Task<VoteBlock> GetLastMinedBlock();

        /// <summary>
        /// Checks if Block chain is empty
        /// </summary>
        Task<bool> BlockChainInitiated();

        /// <summary>
        /// Saves a new pending vote block
        /// </summary>
        Task SavePendingBlock(PendingVoteBlock pendingBlock);


        /// <summary>
        /// Gets the first Pending vote block
        /// </summary>
        Task<PendingVoteBlock> GetPendingBlock();

        /// <summary>
        /// Deletes a pending vote block by Id
        /// </summary>
        Task DeletePendingBlock(string blockGuid);

    }
}