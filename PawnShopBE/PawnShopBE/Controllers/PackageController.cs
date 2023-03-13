using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Services;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1")]
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


        [HttpGet("package/{id}")]
        public async Task<IActionResult> GetPackagehById(int packageId, int interestRecommend)
        {
            var package = await _packageService.GetPackageById(packageId, interestRecommend);

            if (package != null)
            {
                return Ok(package);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("packages/{numPage}")]
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
