﻿using Leave_Management.Contracts;
using Leave_Management.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leave_Management.Repository
{
    public class LeaveAllocationRepository : ILeaveAllocationRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveAllocationRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CheckAllocation(int leavetypeid, string employeeid)
        {
            var period = DateTime.Now.Year;
            return FindAll()
                .Where(q => q.EmployeeId == employeeid &&
                            q.LeaveTypeId == leavetypeid &&
                            q.Period == period)
                .Any();
        }

        public bool Create(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Add(entity);
            return Save();
        }

        public bool Delete(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Remove(entity);
            return Save();
        }

        public ICollection<LeaveAllocation> FindAll()
        {
            var LeaveAllocations = _db.LeaveAllocations.Include(q =>q.LeaveType).ToList();
            return LeaveAllocations;
        }

        public LeaveAllocation FindById(int id)
        {
            var LeaveAllocation = _db.LeaveAllocations.Find(id);
            return LeaveAllocation;
        }

        public ICollection<LeaveAllocation> GetLeaveAllocationsByEmployee(string id)
        {
            var period = DateTime.Now.Year;
            return FindAll()
                .Where(l => l.EmployeeId == id && l.Period == period)
                .ToList();
        }

        public bool isExists(int id)
        {
            var exists = _db.LeaveAllocations.Any(l => l.Id.Equals(id));
            return exists;
        }

        public bool Save()
        {
            var changes = _db.SaveChanges();
            return changes > 0;
        }

        public bool Update(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Update(entity);
            return Save();
        }
    }
}
