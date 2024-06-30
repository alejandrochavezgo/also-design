namespace providerData;

using Newtonsoft.Json;
using common.logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class applicationUserManager : IUserStore<applicationUser>, IUserPasswordStore<applicationUser>, IUserRoleStore<applicationUser>
{

    private log? logger = new log();
    private readonly applicationDbContext _dbContext;

    public applicationUserManager(applicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task AddToRoleAsync(applicationUser user, string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> CreateAsync(applicationUser user, CancellationToken cancellationToken)
    {
        try
        {
            _dbContext.users.Add(user);
            _dbContext.SaveChanges();
            return Task.FromResult(IdentityResult.Success);
        }
        catch (Exception exception)
        {
            logger!.logError($"{JsonConvert.SerializeObject(exception)}");
            return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = $"Could not insert user {user.UserName}." }));
        }
    }

    public Task<IdentityResult> DeleteAsync(applicationUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        // throw new NotImplementedException();
    }

    public Task<applicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<applicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        try
        {
            return Task.FromResult(_dbContext.Users.FirstOrDefault(x => x.NormalizedUserName == normalizedUserName));
        }
        catch (Exception exception)
        {
            logger!.logError($"{JsonConvert.SerializeObject(exception)}");
            throw;
        }
    }

    public Task<string?> GetNormalizedUserNameAsync(applicationUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IList<string>> GetRolesAsync(applicationUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetUserIdAsync(applicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult($"{user.NormalizedId}");
    }

    public Task<string?> GetUserNameAsync(applicationUser user, CancellationToken cancellationToken)
    {

        return Task.FromResult(user.NormalizedUserName);
    }

    public Task<IList<applicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> HasPasswordAsync(applicationUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsInRoleAsync(applicationUser user, string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task RemoveFromRoleAsync(applicationUser user, string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetNormalizedUserNameAsync(applicationUser user, string? normalizedName, CancellationToken cancellationToken)
    {
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public Task SetPasswordHashAsync(applicationUser user, string? passwordHash, CancellationToken cancellationToken)
    {
        user.Password = passwordHash;
        return Task.CompletedTask;
    }

    public Task SetUserNameAsync(applicationUser user, string? userName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> UpdateAsync(applicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(IdentityResult.Success);
    }

    public Task<string?> GetPasswordHashAsync(applicationUser user, CancellationToken cancellationToken)
    {
        try
        {
            return Task.FromResult(_dbContext.Users.Where(x => x!.NormalizedId == user.NormalizedId)
                                                    .Select(x => x!.Password)
                                                    .FirstOrDefault());
        }
        catch (Exception exception)
        {
            logger!.logError($"{JsonConvert.SerializeObject(exception)}");
            throw;
        }
    }
}