using Domain.CustomExceptions;
using Domain.DTO.Requests;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

/// <summary>
/// Provides public methods for CRUD operations with Providers entities
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ProvidersController : ControllerBase
{
    private readonly ProvidersServices _providersServices;

    /// <summary>
    /// Provides controller instance with injected services
    /// </summary>
    /// <param name="providersServices"></param>
    public ProvidersController(ProvidersServices providersServices)
    {
        _providersServices = providersServices;
    }

    /// <summary>
    /// Returns a list of all providers
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetProvidersRequest request)
    {
        return Ok(await _providersServices.Get(request));
    }

    /// <summary>
    /// Returns provider with specified id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="400">There is no provider with specified id</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            return Ok(await _providersServices.GetById(id));
        }
        catch (NoEntityFoundByIdException ex)
        {
            ModelState.AddModelError("Id", ex.Message);
            return ValidationProblem();
        }
    }

    /// <summary>
    /// Creates new provider with specified properties
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="400">Request validation</response>
    [HttpPost]
    public async Task<IActionResult> Create(NewProviderRequest request)
    {
        await _providersServices.Create(request);
        return Ok();
    }

    /// <summary>
    /// Updates existing provider with specified properties
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="400">Request validation, there is no provider with specified id</response>
    [HttpPut]
    public async Task<IActionResult> Update(UpdateProviderRequest request)
    {
        try
        {
            await _providersServices.Update(request);
            return Ok();
        }
        catch (NoEntityFoundByIdException ex)
        {
            ModelState.AddModelError("Id", ex.Message);
            return ValidationProblem();
        }
    }

    /// <summary>
    /// Deletes specified provider
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="400">There is no provider with specified id</response>
    /// <response code="409">Provider with specified id cannot be deleted</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _providersServices.Delete(id);
            return Ok();
        }
        catch (NoEntityFoundByIdException ex)
        {
            ModelState.AddModelError("Id", ex.Message);
            return ValidationProblem();
        }
        catch
        {
            ModelState.AddModelError("Provider", $"Provider with id {id} cannot be deleted");
            return Conflict(ModelState);
        }
    }
}
