using Microsoft.AspNetCore.Mvc;

namespace GenericLicensing.WebApi.Controllers;

[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/[controller]")]
[ApiController]
public class LicenseController : ControllerBase
{
}