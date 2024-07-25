using Custom.Core.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Custom.Database.Data;

/// <summary>
/// Partial extension class for entity framework entity.
/// </summary>
public partial class Topping {

    /// <summary>
    /// Gets a query.
    /// </summary>
    /// <param name="dataContext"></param>
    /// <returns></returns>
    public static IQueryable<Topping>? GetQuery(DataEntities? dataContext) {
        ArgumentNullException.ThrowIfNull(dataContext);

        return
            from p in dataContext.Topping
            select p;
    }

    /// <summary>
    /// Gets a query by id.
    /// </summary>
    /// <param name="dataContext"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static IQueryable<Topping>? GetQueryById(DataEntities? dataContext, int? id) {
        ArgumentNullException.ThrowIfNull(dataContext);
        if (id == null) return null;

        return
            from p in dataContext.Topping
            where p.Id == id
            select p;
    }

    /// <summary>
    /// Gets all instances.
    /// </summary>
    /// <param name="dataContext"></param>
    /// <returns></returns>
    public static List<Topping>? GetAll(DataEntities dataContext) {
        ArgumentNullException.ThrowIfNull(dataContext);

        return GetQuery(dataContext)?.ToList();
    }
    /// <summary>
    /// Gets an instance by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Topping? GetById(DataEntities dataContext, int? id) {
        ArgumentNullException.ThrowIfNull(dataContext);
        if (id == null) return null;

        return GetQueryById(dataContext, id)?.FirstOrDefault();
    }

    /// <summary>
    /// Gets all toppings of a pizza.
    /// </summary>
    /// <param name="dataContext"></param>
    /// <param name="pizzaId"></param>
    /// <returns></returns>
    public static IEnumerable<Topping>? GetByPizzaId(DataEntities? dataContext, int? pizzaId) {
        ArgumentNullException.ThrowIfNull(dataContext);
        if (pizzaId == null) return null;

        return
            from t in dataContext.Topping
            where t.FK_Pizza == pizzaId
            select t;
    }

    /// <summary>
    /// Saves a <see cref="WorkOrder"/> instance from API.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static GenericApiResultDto Save(GenericPostDataDto data) {
        var result = new GenericApiResultDto();
        var fieldDataArray = data.FieldDataArray;
        using var dataContext = DataEntities.GetNewInstance();

        var description = ApiHelper.GetFieldData<string>(fieldDataArray, GlobalConstants.FIELD_NAME_TOPPING_DESCRIPTION);
        var pizzaId = ApiHelper.GetFieldData<int>(fieldDataArray, GlobalConstants.FIELD_NAME_TOPPING_PIZZA_ID);
        if (string.IsNullOrEmpty(description?.Value) || pizzaId == null) return result;
        var topping = new Topping {
            FK_Pizza = pizzaId.Value,
            Description = description.Value
        };
        
        var success = Save(dataContext, topping);
        if (!success) {
            result.Success = false;
            result.DetailInfos.Add($"Database save failed.");
        }
        return result;
    }

    /// <summary>
    /// Saves a topping.
    /// </summary>
    /// <param name="dataContext"></param>
    /// <param name="id"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    public static bool Save(DataEntities? dataContext, Topping? topping) {
        ArgumentNullException.ThrowIfNull(dataContext);
        if (topping == null) return false;
        var toppingData = GetById(dataContext, topping.Id);
        if (toppingData == null) {
            // Create new topping
            dataContext.Topping.Add(toppingData = new Topping {
                Description = topping.Description,
                FK_Pizza = topping.FK_Pizza
            });
            if (!dataContext.SaveChanges()) return false;
        }
        else {
            toppingData.Description = topping.Description;
        }

        return dataContext.SaveChanges();
    }

    /// <summary>
    /// Deletes existing <see cref="Topping"/> instances.
    /// </summary>
    /// <param name="dataContext"></param>
    /// <param name="id">The <see cref="ID"/> to delete.</param>
    /// <returns></returns>
    public static bool Delete(DataEntities dataContext, int? id) {
        ArgumentNullException.ThrowIfNull(dataContext);
        if (id == null) return false;
        var topping = GetById(dataContext, id);
        if (topping == null) return true;

        // Remove item.
        dataContext.Topping.Remove(topping);

        return dataContext.SaveChanges();
    }

}