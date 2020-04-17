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
        IPitchingRepository PitchingStats { get; }


        IBattingRepository BattingStats { get; }
        Task<int> CompleteAsync();
        Task TruncateTableAsync(string tableName);
        Task CleanYearFromTableAsync(string tableName, short year);
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

        private IPitchingRepository _pitchingStats;
        public IPitchingRepository PitchingStats => _pitchingStats ?? (_pitchingStats = new PitchingRepository(_context));

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

        public Task CleanYearFromTableAsync(string tableName, short year)
        {
            return _context.Database.ExecuteSqlRawAsync($"DELETE FROM {tableName} WHERE Year = {year}");
        }
    }
}
