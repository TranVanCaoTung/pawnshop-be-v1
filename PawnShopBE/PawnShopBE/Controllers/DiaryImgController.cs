using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using Services.Services;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/diaryImg")]
    [ApiController]
    [Authorize]
    public class DiaryImgController : ControllerBase
    {
        private readonly IDiaryImgService _diaryImgService;
        private readonly IMapper _mapper;

        public DiaryImgController(IDiaryImgService diaryImgService, IMapper mapper)
        {
            _diaryImgService = diaryImgService;
            _mapper = mapper;
        }

        [HttpGet("getAll/{interestDiaryId}")]
        public async Task<IActionResult> GetDiaryImgsByInterestDiary(int interestDiaryId)
        {
            var imgs = await _diaryImgService.GetDiariesImg(interestDiaryId);
            return (imgs != null)?  Ok(imgs) : BadRequest(imgs);
        }
    }
}
