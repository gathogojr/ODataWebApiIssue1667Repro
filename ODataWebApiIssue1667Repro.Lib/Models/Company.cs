using System.ComponentModel.DataAnnotations;

namespace Repro.Lib.Models
{
    public class Company
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
