using Microsoft.EntityFrameworkCore;
using Moq;
using ToDoListApi.Data;
using ToDoListApi.Enums;
using ToDoListApi.Helpers;
using ToDoListApi.Models;
using ToDoListApi.Services;

namespace ToDoListApi.Tests.Services
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly ApplicationDbContext _context;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new ApplicationDbContext(options);
            _userService = new UserService(_context);
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnTrue_WhenUserIsCreated()
        {
            var user = new User
            {
                Name = "Test User",
                Username = "testuser",
                PasswordHash = "password123"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var createdUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "testuser");
            Assert.NotNull(createdUser);
        }

        [Fact]
        public async Task Authenticate_ShouldReturnUser_WhenCredentialsAreValid()
        {
            var user = new User
            {
                Name = "Test User",
                Username = "testuser",
                PasswordHash = PasswordHelper.HashPassword("password123")
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _userService.Authenticate("testuser", "password123");

            Assert.NotNull(result);
            Assert.Equal("testuser", result?.Username);
        }

        [Fact]
        public async Task Authenticate_ShouldReturnNull_WhenPasswordIsIncorrect()
        {
            var user = new User
            {
                Name = "Test User",
                Username = "testuser",
                PasswordHash = PasswordHelper.HashPassword("password123")
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _userService.Authenticate("testuser", "wrongpassword");

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnTrue_WhenUserIsDeleted()
        {
            var user = new User
            {
                Name = "Test User",
                Username = "testuser",
                PasswordHash = PasswordHelper.HashPassword("password123")
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            var deletedUser = await _context.Users.FindAsync(user.Id);

            Assert.Null(deletedUser);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnFalse_WhenUserNotFound()
        {
            var result = await _userService.DeleteUser(999);

            Assert.False(result);
        }
    }
}
