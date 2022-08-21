using System;
using System.Threading.Tasks;
using Application.Services.EntityServices.Interfaces;
using DataAccess.Dao.Interfaces;
using Domain.Entities;
using Domain.Entities.Enums;

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

    public Task<bool> Create(User user)
    {
        return _userDao.Create(user);
    }
    
    public async Task<bool> UpdateState(long externalId, DialogState newState)
    {
        var user = await _userDao.GetByExternalId(externalId);

        if (user == null)
        {
            throw new Exception(
                $"User not found in database by external id: {externalId}");
        }

        var previousState = user.CurrentDialogState;
        var stateChangeDate = DateTime.UtcNow;

        return await _userDao.UpdateState(
            user.Id,
            previousState,
            newState,
            stateChangeDate);
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