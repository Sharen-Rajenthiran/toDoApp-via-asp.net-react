using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoAppV2.Domain.Entities
{
    public class ToDoItem
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Task cannot be empty.")]
        public string Task {  get; set; } = "Write something here";
        public bool IsCompleted { get; set; } = false;

    }
}
