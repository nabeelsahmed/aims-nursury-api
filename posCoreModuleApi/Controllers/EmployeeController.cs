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
    public class EmployeeController : ControllerBase
    {
        private readonly IOptions<conStr> _dbCon;
        private readonly dapperQuery _dapperQuery;
        private string cmd, cmd2;
        public string saveConStr;

        public EmployeeController(dapperQuery dapperQuery,IOptions<conStr> dbCon)
        {
            _dbCon = dbCon;
            _dapperQuery = dapperQuery;
        }

        [HttpGet("getEmployee")]
        public IActionResult getEmployee(int branchID,int companyID,int userID, int moduleId)
        {
            try
            {
                if(companyID == 0 && branchID != 0){
                    cmd = "SELECT * FROM view_employee where \"branchID\" = " + branchID + " order by \"partyID\" desc";
                }else{
                    cmd = "SELECT * FROM view_employee where \"branchID\" = " + branchID + " and \"companyid\" = " + companyID + " order by \"partyID\" desc";
                }
                var appMenu = _dapperQuery.StrConQry<Party>(cmd,userID,moduleId);
                return Ok(appMenu);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [HttpPost("saveEmployee")]
        public IActionResult saveEmployee(EmployeeCreation obj)
        {
            try
            {
                DateTime curDate = DateTime.Today;

                DateTime curTime = DateTime.Now;

                var time = curTime.ToString("HH:mm");

                int rowAffected = 0;
                var response = "";
                var found = false;
                var employeeName = "";

                List<Party> appMenuEmployee = new List<Party>();
                cmd2 = "select cnic from party where \"isDeleted\"::int = 0 AND cnic = '" + obj.cnic + "' AND \"type\" = 'Employee'";
                appMenuEmployee = (List<Party>)_dapperQuery.StrConQry<Party>(cmd2, obj.userID,obj.moduleId);

                if (appMenuEmployee.Count > 0)
                    employeeName = appMenuEmployee[0].cnic;

                if (obj.partyID == 0)
                {
                    if (employeeName == "")
                    {
                        cmd = "insert into public.\"party\" (\"designationID\", \"cityID\", \"partyName\", \"partyNameUrdu\", \"address\", \"addressUrdu\", \"mobile\", \"type\", \"description\", \"cnic\", \"branchID\", \"createdOn\", \"createdBy\", \"isDeleted\",\"businessid\",\"companyid\",\"EmployeeNo\",\"nextOfKin\") values ('" + obj.designationID + "', '" + obj.cityID + "', '" + obj.partyName + "', '" + obj.partyNameUrdu + "', '" + obj.address + "', '" + obj.addressUrdu + "', '" + obj.mobile + "', '" + obj.type + "', '" + obj.description + "', '" + obj.cnic + "', '" + obj.branchID + "', '" + curDate + "', " + obj.userID + ", B'0'," + obj.businessid + "," + obj.companyid + "," + obj.EmployeeNo + ",'" + obj.nextOfKin + "')";
                    }
                    else
                    {
                        found = true;
                    }
                }
                else
                {
                    cmd = "update public.\"party\" set \"designationID\" = '" + obj.designationID + "', \"cityID\" = '" + obj.cityID + "', \"partyName\" = '" + obj.partyName + "', \"partyNameUrdu\" = '" + obj.partyNameUrdu + "', \"address\" = '" + obj.address + "', \"addressUrdu\" = '" + obj.addressUrdu + "', \"mobile\" = '" + obj.mobile + "', \"type\" = '" + obj.type + "', \"description\" = '" + obj.description + "', \"cnic\" = '" + obj.cnic + "', \"branchID\" = '" + obj.branchID + "', \"modifiedOn\" = '" + curDate + "', \"modifiedBy\" = " + obj.userID + ", \"EmployeeNo\" = " + obj.EmployeeNo + ", \"nextOfKin\" = '" + obj.nextOfKin + "' where \"partyID\" = " + obj.partyID + ";";
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
                        response = "CNIC already exist";
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

    }
}