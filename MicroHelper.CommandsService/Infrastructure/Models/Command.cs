using System.ComponentModel.DataAnnotations;

namespace MicroHelper.CommandsService.Infrastructure.Models
{
    public class Command
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int PlatformId { get; set; }

        [Required]
        public string HowTo { get; set; }

        public Platform Platform { get; set; }
        
        [Required]
        public string CommandLine { get; set; }
    }
}
