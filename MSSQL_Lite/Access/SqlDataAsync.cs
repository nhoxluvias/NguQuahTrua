﻿using MSSQL_Lite.Config;
using MSSQL_Lite.Execution;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MSSQL_Lite.Access
{
    internal partial class SqlData : SqlExecution
    {
        //public async Task ExecuteReaderAsync(SqlCommand sqlCommand)
        //{
        //    if (SqlConfig.objectReceivingData == ObjectReceivingData.SqlDataReader)
        //        data = await ExecuteReaderAsync<SqlDataReader>(sqlCommand);
        //    else
        //        data = await ExecuteReaderAsync<DataSet>(sqlCommand);
        //}

        public async Task<Dictionary<string, object>> ToDictionaryAsync(SqlCommand sqlCommand)
        {
            if (SqlConfig.objectReceivingData == ObjectReceivingData.SqlDataReader)
            {
                using (SqlDataReader sqlDataReader = await ExecuteReaderAsync<SqlDataReader>(sqlCommand))
                {
                    return sqlConvert.ToDictionary(sqlDataReader);
                }
            }
            else
            {
                using (DataSet dataSet = await ExecuteReaderAsync<DataSet>(sqlCommand))
                {
                    return sqlConvert.ToDictionary(dataSet);
                }
            }
        }

        public async Task<List<Dictionary<string, object>>> ToDictionaryListAsync(SqlCommand sqlCommand)
        {
            if (SqlConfig.objectReceivingData == ObjectReceivingData.SqlDataReader)
            {
                using (SqlDataReader sqlDataReader = await ExecuteReaderAsync<SqlDataReader>(sqlCommand))
                {
                    return sqlConvert.ToDictionaryList(sqlDataReader);
                }
            }
            else
            {
                using (DataSet dataSet = await ExecuteReaderAsync<DataSet>(sqlCommand))
                {
                    return sqlConvert.ToDictionaryList(dataSet);
                }
            }
        }

        public async Task<T> ToAsync<T>(SqlCommand sqlCommand)
        {
            if (SqlConfig.objectReceivingData == ObjectReceivingData.SqlDataReader)
            {
                using (SqlDataReader sqlDataReader = await ExecuteReaderAsync<SqlDataReader>(sqlCommand))
                {
                    return sqlConvert.To<T>(sqlDataReader);
                }
            }
            else
            {
                using (DataSet dataSet = await ExecuteReaderAsync<DataSet>(sqlCommand))
                {
                    return sqlConvert.To<T>(dataSet);
                }
            }
        }

        public async Task<List<T>> ToListAsync<T>(SqlCommand sqlCommand)
        {
            if (SqlConfig.objectReceivingData == ObjectReceivingData.SqlDataReader)
            {
                using (SqlDataReader sqlDataReader = await ExecuteReaderAsync<SqlDataReader>(sqlCommand))
                {
                    return sqlConvert.ToList<T>(sqlDataReader);
                }
            }
            else
            {
                using (DataSet dataSet = await ExecuteReaderAsync<DataSet>(sqlCommand))
                {
                    return sqlConvert.ToList<T>(dataSet);
                }
            }
        }

        public async Task<object> ToOriginalDataAsync(SqlCommand sqlCommand)
        {
            if (SqlConfig.objectReceivingData == ObjectReceivingData.SqlDataReader)
                return await ExecuteReaderAsync<SqlDataReader>(sqlCommand);
            return await ExecuteReaderAsync<DataSet>(sqlCommand);
        }
    }
}