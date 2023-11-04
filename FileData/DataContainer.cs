using Domain.Models;

namespace FileData;

public class DataContainer
{
    // we read data from the file and load into these two collections (our "database" tables)
    public ICollection<User> Users { get; set; }
    public ICollection<Todo> Todos { get; set; }
    

}