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
    public class PartyController : ControllerBase
    {
        private readonly IOptions<conStr> _dbCon;
        // private dynamicString _dynamic;
        // private IDynamicString _dynamicString;
        private string cmd, cmd2;
        private string subconStr;
        
        public PartyController(IOptions<conStr> dbCon)
        {
            _dbCon = dbCon;
            // _dynamic = dynamic;
            // _dynamicString = dynamicString;
            // _subconStr = SubdbCon;
        }

        [HttpGet("getParty")]
        public IActionResult getParty(int businessID,int companyID,int userID)
        {
            try
            {
                if (businessID == 0 && companyID == 0)
                {
                    cmd = "SELECT * FROM view_party order by \"partyID\" desc";
                }
                else
                {
                    cmd = "SELECT * FROM view_party where \"businessid\" = " + businessID + " and \"companyid\" = " + companyID + " order by \"partyID\" desc";
                }
                subconStr = userCredentials.FindMe(userID);

                var appMenu = dapperQuery.StrConQry<Party>(cmd, subconStr);
                return Ok(appMenu);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [HttpGet("getAllParties")]
        public IActionResult getAllParties(int businessID,int companyID,int userID)
        {
            try
            {
                if (businessID == 0 && companyID == 0)
                {
                    cmd = "select * from public.party where \"isDeleted\"::int = 0 order by \"partyID\" desc";
                }
                else
                {
                    cmd = "select * from public.party where \"isDeleted\"::int = 0 AND \"businessid\" = " + businessID + " and \"companyid\" = " + companyID + "";
                }
                
                subconStr = userCredentials.FindMe(userID);
                var appMenu = dapperQuery.StrConQry<Party>(cmd, subconStr);
                return Ok(appMenu);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [HttpPost("saveParty")]
        public IActionResult saveParty(PartyCreation obj)
        {
            try
            {
                DateTime curDate = DateTime.Today;

                DateTime curTime = DateTime.Now;

                var time = curTime.ToString("HH:mm");

                int rowAffected = 0;
                var response = "";
                var found = false;
                var cnic = "";

                List<Party> appMenuParty = new List<Party>();
                cmd2 = "select cnic from party where \"isDeleted\"::int = 0 AND cnic = '" + obj.cnic + "' AND (\"type\" = 'supplier' OR \"type\" = 'customer')";
                subconStr = userCredentials.FindMe(obj.userID);
                appMenuParty = (List<Party>)dapperQuery.StrConQry<Party>(cmd2, subconStr);

                if (appMenuParty.Count > 0)
                    cnic = appMenuParty[0].cnic;

                if (obj.partyID == 0)
                {
                    if (cnic == "")
                    {
                        cmd = "insert into public.\"party\" (\"cityID\", \"partyName\", \"partyNameUrdu\", \"address\", \"addressUrdu\", \"phone\", \"mobile\", \"type\", \"description\", \"cnic\", \"focalperson\", \"createdOn\", \"createdBy\", \"isDeleted\",\"businessid\",\"companyid\") values ('" + obj.cityID + "', '" + obj.partyName + "', '" + obj.partyNameUrdu + "', '" + obj.address + "', '" + obj.addressUrdu + "', '" + obj.phone + "', '" + obj.mobile + "', '" + obj.type + "', '" + obj.description + "', '" + obj.cnic + "', '" + obj.focalPerson + "', '" + curDate + "', " + obj.userID + ", B'0'," + obj.businessid + "," + obj.companyid + ")";
                    }
                    else
                    {
                        found = true;
                    }
                }
                else
                {
                    cmd = "update public.\"party\" set \"cityID\" = '" + obj.cityID + "', \"partyName\" = '" + obj.partyName + "', \"partyNameUrdu\" = '" + obj.partyNameUrdu + "', \"address\" = '" + obj.address + "', \"addressUrdu\" = '" + obj.addressUrdu + "', \"phone\" = '" + obj.phone + "', \"mobile\" = '" + obj.mobile + "', \"type\" = '" + obj.type + "', \"description\" = '" + obj.description + "', \"cnic\" = '" + obj.cnic + "', \"focalperson\" = '" + obj.focalPerson + "', \"modifiedOn\" = '" + curDate + "', \"modifiedBy\" = " + obj.userID + " where \"partyID\" = " + obj.partyID + ";";
                }

                if (found == false)
                {
                    using (NpgsqlConnection con = new NpgsqlConnection(subconStr))
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

        [HttpPost("deleteParty")]
        public IActionResult deleteParty(PartyCreation obj)
        {
            try
            {
                DateTime curDate = DateTime.Today;

                DateTime curTime = DateTime.Now;

                var time = curTime.ToString("HH:mm");

                int rowAffected = 0;
                var response = "";

                cmd = "update public.\"party\" set \"isDeleted\" = B'1', \"modifiedOn\" = '" + curDate + "', \"modifiedBy\" = " + obj.userID + " where \"partyID\" = " + obj.partyID + ";";

                using (NpgsqlConnection con = new NpgsqlConnection(_dbCon.Value.dbCon))
                {
                    rowAffected = con.Execute(cmd);
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