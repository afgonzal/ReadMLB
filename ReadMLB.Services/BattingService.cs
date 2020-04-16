using ReadMLB.DataLayer.Repositories;
using ReadMLB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ReadMLB.Services
{
    public interface IBattingService
    {
        Task<long> AddBattingStatAsync(Batting battingStat);

        Task<ICollection<Batting>> GetTeamBattersAsync(short year, short teamId);

        Task<ICollection<Batting>> GetOrganizationBattersAsync(short year, byte organizationId);

        Task<ICollection<Batting>> GetTopLeagueBattersAsync<TKey>(short year, byte league, Expression<Func<Batting, TKey>> category, short top = 50);

        Task TruncateBattingStatsAsync();
    }
    public class BattingService : IBattingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BattingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<long> AddBattingStatAsync(Batting battingStat)
        {
                var result = await _unitOfWork.BattingStats.AddAsync(battingStat);
                await _unitOfWork.CompleteAsync();
                return result.Entity.BattingId;
        }

        public async Task<ICollection<Batting>> GetOrganizationBattersAsync(short year, byte organizationId)
        {
            var batters = await _unitOfWork.BattingStats.FindAsync(b => b.Team, b => b.Year == year && b.Team.OrganizationId == organizationId, b => b.PlayerId);
            return batters.ToList();
        }

        public async Task<ICollection<Batting>> GetTeamBattersAsync(short year, short teamId)
        {
            var batters = await _unitOfWork.BattingStats.FindAsync(new List<string> { "Player", "Team.Organization" }, b => b.Year == year && b.TeamId == teamId, b => b.PlayerId);
            return batters.ToList();
        }

        public async Task<ICollection<Batting>> GetTopLeagueBattersAsync<TKey>(short year, byte league, Expression<Func<Batting,TKey>> category, short top = 50)
        {
            var batters = await _unitOfWork.BattingStats.FindAsync<TKey>(b => b.Year == year && b.League == league, category, top);
            return batters.ToList();
        }

        public Task TruncateBattingStatsAsync()
        {
            return _unitOfWork.TruncateTableAsync("Batting");
        }
    }
}
