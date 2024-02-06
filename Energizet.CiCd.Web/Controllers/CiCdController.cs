using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Energizet.CiCd.Web.Controllers;

[Route("[controller]")]
public class CiCdController : Controller
{
	private readonly CiCd _ciCd;

	public CiCdController(CiCd ciCd)
	{
		_ciCd = ciCd;
	}

	// GET
	public async Task<IActionResult> Index()
	{
		try
		{
			var process = new Process();
			var ciCdScript = Path.Combine(Directory.GetCurrentDirectory(), "ci-cd.script.sh");
			process.StartInfo.FileName = "/bin/sh";
			process.StartInfo.Arguments = $"""
			                              -c "{ciCdScript} "{_ciCd.FrontPath}" "{_ciCd.BackPath}""
			                              """;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			process.Start();
			await process.WaitForExitAsync();

			return Ok(new
			{
				Output = await process.StandardOutput.ReadToEndAsync(),
				Error = await process.StandardError.ReadToEndAsync(),
			});
		}
		catch (Exception ex)
		{
			return BadRequest(ex.ToString());
		}
	}
}