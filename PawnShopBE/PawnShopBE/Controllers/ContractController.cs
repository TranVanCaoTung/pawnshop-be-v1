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
using System.Diagnostics.Contracts;
using Contract = PawnShopBE.Core.Models.Contract;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/contract")]
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
        private Validation<ContractDTO> _validation = new Validation<ContractDTO>();
        [HttpGet("excel")]
        public async Task<IActionResult> exportFileExcel()
        {
            await _contractService.exporteExcel();
            return Ok("Export File Excel Success");
        }
        [HttpGet("homepage/{branchId}")]
        public async Task<IActionResult> GetAllContractHomePage(int branchId)
        {
            var listContracts = await _contractService.getAllContractHomepage(branchId);
            if (listContracts == null)
            {
                return NotFound();
            }
            return Ok(listContracts);
        }

        [HttpPost("createContract")]
        public async Task<IActionResult> CreateContract(ContractDTO request)
        {
            //Check Validation
            var checkValidation = await _validation.CheckValidation(request);
            if (checkValidation != null)
            {
                return BadRequest(checkValidation);
            }
            StringBuilder sb = new StringBuilder();
            var count = 1;
            foreach (AttributeDTO attributes in request.PawnableAttributeDTOs)
            {
                if (request.PawnableAttributeDTOs.Count > count)
                {
                    sb.Append(attributes.Description + "/");
                    count++;
                }
                else
                {
                    sb.Append(attributes.Description);
                }
            }
            //Create asset
            var contractAsset = _mapper.Map<ContractAsset>(request);
            contractAsset.Description = sb.ToString();
            contractAsset.Status = (int)ContractAssetConst.IN_STOCK;
            await _contractAssetService.CreateContractAsset(contractAsset);

            // Create contract
            var contract = _mapper.Map<Contract>(request);
            contract.ContractAssetId = contractAsset.ContractAssetId;
            var result = await _contractService.CreateContract(contract);
            return result ? Ok(result) : BadRequest();
        }
        [HttpGet("getAll/{numPage}")]
        public async Task<IActionResult> GetAllContracts(int numPage)
        {
            var listContracts = await _contractService.GetAllDisplayContracts(numPage);
            if (listContracts == null)
            {
                return NotFound();
            }
            return Ok(listContracts);
        }

        //[HttpPut("updateContract/{contractId}")]
        //public async Task<IActionResult> UpdateContract(int contractId, ContractDTO request)
        //{       
        //        var contract = _mapper.Map<Contract>(request);
        //        var response = await _contractService.UpdateContract(contractId, contract);
        //        if (response)
        //        {
        //            return Ok(response);
        //        }         
        //    return Ok();
        //}

        [HttpGet("getContractDetail/{idContract}")]
        public async Task<IActionResult> GetContractDetail(int idContract)
        {
            var contractDetail = await _contractService.GetContractDetail(idContract);
            if (contractDetail == null)
            {
                return NotFound();
            }
            return Ok(contractDetail);
        }

        //[HttpGet("getByContractId/{contractId}")]
        //public async Task<IActionResult> GetContractByContractId(int contractId)
        //{
        //    var contract = await _contractService.GetContractById(contractId);
        //    return (contract != null) ? Ok(contract) : NotFound();
        //}

        [HttpGet("getContractInfoByContractId/{contractId}")]
        public async Task<IActionResult> GetContractInfoByContractId(int contractId)
        {
            var contract = await _contractService.GetContractInfoByContractId(contractId);
            return (contract != null) ? Ok(contract) : NotFound();
        }

        [HttpPut("uploadContractImg/{contractId}")]
        public async Task<IActionResult> UploadContractImg(int contractId, string customerImg, string contractImg)
        {

            var uploadContract = await _contractService.UploadContractImg(contractId, customerImg, contractImg);
            if (uploadContract)
                return Ok(uploadContract);
            else
                return BadRequest(uploadContract);
        }

        [HttpPost("createContractExpiration/{contractId}")]
        public async Task<IActionResult> CreateContractExpiration(int contractId)
        {
            var contractExpiration = await _contractService.CreateContractExpiration(contractId);
            if (contractExpiration != null)
            {
                return Ok(contractExpiration);
            }
            return BadRequest();
        }
    }
}
