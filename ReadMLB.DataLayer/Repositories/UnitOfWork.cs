using Microsoft.EntityFrameworkCore;
using ReadMLB.DataLayer.Context;
using System;
using System.Threading.Tasks;

namespace ReadMLB.DataLayer.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ITeamsRepository Teams { get; }
        IPlayersRepository Players { get; }

        IBattingRepository BattingStats { get; }
        Task<int> CompleteAsync();
        Task TruncateTableAsync(string tableName);
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly FranchiseContext _context;

        public UnitOfWork(FranchiseContext context)
        {
            _context = context;
        }
        private ITeamsRepository _teams;
        public ITeamsRepository Teams => _teams ?? (_teams = new TeamsRepository(_context));

        private IPlayersRepository _players;
        public IPlayersRepository Players => _players ?? (_players = new PlayersRepository(_context));


        private IBattingRepository _battingStats;
        public IBattingRepository BattingStats => _battingStats ?? (_battingStats = new BattingRepository(_context));
        public Task<int> CompleteAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task TruncateTableAsync(string tableName)
        {
            return _context.Database.ExecuteSqlRawAsync($"Truncate Table {tableName}");
        }
    }
}
