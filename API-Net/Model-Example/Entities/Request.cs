using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
  [Table("Request")]
  public class Request
  {
    [Key]
    public int Id { get; set; }
    [StringLength(255)]
    public required string Title { get; set; }
    [StringLength(255)]
    public required string Description { get; set; }
    [Column("datetime")]
    public DateTime SentDate { get; set; }
    public required int PriorityId { get; set; }
    public required int EmployeeIdSubmit { get; set; }
    public int? EmployeeIdHandling { get; set; }


    [ForeignKey("EmployeeIdSubmit")]
    [InverseProperty("RequestEmployeeSubmiters")]
    public virtual required Employee EmployeeSubmiter { get; set; }

    [ForeignKey("EmployeeIdHandling")]
    [InverseProperty("RequestEmployeeHandlers")]

    public virtual Employee? EmployeeHandler { get; set; }

    [ForeignKey("PriorityId")]
    [InverseProperty("Requests")]
    public virtual required Priority Priorities { get; set; }
  }
}