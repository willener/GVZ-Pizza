using Custom.Core.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Custom.Database.Data;

/// <summary>
/// Partial extension class for entity framework entity.
/// </summary>
public partial class Pizza {

    /// <summary>
    /// Gets an instance by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Pizza? GetById(int? id) {
        if (id == null) return null;

        using var dataContext = DataEntities.GetNewInstance();
        return GetQueryById(dataContext, id.Value).FirstOrDefault();
    }

    /// <summary>
    /// Gets a query.
    /// </summary>
    /// <param name="dataContext"></param>
    /// <returns></returns>
    public static IQueryable<Pizza> GetQuery(DataEntities? dataContext) {
        ArgumentNullException.ThrowIfNull(dataContext);

        return
            from p in dataContext.Pizza
            select p;
    }

    /// <summary>
    /// Gets a query by id.
    /// </summary>
    /// <param name="dataContext"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static IQueryable<Pizza> GetQueryById(DataEntities? dataContext, int id) {
        ArgumentNullException.ThrowIfNull(dataContext);

        return
            from p in dataContext.Pizza
            where p.Id == id
            select p;
    }

    /// <summary>
    /// Gets a query by the string containing the last name and the first name, separated by a whitespace.
    /// </summary>
    /// <param name="dataContext"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IQueryable<Pizza> GetQueryByDescription(DataEntities? dataContext, string? description) {
        ArgumentNullException.ThrowIfNull(dataContext);
        ArgumentNullException.ThrowIfNull(description);

        return
            from p in dataContext.Pizza
            where p.Description == description
            select p;
    }

    /// <summary>
    /// Gets an instance by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Pizza? GetById(DataEntities dataContext, int? id) {
        ArgumentNullException.ThrowIfNull(dataContext);
        if (id == null) return null;

        return GetQueryById(dataContext, id.Value).FirstOrDefault();
    }

    /// <summary>
    /// Gets all pizzas.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Pizza> GetAll() {
        using var dataContext = DataEntities.GetNewInstance();

        return
            from p in dataContext.Pizza
            select p;
    }

    /// <summary>
    /// Gets all pizzas.
    /// </summary>
    /// <param name="dataContext"></param>
    /// <returns></returns>
    public static IEnumerable<Pizza> GetAll(DataEntities? dataContext) {
        ArgumentNullException.ThrowIfNull(dataContext);
        return
            from p in dataContext.Pizza
            select p;
    }

    /// <summary>
    /// Saves a <see cref="WorkOrder"/> instance from API.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static GenericApiResultDto Save(GenericPostDataDto data) {
        var result = new GenericApiResultDto();

        var fieldDataArray = data.FieldDataArray;
        var description = ApiHelper.GetFieldData<string>(fieldDataArray, GlobalConstants.FIELD_NAME_PIZZA_DESCRIPTION);
        var diameter = ApiHelper.GetFieldData<int>(fieldDataArray, GlobalConstants.FIELD_NAME_PIZZA_DIAMETER);
        var bakingTime = ApiHelper.GetFieldData<double>(fieldDataArray, GlobalConstants.FIELD_NAME_PIZZA_BAKING_TIME);
        if(string.IsNullOrEmpty(description?.Value) || diameter == null)  return result;
        var pizza = new Pizza {
            Description = description.Value,
            Diameter = diameter.Value,
            BakingTime = bakingTime?.Value
        };
        var dataContext = DataEntities.GetNewInstance();
        var success = Save(dataContext,pizza, null);
        if(!success) {
            result.Success = false;
            result.DetailInfos.Add($"Database save failed.");
        }
        return result;
    }

    /// <summary>
    /// Saves a pizza with toppings.
    /// </summary>
    /// <param name="dataContext"></param>
    /// <param name="pizza"></param>
    /// <param name="toppings"></param>
    /// <returns></returns>
    public static bool Save(DataEntities? dataContext, Pizza? pizza, List<Topping>? toppings) {
        ArgumentNullException.ThrowIfNull(dataContext);
        if (pizza == null ||
            pizza?.Description == null ||
            pizza?.Diameter == null || 
            (toppings != null && toppings.Any(x => string.IsNullOrEmpty(x.Description) == true))
        ) return false;
        string? descriptionText = pizza.Description.Trim();

        bool success;
        var pizzaData = GetById(dataContext, pizza.Id);
        if (pizzaData == null) {
            // Create new pizza
            dataContext.Pizza.Add(pizzaData = new Pizza {
                Description = pizza.Description,
                Diameter = pizza.Diameter,
                BakingTime = pizza.BakingTime                
            });
            if (!dataContext.SaveChanges()) return false;
        }
        else {
            pizzaData.Description = pizza.Description;
            pizzaData.Diameter = pizza.Diameter;
            pizzaData.BakingTime = pizza.BakingTime;
        }

        var toppingsData = Data.Topping.GetByPizzaId(dataContext, pizza.Id);
        if (toppings?.Count() == 0 || toppingsData?.Count() == 0) return true;
        if ((toppings == null || toppings.Count == 0) && toppingsData != null) {
            toppingsData?.Select(item => Data.Topping.Delete(dataContext, item.Id));
        }
        else if(toppings != null){
            foreach (var topping in toppings) {
                if (toppingsData != null && 
                    toppingsData?.Where(x => x.Id == topping.Id).Any() == false) {
                    // Remove topping
                    dataContext.Topping.Remove(topping);
                    continue;
                }

                // Save the topping
                success = Data.Topping.Save(dataContext, topping);
                if (!success) return false;
            }
        }

        return dataContext.SaveChanges();
    }

    /// <summary>
    /// Deletes existing <see cref="Person"/> instances.
    /// </summary>
    /// <param name="id">The <see cref="ID"/> to delete.</param>
    /// <param name="deleteRelated">Deletes related entities if true.</param>
    /// <returns></returns>
    public static bool Delete(int? id) {
        if (id == null) return false;

        using var dataContext = DataEntities.GetNewInstance();
        var pizza = GetById(dataContext, id);
        if (pizza == null) return true;

        var toppings = Data.Topping.GetByPizzaId(dataContext, pizza.Id);
        if(toppings?.Count() > 0) {
            foreach (var topping in toppings) {
                // Remove item.
                dataContext.Topping.Remove(topping);
            }
        }

        dataContext.Pizza.Remove(pizza);
        return dataContext.SaveChanges();
    }

}