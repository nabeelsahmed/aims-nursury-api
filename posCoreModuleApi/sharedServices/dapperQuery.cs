
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Options;
using posCoreModuleApi.Configuration;
using posCoreModuleApi.Entities;
using Npgsql;
using System.IO;

namespace posCoreModuleApi.Services
{
    public class dapperQuery
    {
        // private readonly IOptions<conStr> _dbCon;

        // public dapperQuery(IOptions<conStr> dbCon)
        // {
        //     _dbCon = dbCon;
        //     // _dynamic = dynamic;
        //     // _dynamicString = dynamicString;
        //     // _subconStr = SubdbCon;
        // }

        public static IEnumerable<T> Qry<T>(string sql, IOptions<conStr> conStr)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(conStr.Value.dbCon))
            {
                return con.Query<T>(sql);
            }
        }

        // public static string FindMe (int userID)
        // {
        //     try
        //     {
        //         string cmd = "Select * from view_getcompany where \"userID\" = " + userID + " "; // corrected query string

        //         var user = (List<dynamicResponse>)dapperQuery.Qry<dynamicResponse>(cmd, _dbCon); // assuming _dapper is properly instantiated

        //         if (user.Count > 0)
        //         {
        //             return "Host="+user[0].instanceName+";Database="+user[0].dbName+";Port=5432;Username="+user[0].userName+";Password="+user[0].credentails+"";
        //         }
        //         else
        //         {
        //             return null; // or return an appropriate value when no results are found
        //         }
        //     }
        //     catch (Exception e)
        //     {
        //         // Handle exception
        //         return null; // or return an appropriate value when an exception occurs
        //     }
        // }

        public static IEnumerable<T> QryResult<T>(string sql, IOptions<conStr> conStr)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(conStr.Value.dbCon))
            {
                return con.Query<T>(sql).ToList();
            }
        }
        public static IEnumerable<T> StrConQry<T>(string sql, string conStr)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(conStr))
            {
                return con.Query<T>(sql).ToList();
            }
        }
        public static string saveImageFile(string regPath, string name, string binData, string ext)
        {
            String path = regPath; //Path
            //Check if directory exist
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
            }

            string imageName = name + "." + ext;

            //set the image path
            string imgPath = Path.Combine(path, imageName);

            byte[] imageBytes = Convert.FromBase64String(binData);

            System.IO.File.WriteAllBytes(imgPath, imageBytes);

            return "Ok";
        }
        // public static IEnumerable<int> CRUDQry(string query, DynamicParameters parameters, IOptions<conStr> conStr)
        // {
        //     using (NpgsqlConnection con = new NpgsqlConnection(conStr.Value.dbCon))
        //     {
        //         var rowAffected = con.Execute(query, parameters, commandType: CommandType.Text);

        //         yield return rowAffected;
        //     }
        // }
    }
}