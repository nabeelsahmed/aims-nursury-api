using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using posCoreModuleApi.Services;
using Microsoft.Extensions.Options;
using posCoreModuleApi.Configuration;
using posCoreModuleApi.Entities;
using Dapper;
using System.Data;
using Npgsql;
using System.Collections.Generic;

namespace posCoreModuleApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class DesignationController : ControllerBase
    {
        private readonly IOptions<conStr> _dbCon;
        private readonly dapperQuery _dapperQuery;
        private string cmd, cmd2;
        public string saveConStr;

        public DesignationController(dapperQuery dapperQuery,IOptions<conStr> dbCon)
        {
            _dbCon = dbCon;
            _dapperQuery = dapperQuery;
        }

        [HttpGet("getDesignation")]
        public IActionResult getDesignation(int branchID, int companyID,int userID, int moduleId)
        {
            try
            {
                if(companyID == 0 && branchID != 0){
                    cmd = "SELECT * FROM public.designation where \"isDeleted\"::int = 0 AND \"branchID\" = " + branchID + "";
                }else{
                cmd = "SELECT * FROM public.designation where \"isDeleted\"::int = 0 AND \"branchID\" = " + branchID + " AND \"companyid\" = " + companyID + "";
                }
                var appMenu = _dapperQuery.StrConQry<Designation>(cmd,userID,moduleId);
                return Ok(appMenu);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }


        [HttpPost("saveDesignation")]
        public IActionResult saveDesignation(DesignationCreation obj)
        {
            try
            {
                DateTime curDate = DateTime.Today;

                DateTime curTime = DateTime.Now;

                var time = curTime.ToString("HH:mm");

                int rowAffected = 0;
                var response = "";
                var found = false;
                var designation = "";

                List<Designation> appMenuDesignation = new List<Designation>();
                cmd2 = "select \"desginationName\" from designation where \"isDeleted\"::int = 0 and \"desginationName\" = '" + obj.designationName + "' and \"businessid\" = " + obj.businessid + " AND \"companyid\" = " + obj.companyid + " AND \"branchID\" = " + obj.branchID + "";
                appMenuDesignation = (List<Designation>)_dapperQuery.StrConQry<Designation>(cmd2, obj.userID,obj.moduleId);

                if (appMenuDesignation.Count > 0)
                    designation = appMenuDesignation[0].desginationName;

                if (obj.designationID == 0)
                {

                    if (designation == "")
                    {
                        cmd = "insert into public.designation (\"desginationName\", \"createdOn\", \"createdBy\", \"isDeleted\",\"businessid\",\"companyid\",\"branchID\") values ('" + obj.designationName + "', '" + curDate + "', " + obj.userID + ", B'0'," + obj.businessid + ", " + obj.companyid + ", " + obj.branchID + ")";
                    }
                    else
                    {
                        found = true;
                    }
                }
                else
                {
                    cmd = "update public.designation set \"desginationName\" = '" + obj.designationName + "', \"modifiedOn\" = '" + curDate + "', \"modifiedBy\" = " + obj.userID + " where \"designationID\" = " + obj.designationID + ";";
                }

                if (found == false)
                {
                    if(obj.userID != 0 && obj.moduleId !=0)
                    {
                    saveConStr = _dapperQuery.FindMe(obj.userID,obj.moduleId);
                    }
                    
                    using (NpgsqlConnection con = new NpgsqlConnection(saveConStr))
                    {
                        rowAffected = con.Execute(cmd);
                    }
                }

                if (rowAffected > 0)
                {
                    response = "Success";
                }
                else
                {
                    if (found == true)
                    {
                        response = "Record already exist";
                    }
                    else
                    {
                        response = "Server Issue";
                    }
                }

                return Ok(new { message = response });
            }
            catch (Exception e)
            {
                return Ok(e);
            }

        }

        [HttpPost("deleteDesignation")]
        public IActionResult deleteDesignation(DesignationCreation obj)
        {
            try
            {
                DateTime curDate = DateTime.Today;

                DateTime curTime = DateTime.Now;

                var time = curTime.ToString("HH:mm");

                int rowAffected = 0;
                var response = "";

                cmd = "update public.designation set \"isDeleted\" = B'1', \"modifiedOn\" = '" + curDate + "', \"modifiedBy\" = " + obj.userID + " where \"designationID\" = " + obj.designationID + ";";

                if(obj.userID != 0 && obj.moduleId !=0)
                    {
                        saveConStr = _dapperQuery.FindMe(obj.userID,obj.moduleId);
                        using (NpgsqlConnection con = new NpgsqlConnection(saveConStr))
                        {
                        rowAffected = con.Execute(cmd);
                        }
                    }

                if (rowAffected > 0)
                {
                    response = "Success";
                }
                else
                {
                    response = "Server Issue";
                }

                return Ok(new { message = response });
            }
            catch (Exception e)
            {
                return Ok(e);
            }

        }

    }
}