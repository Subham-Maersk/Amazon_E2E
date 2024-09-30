// using System.IO;
// using System.Text.Json;

// namespace TestData
// {
//     public class LoginCredentials
//     {
//         public string Email { get; set; }
//         public string Password { get; set; }

//         public static LoginCredentials LoadFromJson(string filePath)
//         {
//             var json = File.ReadAllText(filePath);
//             return JsonSerializer.Deserialize<LoginCredentials>(json);
//         }
//     }
// }
