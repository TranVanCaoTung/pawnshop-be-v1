using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/interestDiary")]
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

        [HttpDelete("deleteInterestDiary/{id}")]
        public async Task<IActionResult> DeleteInterestDiary(int id)
        {
            var respone = await _interestDiaryService.DeleteInteresDiary(id);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPut("updateInterestDiary/{id}")]
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
