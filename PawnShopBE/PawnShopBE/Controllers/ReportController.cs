using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.Data.DescriptionAttribute;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Core.Validation;
using Services.Services.IServices;
using System.ComponentModel.DataAnnotations;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _report;
        private readonly IMapper _mapper;
       

        public ReportController(IReportService report, IMapper mapper) 
        { 
        _report= report;
            _mapper=mapper;
        }
        [HttpGet("getAll/transaction/{numPage}")]
        public async Task<IActionResult> GetAllReportTransaction(int numPage) {
            var respone =await _report.getReportTransaction(numPage);
          
            if (respone != null)
            {
                return Ok(respone) ;
            }
            return BadRequest();
        }

        [HttpGet("report/month")]
        public async Task<IActionResult> GetAllReportMonth()
        {
            var respone = await _report.getReportMonth();

            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

    }
}
