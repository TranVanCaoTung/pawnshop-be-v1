
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/money")]
    [ApiController]
    [Authorize]
    public class MoneyController : ControllerBase
    {
        private readonly IMoneyService _moneyService;
        private readonly IMapper _mapper;

        public MoneyController(IMoneyService moneyService, IMapper mapper)
        {
            _moneyService = moneyService;
            _mapper = mapper;
        }

        [HttpGet("getAll/{num},{branchId}")]
        public async Task<IActionResult> GetAll(int num,int branchId)
        {
            var listMoney = await _moneyService.GetAllMoney(num,branchId);
            if (listMoney == null)
                return BadRequest();
            return Ok(listMoney);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateMoney( MoneyDTO moneyDTO)
        {
            if (moneyDTO != null)
            {
                var money = _mapper.Map<Money>(moneyDTO);
                var respone = await _moneyService.CreateMoney(money);
                if (respone != null)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

    }
}
