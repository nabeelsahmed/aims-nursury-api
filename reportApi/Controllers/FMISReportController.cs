using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using reportApi.Services;
using Microsoft.Extensions.Options;
using reportApi.Configuration;
using reportApi.Entities;
using Dapper;
using System.Data;
using Npgsql;
using System.Collections.Generic;

namespace reportApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class FMISReportController : ControllerBase
    {
        private readonly IOptions<conStr> _dbCon;
        private string cmd, cmd2;

        public FMISReportController(IOptions<conStr> dbCon)
        {
            _dbCon = dbCon;
        }
        
        [HttpGet("getLedgerReport")]
        public IActionResult getLedgerReport(int coaID, string fromDate, string toDate)
        {
            try
            {
                cmd = "select * from public.ledgerreport  where coaid = '" + coaID + "' and invoicedate BETWEEN '" + fromDate + "' AND '" + toDate + "'";

                var appMenu = dapperQuery.Qry<Ledger>(cmd, _dbCon);
                return Ok(appMenu);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [HttpGet("getPartyLedgerReport")]
        public IActionResult getPartyLedgerReport(int partyID, string fromDate, string toDate)
        {
            try
            {
                cmd = "select * from public.partyledgerview  where partyledgerview.partyid = '" + partyID + "' and invoicedate BETWEEN '" + fromDate + "' AND '" + toDate + "'";

                var appMenu = dapperQuery.Qry<PartyLedger>(cmd, _dbCon);
                return Ok(appMenu);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [HttpGet("getPresentStock")]
        public IActionResult getPresentStock(int partyID, string fromDate, string toDate)
        {
            try
            {
                cmd = "select * from public.partyledgerview  where partyledgerview.partyid = '" + partyID + "' and invoicedate BETWEEN '" + fromDate + "' AND '" + toDate + "'";

                var appMenu = dapperQuery.Qry<PartyLedger>(cmd, _dbCon);
                return Ok(appMenu);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }
        
        [HttpGet("getDailyCategorySale")]
        public IActionResult getDailyCategorySale(int partyID, string fromDate, string toDate)
        {
            try
            {
                cmd = "select * from public.partyledgerview  where partyledgerview.partyid = '" + partyID + "' and invoicedate BETWEEN '" + fromDate + "' AND '" + toDate + "'";

                var appMenu = dapperQuery.Qry<PartyLedger>(cmd, _dbCon);
                return Ok(appMenu);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }
        [HttpGet("getDailySales")]
        public IActionResult getDailySales(string startDate,string endDate)
        {
            try
            {
                cmd = "SELECT * FROM public.\"view_dailySales\" WHERE \"view_dailySales\".\"invoiceDate\" BETWEEN '" + startDate + "' AND '" + endDate + "'";

                var appMenu = dapperQuery.Qry<DailySales>(cmd, _dbCon);
                return Ok(appMenu);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }
        [HttpGet("getDailySalesByOrder")]
        public IActionResult getDailySalesByOrder(string invoiceDate)
        {
            try
            {
                cmd = "SELECT * FROM public.\"view_dailySalesbyOrder\" WHERE \"view_dailySalesbyOrder\".\"invoiceDate\" = '" + invoiceDate + "'";

                var appMenu = dapperQuery.Qry<DailySalesByOrder>(cmd, _dbCon);
                return Ok(appMenu);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }
        [HttpGet("getStockInStockOut")]
        public IActionResult getStockInStockOut(string invDate)
        {
            try
            {
                cmd = "select * from public.\"productStockInStockOut\"('"+invDate+"')";

                var appMenu = dapperQuery.Qry<StockInStockOut>(cmd, _dbCon);
                return Ok(appMenu);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }
        
        [HttpGet("getPeriodicSale")]
        public IActionResult getPeriodicSale(int partyID, string fromDate, string toDate)
        {
            try
            {
                cmd = "select * from public.partyledgerview  where partyledgerview.partyid = '" + partyID + "' and invoicedate BETWEEN '" + fromDate + "' AND '" + toDate + "'";

                var appMenu = dapperQuery.Qry<PartyLedger>(cmd, _dbCon);
                return Ok(appMenu);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }
        
        [HttpGet("getPeriodicCategorySale")]
        public IActionResult getPeriodicCategorySale(int partyID, string fromDate, string toDate)
        {
            try
            {
                cmd = "select * from public.partyledgerview  where partyledgerview.partyid = '" + partyID + "' and invoicedate BETWEEN '" + fromDate + "' AND '" + toDate + "'";

                var appMenu = dapperQuery.Qry<PartyLedger>(cmd, _dbCon);
                return Ok(appMenu);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }
    }
}