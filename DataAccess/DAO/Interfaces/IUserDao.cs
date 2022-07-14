﻿using System.Threading.Tasks;
using Domain.Entities;

namespace DataAccess.DAO.Interfaces
{
    public interface IUserDao
    {
        Task<User?> GetUserById(int userId);
        Task<bool> Create(User user);
        Task<bool> Update(User user);
    }
}