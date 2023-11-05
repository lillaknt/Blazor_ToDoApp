using System.Text.Json;
using Domain.Models;

namespace FileData;

//responsible for reading and writing the data from and to the file
public class FileContext
{
    private const string filePath = "data.json";
    private DataContainer? dataContainer; //nullable because we will regularly reset the data, clear it out and reload it

    public ICollection<Todo> Todos
    {
        get
        {
            LoadData();
            return dataContainer!.Todos;
        }
    }

    public ICollection<User> Users
    {
        get
        {
            LoadData();
            return dataContainer!.Users;
        }
    }

    private void LoadData()
    {
        if (dataContainer != null ) return;
        if (!File.Exists(filePath))
        {
            dataContainer = new()
            {
                Todos = new List<Todo>(),
                Users = new List<User>()
            };
            return;
        }

        string content = File.ReadAllText(filePath);
        dataContainer = JsonSerializer.Deserialize<DataContainer>(content);
    }

    public void SaveChanges()
    {
        string serialized = JsonSerializer.Serialize(dataContainer, new JsonSerializerOptions
        {
WriteIndented = true
        });
        File.WriteAllText(filePath, serialized);
        dataContainer = null;
    }

}