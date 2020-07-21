using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repro.Lib.Models
{
    public class Employee
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		[ForeignKey("Manager")]
		public int? ManagerId { get; set; }
		public Employee Manager { get; set; }
		public Collection<Employee> Reports { get; set; }
		public Collection<Task> Tasks { get; set; }
	}
}
