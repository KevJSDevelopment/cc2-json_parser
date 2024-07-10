using JSON_Parser;

internal partial class Program
{
    private static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("You must pass in the json file path");
            return;
        }

        string filePath = args[0];

        string dir = Directory.GetCurrentDirectory();

        using StreamReader streamReader = new StreamReader(dir + filePath);

        var json = streamReader.ReadToEnd();

        json = json.Replace("\n", "");

        var jsonObject = new OurJSONObject(json);
        if(jsonObject._jsonString == "{}"){
            Console.WriteLine("JSON is Valid - \n{}");
            return;
        }

        var valueDict = jsonObject.GetJSONObject();

        if(!valueDict.Any()) Console.WriteLine("Invalid JSON");
        else {
            Console.WriteLine("JSON is Valid - \n");
            foreach(var item in valueDict)
            {
                Console.WriteLine("Key: {0}, Value: {1}", item.Key.ToString(), item.Value?.ToString());
            }
        }
    }
}
