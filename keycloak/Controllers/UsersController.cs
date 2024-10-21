using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace keycloak.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpGet("test-auth")]
    public IActionResult AuthTesting()
    {
        return Ok("you are authen ticate");
    }
    private string filePath = "users.json";

    // Get all pending users
    [HttpGet("pending")]
    [Authorize]
    public IActionResult GetPendingUsers2()
    {
        var users = GetUsersFromFile();
        var pendingUsers = users.Where(u => u.Status == "pending").ToList();
        return Ok(pendingUsers);
    }

    [HttpPost("approve/{id}")]
    [Authorize(Roles = "approve-user")]
    public IActionResult ApproveUser(int id)
    {
        var users = GetUsersFromFile();

        var user = users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound(new { Message = "User not found" });
        }

        user.Status = "approved";
        SaveUsersToFile(users);

        return Ok(new { Message = "User approved successfully"});
        //return Ok(new { Message = "User approved successfully", User = user });
    }

    [HttpPost("reject/{id}")]
    [Authorize(Roles = "reject-user")]
    public IActionResult RejectUser(int id)
    {
        var users = GetUsersFromFile();

        var user = users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound(new { Message = "User not found" });
        }

        user.Status = "rejected";
        SaveUsersToFile(users);

        return Ok(new { Message = "User rejected successfully"});
    }

    // Get all approved users
    [HttpGet("approved")]
    [Authorize(Roles = "approve-user-list")]
    public IActionResult GetApprovedUsers()
    {
        var users = GetUsersFromFile();
        var approvedUsers = users.Where(u => u.Status == "approved").ToList();
        return Ok(approvedUsers);
    }

    // Get all rejected users
    [HttpGet("rejected")]
    [Authorize(Roles = "reject-user-lis")]
    public IActionResult GetRejectedUsers()
    {
        var users = GetUsersFromFile();
        var rejectedUsers = users.Where(u => u.Status == "rejected").ToList();
        return Ok(rejectedUsers);
    }

    // Method to read users from JSON file
    private List<User> GetUsersFromFile()
    {
        if (!System.IO.File.Exists(filePath))
        {
            return new List<User>();
        }

        var json = System.IO.File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<User>>(json);
    }

    private void SaveUsersToFile(List<User> users)
    {
        var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        System.IO.File.WriteAllText(filePath, json);
    }

    //[Authorize(Roles = "admin,user")]
    //[Authorize(Policy = "MultiRolePolicy")]
}
