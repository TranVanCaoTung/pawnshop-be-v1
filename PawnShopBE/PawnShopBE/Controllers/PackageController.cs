using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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


        [HttpGet("getPackageById/{id}")]
        public async Task<IActionResult> GetPackageById(int packageId)
        {
            var package = await _packageService.GetPackageById(packageId);

            if (package != null)
            {
                return Ok(package);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("getAll/{numPage}")]
        public async Task<IActionResult> GetAllPackages(int numPage)
        {
            var packageList = await _packageService.GetAllPackages(numPage);
            if (packageList == null)
            {
                return NotFound();
            }
            return Ok(packageList);
        }
    }
}
