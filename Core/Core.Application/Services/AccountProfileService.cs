using Core.Domain;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;
using Dapper;
using System.Data;

namespace Core.Application.Services {
    public class AccountProfileService: GenericService<AccountProfile>, IAccountProfileService {
        #region
        private readonly IDbConnection _connpool;

        public AccountProfileService(
            ConnectionPool connpool,
            MsSQLDbContext msSQLDbContext) : base(dbContext: msSQLDbContext) {

            _connpool = connpool.DbConnection;
        }
        #endregion

        public async Task<bool> CleanForgotPasswordTokensAsync(int accountId) {
            var query = $"UPDATE dbo.AccountProfile SET ForgotPasswordToken = NULL WHERE AccountId = {accountId};";
            var result = await _connpool.ExecuteAsync(query);
            if(result > 0)
                return true;
            return false;
        }

        public async Task<bool> CleanForgotPasswordTokensAsync(int accountId, IDbContextTransaction transaction = null) {
            var query = $"UPDATE dbo.AccountProfile SET ForgotPasswordToken = NULL WHERE AccountId = {accountId};";
            var result = await GetMsSQLDbContext(transaction).Database.ExecuteSqlRawAsync(query);
            if(result > 0)
                return true;
            return false;
        }
    }
}
