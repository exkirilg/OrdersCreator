using Domain.CustomExceptions;
using Domain.DTO;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

/// <summary>
/// Provides public methods for CRUD operations with Orders entities
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrdersServices _ordersServices;

    /// <summary>
    /// Provides controller instance with injected services
    /// </summary>
    /// <param name="ordersServices"></param>
    public OrdersController(IOrdersServices ordersServices)
    {
        _ordersServices = ordersServices;
    }

    /// <summary>
    /// Returns a list of all orders with specified filters
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetOrdersRequest request)
    {
        return Ok(await _ordersServices.Get(request));
    }

    /// <summary>
    /// Returns order with specified id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="400">There is no order with specified id</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            return Ok(await _ordersServices.GetById(id));
        }
        catch (NoEntityFoundByIdException ex)
        {
            ModelState.AddModelError(nameof(Order.Id), ex.Message);
            return ValidationProblem();
        }
        catch
        {
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Creates new order with specified properties
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="400">Request validation</response>
    [HttpPost]
    public async Task<IActionResult> Create(NewOrderRequest request)
    {
        try
        {
            await _ordersServices.Create(request);
            return Ok();
        }
        catch(OrderValidationException ex)
        {
            foreach (var validationResult in ex.ValidationResults)
            {
                ModelState.AddModelError(
                    string.Join(',', validationResult.MemberNames),
                    validationResult.ErrorMessage ?? string.Empty);
            }
            return ValidationProblem();
        }
        catch
        {
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Updates existing order with specified properties
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="400">Request validation, there is no order with specified id</response>
    [HttpPut]
    public async Task<IActionResult> Update(UpdateOrderRequest request)
    {
        try
        {
            await _ordersServices.Update(request);
            return Ok();
        }
        catch (NoEntityFoundByIdException ex)
        {
            ModelState.AddModelError(nameof(Order.Id), ex.Message);
            return ValidationProblem();
        }
        catch (OrderValidationException ex)
        {
            foreach (var validationResult in ex.ValidationResults)
            {
                ModelState.AddModelError(
                    string.Join(',', validationResult.MemberNames),
                    validationResult.ErrorMessage ?? string.Empty);
            }
            return ValidationProblem();
        }
        catch
        {
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Deletes specified order
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="400">There is no order with specified id</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _ordersServices.Delete(id);
            return Ok();
        }
        catch (NoEntityFoundByIdException ex)
        {
            ModelState.AddModelError(nameof(Order.Id), ex.Message);
            return ValidationProblem();
        }
        catch
        {
            return StatusCode(500);
        }
    }
}
