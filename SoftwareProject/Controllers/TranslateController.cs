using Google.Api.Gax.ResourceNames;
using Google.Cloud.Translate.V3;
using Microsoft.AspNetCore.Mvc;
using SoftwareProject.Client.Data;


/// <summary>
/// API controller to translate text.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TranslateController : ControllerBase
{
    
    /// <summary>
    /// Uses Google Translate API to translate text.
    /// </summary>
    /// <param name="bodyRequest">Translation request. Stores text to be translated as well as language code.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Translate([FromBody] TranslateRequest bodyRequest)
    {
        var client = await TranslationServiceClient.CreateAsync();
        var request = new TranslateTextRequest
        {
            Contents = { bodyRequest.Text },
            TargetLanguageCode = bodyRequest.Language,
            Parent = new ProjectName("pure-league-457823-f7").ToString()
        };
        var response = await client.TranslateTextAsync(request);
        return Ok(response.Translations.FirstOrDefault()?.TranslatedText);
    }
}

