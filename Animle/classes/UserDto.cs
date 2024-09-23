using FluentMigrator.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Animle.Controllers
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public ICollection<Quiz>? Quizzes { get; set; } = null;
    }
}