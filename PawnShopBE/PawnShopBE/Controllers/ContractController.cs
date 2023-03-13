using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using PawnShopBE.Core.Const;
using PawnShopBE.Core.Requests;
using Services.Services;
using Services.Services.IServices;
using System.Text;
using PawnShopBE.Core.Validation;

namespace PawnShopBE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContractService _contractService;
        private readonly ICustomerService _customerService;
        private readonly IContractAssetService _contractAssetService;
        private readonly IPackageService _packageService;
        private readonly IInteresDiaryService _interesDiaryService;
        private readonly IRansomService _ransomService;

        private readonly IMapper _mapper;

        public ContractController(
            IContractService contractService, 
            ICustomerService customer, 
            IContractAssetService contractAssetService, 
            IPackageService packageService,
            IInteresDiaryService interestDiaryService,
            IRansomService ransomService,
            IMapper mapper)
        {
            _contractService = contractService;
            _customerService = customer;
            _contractAssetService = contractAssetService;
            _packageService = packageService;
            _interesDiaryService = interestDiaryService;
            _ransomService = ransomService;
            _mapper = mapper;
        }
        private Validation<ContractDTO> _validation;
       
    [HttpPost("contract")]
        public async Task<IActionResult> CreateContract(ContractDTO request)
        {
            //Check Validation
            //var checkValidation = await _validation.CheckValidation(request);
            //if (checkValidation != null)
            //{
            //    return BadRequest(checkValidation);
            //}
            StringBuilder sb = new StringBuilder();
            foreach (AttributeDTO attributes in request.PawnableAttributeDTOs)
            {            
                sb.Append(attributes.Description + "/");              
            }       
            //Get package
            Package package = await _packageService.GetPackageById(request.PackageId, request.InterestRecommend);
            //Get customer 
            var customer = await _customerService.getCustomerByCCCD(request.CCCD); 
            
            //Create asset
            var contractAsset = _mapper.Map<ContractAsset>(request);
                contractAsset.Description = sb.ToString();
            await _contractAssetService.CreateContractAsset(contractAsset);
            
            // Create contract
            var contract = _mapper.Map<Contract>(request);
            contract.ContractAssetId = contractAsset.ContractAssetId;
            contract.CustomerId = customer.CustomerId;
            var contractCreated = await _contractService.CreateContract(contract);
            var ransomSuccess = await _ransomService.CreateRansom(contractCreated);
            var interestDiarySuccess = await _interesDiaryService.CreateInterestDiary(contractCreated);
            
            if (interestDiarySuccess == true)
            {
                return Ok(interestDiarySuccess); 
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("contracts/{numPage}")]
        public async Task<IActionResult> GetAllContracts(int numPage)
        {
            var listContracts = await _contractService.GetAllContracts(numPage);
            if (listContracts == null)
            {
                return NotFound();
            }
            return Ok(listContracts);
        }

        [HttpPut("contract")]
        public async Task<IActionResult> UpdateContract(ContractDTO request)
        {       
                var contract = _mapper.Map<Contract>(request);
                var response = await _contractService.UpdateContract(contract);
                if (response)
                {
                    return Ok(response);
                }         
            return BadRequest();
        }
    }
}
