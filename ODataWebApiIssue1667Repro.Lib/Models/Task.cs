using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repro.Lib.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        [ForeignKey("Milestone")]
        public int? MilestoneId { get; set; }
        public Milestone Milestone { get; set; }
        [ForeignKey("Supervisor")]
        public int? SupervisorId { get; set; }
        public Employee Supervisor { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
