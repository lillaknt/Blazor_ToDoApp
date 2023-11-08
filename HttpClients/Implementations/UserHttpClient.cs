using System.Net.Http.Json;
using System.Text.Json;
using Domain.DTOs;
using Domain.Models;
using HttpClients.ClientInterfaces;

namespace HttpClients.Implementations;

public class UserHttpClient : IUserService
{
    private readonly HttpClient client;

    public UserHttpClient(HttpClient client)
    {
        this.client = client;
    }

    public async Task<User> Create(UserCreationDto dto)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("/user", dto); //client makes post request to /users sending the dto
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) //If the response is not a success code, i.e. an error code in the 400 or 500 range, we know the result content is the error message, and an exception is thrown with that message.
        {
            throw new Exception(result);
        }

        User user = JsonSerializer.Deserialize<User>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true //We supply the JsonSerializer with options to ignore casing, because the result from the Web API will be camelCase, but our model classes use PascalCase for the propertie
        })!; //a null-suppressor: "!", i.e. the exclamation mark. This is because, the Deserialize method returns a nullable object, i.e. User?, but we just above checked if the request went well, so at this point we know there is a User to be deserialized.
        return user;
    }
    
    public async Task<IEnumerable<User>> GetUsersAsync(string? usernameContains = null)
    {
        string uri = "/user";
        if (!string.IsNullOrEmpty(usernameContains))
        {
            uri += $"?username={usernameContains}";
        }
        HttpResponseMessage response = await client.GetAsync(uri);
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }

        IEnumerable<User> users = JsonSerializer.Deserialize<IEnumerable<User>>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        return users;
    }
    
}