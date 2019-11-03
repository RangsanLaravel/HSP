using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TeaUnitWebAPI.DataAccess;
using TeaUnitWebAPI.Models;

namespace TeaUnitWebAPI.BussinessLogin
{
    public partial class ServiceAction
    {
        public void Register(Users user) => Register(user, (Repository)null);

        private void Register(Users user, Repository repository)
        {
            bool internalConnection = false;
            if (repository == null)
            {
                repository = new Repository(ConnectionString);
                repository.OpenConnection();
                repository.BeginTransection();
                internalConnection = true;
            }
            try
            {
                repository.Register(user);
                if (internalConnection)
                    repository.Commit();
            }
            catch (System.Exception ex)
            {
                if (internalConnection)
                    repository.RollBack();
                throw ex;
            }
            finally
            {
                if (internalConnection)
                    repository.CloseConnection();
            }
        }

        public void CheckUser(out Users user, string userName) => CheckUser(out user, userName, (Repository)null);

        private void CheckUser(out Users user, string userName, Repository repository)
        {
            bool internalConnection = false;
            if (repository == null)
            {
                repository = new Repository(ConnectionString);
                repository.OpenConnection();
                internalConnection = true;
            }
            try
            {
                repository.CheckUser(out user, userName);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (internalConnection)
                    repository.CloseConnection();
            }

        }
    }
}