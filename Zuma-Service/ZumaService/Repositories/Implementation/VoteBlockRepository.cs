namespace ZumaService.Repositories.Implementation
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ZumaService.Data;
    using ZumaService.Models;
    using ZumaService.Repositories.Interface;
    public class VoteBlockRepository : IVoteBlockRepository
    {
        private readonly ZumaDB _zumaDb;

        public VoteBlockRepository(ZumaDB zumaDb)
        {
            _zumaDb = zumaDb;
        }

        public async Task SavePendingVoteBlock(PendingVoteBlock block)
        {
            await _zumaDb.PendingVoteBlocks.AddAsync(block);
            await _zumaDb.SaveChangesAsync();
        }

        public async Task DeletePendingVoteBlock(string blockGuidId)
        {
            var pendingBlock = await _zumaDb.PendingVoteBlocks.FirstAsync(b => b.VoteBlockGuid == blockGuidId);

            if (pendingBlock != null)
            {
                _zumaDb.PendingVoteBlocks.Remove(pendingBlock);
                _zumaDb.SaveChanges();
            }
        }

        public async Task<PendingVoteBlock> GetPendingVoteBlock()
        {
            return await _zumaDb.PendingVoteBlocks
                .OrderByDescending(b => b.Id)
                .FirstAsync();
        }

        public async Task<PendingVoteBlock> GetPendingVoteBlockById(string blockId)
        {
            return await _zumaDb.PendingVoteBlocks
                .FirstAsync(b => b.VoteBlockGuid == blockId);
        }

        public async Task<IEnumerable<VoteBlock>> GetAllMinedVoteBlockAsync()
        {
           return await _zumaDb.VoteBlocks.ToListAsync();
        }

        public async Task<VoteBlock> GetLastMinedVoteBlockAsync()
        {
            return await _zumaDb.VoteBlocks
                  .OrderBy(b => b.VoteBlockIndex)
                  .LastAsync();
        }
      
        public async Task<bool> MinedVoteBlockRecordsExist()
        {
            return await _zumaDb.VoteBlocks.AnyAsync();
        }

        public async Task SaveMinedVoteBlockAsync(VoteBlock block)
        {
            await _zumaDb.VoteBlocks.AddAsync(block);
            await _zumaDb.SaveChangesAsync();
        }
    }
}
