using Newtonsoft.Json;

Ingredients ingredients = new Ingredients("ingredients.json");
CookBook cookBook = new CookBook(ingredients);
cookBook.Cook();
Console.ReadKey();

public class Ingredient
{
    public string Name { get; set; }
    public int Id { get; set; }
    public string Description { get; set; }
}


public class Ingredients
{
    public List<Ingredient> ingredients { get; }

    public Ingredients(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentNullException(nameof(fileName));
        }

        ingredients = new List<Ingredient>();
        LoadIngredientsFromFile(fileName);
    }

    private void LoadIngredientsFromFile(string fileName)
    {
        try
        {
            string jsonString = File.ReadAllText(fileName);
            var jsonText = JsonConvert.DeserializeObject<List<Ingredient>>(jsonString)
                           ?? throw new JsonSerializationException("Error deserializing JSON");
            foreach (var ingredient in jsonText)
            {
                if (IsValidIngredient(ingredient))
                {
                    ingredients.Add(ingredient);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static bool IsValidIngredient(Ingredient ingredient)
    {
        if (string.IsNullOrEmpty(ingredient.Name))
        {
            return false;
        }

        return true;
    }


    public void ShowAll()
    {
        foreach (var ingredient in ingredients)
        {
            Console.WriteLine($"{ingredient.Id} - {ingredient.Name}");
        }
    }
}

public class CookBook
{
    private readonly Ingredients _ingredients;
    private List<int> _chosenIngredients = new List<int>();

    public CookBook(Ingredients ingredients)
    {
        _ingredients = ingredients;
    }

    public void Cook()
    {
        WelcomeMsg();
        PrintIngredientInfo();
        Instruction();
        while (true)
        {

            var inputVal = Console.ReadLine();
            if (int.TryParse(inputVal, out int inputNumber))
            {
                bool exist = _ingredients.ingredients.Exists(i => i.Id == inputNumber);
                if (exist)
                {
                    _chosenIngredients.Add(inputNumber);
                    Instruction();
                }
            }
            else
            {
                if (_chosenIngredients.Count > 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("You have chosen the following ingredients:");
                }
            }
        }
        Console.WriteLine("Done");
    }

    public void WelcomeMsg()
    {
        Console.WriteLine("Welcome to the CookBook!");
    }

    public void PrintIngredientInfo()
    {
        _ingredients.ShowAll();
    }

    public void Instruction()
    {
        Console.WriteLine("Add an ingredient by it's id or type anything else if finish");
    }
}