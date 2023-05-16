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
using Microsoft.AspNetCore.Authorization;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/contract")]
    [ApiController]
    [Authorize]
    public class ContractController : ControllerBase
    {
        private readonly IContractService _contractService;
        private readonly IContractAssetService _contractAssetService;
        private readonly IMapper _mapper;

        public ContractController(IContractService contractService, IContractAssetService contractAssetService, IMapper mapper)
        {
            _contractService = contractService;
            _contractAssetService = contractAssetService;
            _mapper = mapper;
        }
        [HttpGet("excel/{branchId}")]
        public async Task<IActionResult> exportFileExcel(int branchId)
        {
            await _contractService.exporteExcel(branchId);
            return Ok("Export File Excel Success");
        }
        [HttpGet("homepage/{branchId}")]
        public async Task<IActionResult> GetAllContractHomePage(int branchId)
        {
            var listContracts = await _contractService.getAllContractHomepage(branchId);
            return (listContracts == null) ? NotFound(listContracts) : Ok(listContracts);
        }

        [HttpPost("createContract")]
        public async Task<IActionResult> CreateContract(ContractDTO request)
        {

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
        [HttpGet("getAll/{numPage}/{branchId}")]
        public async Task<IActionResult> GetAllContracts(int numPage, int branchId)
        {
            var listContracts = await _contractService.GetAllDisplayContracts(numPage, branchId);
            return (listContracts == null) ? NotFound() : Ok(listContracts);
        }

        [HttpGet("getContractDetail/{idContract}")]
        public async Task<IActionResult> GetContractDetail(int idContract)
        {
            var contractDetail = await _contractService.GetContractDetail(idContract);
            return (contractDetail == null) ? NotFound() : Ok(contractDetail);
        }

        [HttpGet("getImgByContractId/{contractId}")]
        public async Task<IActionResult> GetContractByContractId(int contractId)
        {
            var contract = await _contractService.GetContractById(contractId);
            return (contract != null) ? Ok(contract) : NotFound();
        }

        [HttpGet("getContractInfoByContractId/{contractId}")]
        public async Task<IActionResult> GetContractInfoByContractId(int contractId)
        {
            var contract = await _contractService.GetContractInfoByContractId(contractId);
            return (contract != null) ? Ok(contract) : NotFound();
        }

        [HttpPut("uploadContractImg/{contractId}")]
        public async Task<IActionResult> UploadContractImg(int contractId, string? customerImg, string? contractImg)
        {

            var uploadContract = await _contractService.UploadContractImg(contractId, customerImg, contractImg);
            return (uploadContract) ? Ok(uploadContract) : BadRequest(uploadContract);
        }

        [HttpPost("createContractExpiration/{contractId}/{userId}")]
        public async Task<IActionResult> CreateContractExpiration(int contractId, string proofImg, Guid userId)
        {
            var contractExpiration = await _contractService.CreateContractExpiration(contractId, proofImg, userId);
             return (contractExpiration != null) ? Ok(contractExpiration) : BadRequest();
        }
    }
}
