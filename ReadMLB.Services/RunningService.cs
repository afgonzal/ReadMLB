﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ReadMLB.DataLayer.Repositories;
using ReadMLB.Entities;

namespace ReadMLB.Services
{
    public interface IRunningStatsService
    {
        Task<long> AddRunningStat(Running newRunningStat);
        Task CleanYearAsync(short year, bool inPO);
        Task<List<Running>> GetPlayerRunningStatsAsync(long playerId, bool inPo = false);
    }
    public class RunningStatsService : IRunningStatsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RunningStatsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<long> AddRunningStat(Running newRunningStat)
        {
            var result = await _unitOfWork.RunningStats.AddAsync(newRunningStat);
            await _unitOfWork.CompleteAsync();
            return result.Entity.RunningId;
        }

        public Task CleanYearAsync(short year, bool inPO)
        {
            return _unitOfWork.CleanYearFromTableAsync("Running", year, inPO);
        }

        public Task<List<Running>> GetPlayerRunningStatsAsync(long playerId, bool inPo = false)
        {
            return _unitOfWork.RunningStats.FindAsync(r => r.Team.Organization, r => r.PlayerId == playerId && r.InPO == inPo);
        }
    }
}
