using Custom.Database.Data;

namespace Test.Custom.Core.Database;

/// <summary>
/// Test the database functions.
/// </summary>
[TestClass]
public class TestPizzaCustom {
    [TestInitialize]
    public void Initialize() {
        var dataContext = DataEntities.GetNewInstance();
        // Pizza
        var success = Pizza.Save(dataContext, new Pizza() { Description = "Test Pizza", Diameter = 30, BakingTime = 16.4 }, null);
        Assert.IsTrue(success, "Pizza is not saved");
        // Topping
        var pizzaId = Pizza.GetAll(dataContext).LastOrDefault()?.Id;
        Assert.IsTrue(pizzaId != null, "Pizza is no available");
        success = Topping.Save(dataContext, new Topping() { Description = "Olives", FK_Pizza = pizzaId!.Value });
        Assert.IsTrue(success, "Topping is not saved");
        success = Topping.Save(dataContext, new Topping() { Description = "Pineapple", FK_Pizza = pizzaId!.Value });
        Assert.IsTrue(success, "Topping is not saved");
    }

    [TestCleanup]
    public void Cleanup() {
        var dataContext = DataEntities.GetNewInstance();
        var pizzas = Pizza.GetAll(dataContext);
        foreach(var pizza in pizzas) {
            Pizza.Delete(pizza.Id);
        }
    }

    [TestMethod]
    public void TestGetById() {
        var dataContext = DataEntities.GetNewInstance();
        var pizzaId = Pizza.GetAll(dataContext).LastOrDefault()?.Id;
        var pizza = Pizza.GetById(dataContext, pizzaId);
        Assert.IsTrue(pizza != null, "Pizza is not available");
        Assert.IsTrue(pizzaId != null, "Pizza is not available");

        var toppings = Topping.GetByPizzaId(dataContext, pizzaId);
        Assert.IsTrue(toppings != null, "Topping is not available");
    }

    [TestMethod]
    public void TestDelete() {
        var dataContext = DataEntities.GetNewInstance();
        // Topping
        var topping = Topping.GetAll(dataContext)?.FirstOrDefault();
        var pizzaId = topping?.FK_Pizza;
        Assert.IsTrue(topping != null, "Topping is not available");
        var success = Topping.Delete(dataContext, topping.Id);
        Assert.IsTrue(success, "Topping is not deleted");

        topping = Topping.GetById(dataContext, topping.Id);
        Assert.IsTrue(topping == null, "Topping is not deleted");
        // Pizza
        success = Pizza.Delete(pizzaId);
        Assert.IsTrue(success, "Pizza is not deleted");
        var pizza = Pizza.GetById(pizzaId);
        Assert.IsTrue(pizza == null, "Pizza is not deleted");
        _ = Topping.GetByPizzaId(dataContext, pizzaId);
    }
}