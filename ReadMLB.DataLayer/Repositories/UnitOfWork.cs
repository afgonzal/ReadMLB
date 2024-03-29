﻿using ReadMLB.DataLayer.Context;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ReadMLB.DataLayer.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ITeamsRepository Teams { get; }
        IPlayersRepository Players { get; }
        IPitchingRepository PitchingStats { get; }

        IRosterRepository Rosters { get; }

        IRunningRepository RunningStats{ get; }

        IBattingRepository BattingStats { get; }
        IDefenseRepository DefenseStats { get; }
        IRotationRepository Rotations { get; }
        IScheduleRepository Schedule { get; }


        Task<int> CompleteAsync();
        Task TruncateTableAsync(string tableName);
        Task CleanYearFromTableAsync(string tableName, short year, bool? inPO = null);
        void DisableTracking();
        void EnableTracking();

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

        private IRosterRepository _rosters;
        public IRosterRepository Rosters => _rosters ?? (_rosters = new RosterRepository(_context));

        private IRunningRepository _runningStats;
        public IRunningRepository RunningStats => _runningStats ?? (_runningStats = new RunningRepository(_context));

        private IDefenseRepository _defenseStats;
        public IDefenseRepository DefenseStats => _defenseStats ?? (_defenseStats = new DefenseRepository(_context));

        private IRotationRepository _rotations;
        public IRotationRepository Rotations => _rotations ?? (_rotations = new RotationRepository(_context));

        private IScheduleRepository _schedule;
        public IScheduleRepository Schedule => _schedule ?? (_schedule = new ScheduleRepository(_context));

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

        public Task CleanYearFromTableAsync(string tableName, short year, bool? inPO = null)
        {
            if (inPO.HasValue)
                return _context.Database.ExecuteSqlRawAsync(sql: $"DELETE FROM {tableName} WHERE Year = {year} AND InPO = {(inPO.Value ? 1 : 0)}");
            else
                return _context.Database.ExecuteSqlRawAsync(sql: $"DELETE FROM {tableName} WHERE Year = {year}");
        }

      

        public void DisableTracking()
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public void EnableTracking()
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = true;
        }
    }
}
