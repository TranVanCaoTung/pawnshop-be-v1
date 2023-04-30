using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using Services.Services;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/package")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;
        private readonly IMapper _mapper;
        public PackageController(IPackageService packageService, IMapper mapper)
        {
            _packageService = packageService;
            _mapper = mapper;
        }


        [HttpGet("getPackageById/{packageId}")]
        public async Task<IActionResult> GetPackageById(int packageId)
        {
            var package = await _packageService.GetPackageById(packageId);

            return (package != null) ? Ok(package) : NotFound(package);
                
        }

        [HttpGet("getAll/{numPage}")]
        public async Task<IActionResult> GetAllPackages( int numPage)
        {
            var packageList = await _packageService.GetAllPackages(numPage);
            if (packageList == null)
            {
                return NotFound(packageList);
            }
            return Ok(packageList);
        }

        [HttpPut("updatePackage")]
        public async Task<IActionResult> UpdatePackage(Package package)
        {
            var updatePackage = await _packageService.UpdatePackage(package);
            if (updatePackage)
            {
                return Ok(updatePackage);
            }
            return BadRequest(updatePackage);
        }

        [HttpPost("createPackage")]
        public async Task<IActionResult> CreatePackage(Package package)
        {
            var result = await _packageService.CreatePackage(package);
            return (result) ? Ok(result) : BadRequest(result);
        }
    }
}
