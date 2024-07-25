using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Custom.Core.Api;
using Custom.Database.Data;

namespace WebApplication1.Controllers;

/// <summary>
/// Api Controll to manage Pizzas and Toppings.
/// </summary>
[ApiController]
[Route("[controller]")]
public class GenericController : ControllerBase {

    /// <summary>
    /// The delay time of the dummy task.
    /// </summary>
    private static readonly TimeSpan DUMMYTASK_DELAY = TimeSpan.FromTicks(1);

    /// <summary>
    /// Gets summary meta data of all supported entities.
    /// </summary>
    /// <returns></returns>
    [HttpGet("pizzas")]
    public async Task<ActionResult<object>> ReadAll() {
        try {
            await Task.Delay(DUMMYTASK_DELAY).ConfigureAwait(false);
            GenericApiResultDto result = new();
            using var dataContext = DataEntities.GetNewInstance();

            var allPizzas = Pizza.GetAll(dataContext);
            result.DetailInfos = allPizzas != null ? allPizzas.Select(x=> x.Description).ToList() : [];
            result.Result = allPizzas != null ?  allPizzas.Count() : 0;
            result.Success = true;
            return Ok(result);
        }
        catch(SqlException) {
            return BadRequest($"ReadAll Pizzas is not available.");
        }
    }

    /// <summary>
    /// Gets data of a single entity object.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("pizzas/{id}")]
    public async Task<ActionResult<object>> Read(int id) {
        try {
            await Task.Delay(DUMMYTASK_DELAY).ConfigureAwait(false);
            GenericApiResultDto result = new();
            using var dataContext = DataEntities.GetNewInstance();

            var pizza = Pizza.GetById(dataContext, id);
            if (pizza == null) return BadRequest($"Put Pizza with '{id}' is not available.");
            result.DetailInfos = [pizza.Description];
            result.Result = 1;
            result.Success = true;
            return Ok(result);
        }
        catch (SqlException) {
            return BadRequest($"Read Pizza '{id}' is not available.");
        }
    }

    /// <summary>
    /// Gets data of a single entity object.
    /// </summary>
    /// <param name="entityName"></param>
    /// <param name="id"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut("{entityName}/{id}")]
    public async Task<ActionResult<object>> Update(string entityName, string id, GenericPostDataDto data) {
        try {
            await Task.Delay(DUMMYTASK_DELAY).ConfigureAwait(false);                
            GenericApiResultDto result = new(); 

            if (entityName == GlobalConstants.ENTITY_NAME_PIZZA) {
                result = Pizza.Save(data); // Create a pizza
            }
            else if (entityName == GlobalConstants.ENTITY_NAME_TOPPING) {
                result = Topping.Save(data); // Create a Topping
            }
            else {
                return BadRequest($"Entity with name '{entityName}' is not valid.");
            }

            if (!result.Success) {
                return BadRequest($"Updata '{entityName}' with '{id}' is not available.");
            }

            result.Result = 1;
            result.SummaryInfo = "Pizza is successfully updated!";
            return Ok(result);
        }
        catch (SqlException) {
            return BadRequest($"Updata Pizza is not available.");
        }
    }

    /// <summary>
    /// Sets data of a single entity object.
    /// </summary>
    /// <param name="entityName"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost("{entityName}")]
    public async Task<ActionResult<GenericApiResultDto>> Create(string entityName, GenericPostDataDto data) {
        try {
                await Task.Delay(DUMMYTASK_DELAY).ConfigureAwait(false);
                GenericApiResultDto result = new();

                if (entityName == GlobalConstants.ENTITY_NAME_PIZZA) {
                    result = Pizza.Save(data); // Save or update a pizza
                }
                if(entityName == GlobalConstants.ENTITY_NAME_TOPPING) {
                    result = Topping.Save(data); // Save or Update a Topping
                }
                else {
                    return BadRequest($"Entity with name '{entityName}' is not valid.");
                }

                if (result.Success) {
                    result.Result = 0;
                    result.SummaryInfo = $"Create Pizza done.";
                    return Ok(result);
                }
                return BadRequest($"Pizza is not created.");
        }
        catch (SqlException) {
            return BadRequest($"Create Pizza is not available.");
        }
    }

    /// <summary>
    /// Deletes data of a single entity object.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("pizzas/{id}")]
    public async Task<ActionResult<GenericApiResultDto>> Delete(int? id) {
        try {
            await Task.Delay(DUMMYTASK_DELAY).ConfigureAwait(false);
            GenericApiResultDto result = new();

            var success = Pizza.Delete(id); // Remove a pizza with its toppings
            if (!success) {
                return BadRequest($"Delete Pizza with '{id}' is not available.");
            }

            result.Result = 1;
            result.Success = true;
            result.SummaryInfo = "Pizza is successfully removed!";
            return Ok(result);
        }
        catch (SqlException) {
            return BadRequest($"Delete Pizza is not available.");
        }
    }
}