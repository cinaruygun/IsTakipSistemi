using Arch.Data.Context;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
namespace Arch.Data.GenericRepository
{
    public class GenericRawSql<TEntity> : IGenericRawSql<TEntity> where TEntity : class
    {
        private readonly CommonContext _context;
        private readonly DbSet<TEntity> _dbSet;
        public GenericRawSql(CommonContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual DbRawSqlQuery Execute(string connstr, Type a, string sql, params object[] Parameter)
        {
            _context.Database.Connection.ConnectionString = connstr;
            return _context.Database.SqlQuery(a, sql, Parameter);
        }
        public virtual int ExecuteSqlCommand(string connstr, string sql, params object[] Parameter)
        {
            _context.Database.Connection.ConnectionString = connstr;
            return _context.Database.ExecuteSqlCommand(sql, Parameter);
        }
        public virtual List<Dictionary<string, object>> ExecuteSql(string connstr, string sql, params object[] Parameters)
        {
            SqlConnection connection = null;
            connection = new SqlConnection(connstr);
            connection.Open();
            SqlCommand cmd = new SqlCommand(sql, connection);
            if (Parameters.Length > 0 && Parameters[0] is int == false && ((object[])Parameters[0])!=null && ((object[])Parameters[0]).Length > 0)
                for (Int32 i = 0; i < ((object[])Parameters[0]).Length; i++)
                    cmd.Parameters.AddWithValue("@" + i, ((Object)((object[])Parameters[0])[i] == null || (Object)((object[])Parameters[0])[i] == "") ? DBNull.Value : (Object)((object[])Parameters[0])[i]);
            else
                for (Int32 i = 0; i < Parameters.Length; i++)
                    cmd.Parameters.AddWithValue("@" + i, (Object)Parameters[i] ?? DBNull.Value);
            SqlDataReader reader = null;
            reader = cmd.ExecuteReader();
            List<Dictionary<string, object>> expandolist = new List<Dictionary<string, object>>();
            foreach (var item in reader)
            {
                IDictionary<string, object> expando = new ExpandoObject();
                foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(item))
                {
                    var obj = propertyDescriptor.GetValue(item);
                    expando.Add(propertyDescriptor.Name, obj);
                }
                expandolist.Add(new Dictionary<string, object>(expando));
            }
            reader.Close();
            connection.Close();
            return expandolist;
        }
        public virtual DbRawSqlQuery<T> ExecuteQuery<T>(string connstr, T a, string sql, params object[] Parameter)
        {
            _context.Database.Connection.ConnectionString = connstr;
            return _context.Database.SqlQuery<T>(sql, Parameter);
        }

        public virtual DataTable Execute(string connstr, string sql, params object[] Parameter)
        {
            DataTable dt = new DataTable();
            OracleConnection connection = null;
            connection = new OracleConnection(connstr);
            connection.Open();
            OracleCommand cmd = new OracleCommand(sql, connection);
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;
            OracleDataReader reader = null;
            reader = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(reader);
            reader.Close();
            connection.Close();
            return dt;
        }

        public virtual string ExecuteSqlJsonOutput(string connstr, string sql, params object[] Parameter)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = null;
            connection = new SqlConnection(connstr);
            connection.Open();
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;
            SqlDataReader reader = null;
            reader = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(reader);
            reader.Close();
            connection.Close();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return JsonConvert.SerializeObject(rows);
        }

        public virtual string ExecuteOracleJsonOutput(string connstr, string sql, params object[] Parameter)
        {
            DataTable dt = new DataTable();
            OracleConnection connection = null;
            connection = new OracleConnection(connstr);
            connection.Open();
            OracleCommand cmd = new OracleCommand(sql, connection);
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;
            OracleDataReader reader = null;
            reader = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(reader);
            reader.Close();
            connection.Close();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return JsonConvert.SerializeObject(rows);
        }
    }
}