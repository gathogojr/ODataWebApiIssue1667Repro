using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repro.Lib.Models
{
    public class Milestone
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public ICollection<Task> Tasks { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
