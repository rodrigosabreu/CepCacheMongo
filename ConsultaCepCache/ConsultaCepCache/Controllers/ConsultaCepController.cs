using ConsultaCepCache.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConsultaCepCache.Controllers;

[ApiController]
[Route("[controller]")]
public class ConsultaCepController : ControllerBase
{
    private IConsultaCepService _consultaCepService;

    public ConsultaCepController(IConsultaCepService consultaCepService)
    {
        _consultaCepService = consultaCepService;
    }


    [HttpGet(Name = "ConsultaCep")]
    public async Task<IActionResult> ConsultaCep(string cep, string version)
    {
        var response = await _consultaCepService.GetCepAsync(cep, version);

        return Ok(response);
    }
}
