using System;
using System.Threading.Tasks;
using Application.Services.EntityServices.Interfaces;
using DataAccess.Dao.Interfaces;
using Domain.Entities;

namespace Application.Services.EntityServices;

public class UserService : IUserService
{
    private readonly IUserDao _userDao;

    public UserService(IUserDao userDao)
    {
        _userDao = userDao;
    }

    public Task<User?> GetByExternalId(long externalId)
    {
        return _userDao.GetByExternalId(externalId);
    }

    public async Task<bool> Create(User user)
    {
        var existingUser = await _userDao.GetByExternalId(user.ExternalId);

        if (existingUser != null)
        {
            throw new Exception(
                $"User is already exists in database with external id: {existingUser.ExternalId}");
        }

        return await _userDao.Create(user);
    }

    public async Task<bool> UpdateCity(long externalId, int? cityId)
    {
        var user = await _userDao.GetByExternalId(externalId);

        if (user == null)
        {
            throw new Exception(
                $"User not found in database by external id: {externalId}");
        }

        return await _userDao.UpdateCity(user.Id, cityId);
    }
}