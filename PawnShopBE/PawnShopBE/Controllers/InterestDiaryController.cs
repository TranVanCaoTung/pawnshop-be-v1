using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class InterestDiaryController : ControllerBase
    {
        private readonly IInteresDiaryService _interestDiaryService;
        private readonly IMapper _mapper;

        public InterestDiaryController(IInteresDiaryService interestDiaryService, IMapper mapper) 
        { 
        _interestDiaryService=interestDiaryService;
            _mapper=mapper;
        }

        //[HttpGet("interestDiary{contractId}")]
        //public async Task<IActionResult> GetAllinterestDiary(int contractId) {
        //    var respone =await _interestDiaryService.GetInteresDiaryByContractId(contractId);
        //    if (respone != null)
        //    {
        //        return Ok(respone);
        //    }
        //    return BadRequest();
        //}

        //[HttpPost("interestDiary")]
        //public async Task<IActionResult> CreateinterestDiary(InterestDiaryDTO diary)
        //{
        //    var diaryMapper = _mapper.Map<InterestDiary>(diary);
        //    var respone = await _interestDiaryService.CreateInteresDiary(diaryMapper);
        //    if (respone != null)
        //    {
        //        return Ok(respone);
        //    }
        //    return BadRequest();
        //}

        [HttpDelete("interestDiary/{id}")]
        public async Task<IActionResult> DeleteInterestDiary(int id)
        {
            var respone = await _interestDiaryService.DeleteInteresDiary(id);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPut("interestDiary/{id}")]
        public async Task<IActionResult> UpdateInterestDiary(int id, InterestDiaryDTO diary)
        {
            var diaryUpdate=_mapper.Map<InterestDiary>(diary);
            diaryUpdate.InterestDiaryId = id;
            var respone = await _interestDiaryService.UpdateInteresDiary(diaryUpdate);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }
    }
}
