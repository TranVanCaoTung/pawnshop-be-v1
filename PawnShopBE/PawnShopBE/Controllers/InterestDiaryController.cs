using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using Services.Services;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/interestDiary")]
    [ApiController]
    [Authorize]
    public class InterestDiaryController : ControllerBase
    {
        private readonly IInteresDiaryService _interestDiaryService;
        private readonly IMapper _mapper;

        public InterestDiaryController(IInteresDiaryService interestDiaryService, IMapper mapper) 
        { 
        _interestDiaryService=interestDiaryService;
            _mapper=mapper;
        }

        [HttpGet("getInterestDiariesByContractId{contractId}")]
        public async Task<IActionResult> GetInterestDiariesByContractId(int contractId)
        {
            var respone = await _interestDiaryService.GetInteresDiariesByContractId(contractId);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        //[HttpDelete("deleteInterestDiary/{id}")]
        //public async Task<IActionResult> DeleteInterestDiary(int id)
        //{
        //    var respone = await _interestDiaryService.DeleteInteresDiary(id);
        //    if (respone != null)
        //    {
        //        return Ok(respone);
        //    }
        //    return BadRequest();
        //}

        [HttpPut("updateInterestDiary/{id}")]
        public async Task<IActionResult> UpdateInterestDiary(int id, decimal paidMoney)
        {
            var respone = await _interestDiaryService.UpdateInterestDiary(id, paidMoney);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPut("uploadInterestImg/{id}")]
        public async Task<IActionResult> UploadInterestImg(int id, string interestImg)
        {
            var uploadInterest = await _interestDiaryService.UploadInterestDiaryImg(id, interestImg);
            if (uploadInterest)
                return Ok(uploadInterest);
            else
                return BadRequest(uploadInterest);
        }
    }
}
