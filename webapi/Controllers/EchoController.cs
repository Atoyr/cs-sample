using Microsoft.AspNetCore.Mvc;

namespace Medoz.WebApi.Controllers;

public class EchoController : ControllerBase
{
    /// <summary>
    /// GET
    /// </summary>
    /// <returns>実行結果</returns>
    [HttpGet()]
    [Route("echo")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(500)]
    public ActionResult<string> Get()
    {
        return Ok();
    }

    /// <summary>
    /// POST
    /// </summary>
    /// <returns>実行結果</returns>
    [HttpPost()]
    [Route("echo")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(500)]
    public ActionResult<string> Post(object obj)
    {
        return Ok(obj);
    }
}
