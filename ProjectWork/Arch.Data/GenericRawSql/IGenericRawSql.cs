using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
namespace Arch.Data.GenericRepository
{
    public interface IGenericRawSql<TEntity> where TEntity : class
    {
        DbRawSqlQuery Execute(string connstr, Type a, string sql, params object[] Parameter);
        int ExecuteSqlCommand(string connstr, string sql, params object[] Parameter);
        List<Dictionary<string, object>> ExecuteSql(string connstr, string sql, params object[] Parameters);
        DataTable Execute(string connstr, string sql, params object[] Parameter);
        string ExecuteSqlJsonOutput(string connstr, string sql, params object[] Parameter);
        string ExecuteOracleJsonOutput(string connstr, string sql, params object[] Parameter);
        DbRawSqlQuery<T> ExecuteQuery<T>(string connstr, T a, string sql, params object[] Parameter);
    }
}