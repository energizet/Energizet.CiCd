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
		Console.WriteLine(Request.Path);
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

			var res = new
			{
				Output = await process.StandardOutput.ReadToEndAsync(),
				Error = await process.StandardError.ReadToEndAsync(),
			};

			var foregroundColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(res.Error);
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(res.Output);
			Console.ForegroundColor = foregroundColor;

			return Ok(res);
		}
		catch (Exception ex)
		{
			return BadRequest(ex.ToString());
		}
		finally
		{
			Console.WriteLine($"{Request.Path} end");
		}
	}
}