using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerFileInfoService.Entities
{
    [Table("UserNotificationPath")]
    public class UserNotificationPath
    {
        [Key]
        public int Id { get; set; }

        public string SystemName { get; set; }

        public string UserId { get; set; }

        public string Path { get; set; }

        public string Email { get; set; }
    }
}
