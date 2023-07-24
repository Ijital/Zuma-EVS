namespace ZumaService.Repositories.Interface
{
    using ZumaService.Models;
    public interface IVoteBlockRepository
    {
        /// <summary>
        /// Inserts an entity object to data storage
        /// </summary>
        Task SaveMinedVoteBlockAsync(VoteBlock block);

        /// <summary>
        /// Gets all entity records from data storage
        /// </summary>    
        Task<IEnumerable<VoteBlock>> GetAllMinedVoteBlockAsync();

        /// <summary>
        /// Gets the last data entity record
        /// </summary>
        Task<VoteBlock> GetLastMinedVoteBlockAsync();

        /// <summary>
        /// Checks if records exist
        /// </summary>
        Task<bool> MinedVoteBlockRecordsExist();


        /// <summary>
        /// Saves a new pending vote block
        /// </summary>
        Task SavePendingVoteBlock(PendingVoteBlock block);

        /// <summary>
        /// Gets the first Pending vote block
        /// </summary>
        Task<PendingVoteBlock> GetPendingVoteBlock();

        /// <summary>
        /// Gets the first Pending vote block
        /// </summary>
        Task<PendingVoteBlock> GetPendingVoteBlockById(string blockId);

        /// <summary>
        /// Deletes a pending vote block by Id
        /// </summary>
        Task DeletePendingVoteBlock(string pendingBlockId);
    }
}
