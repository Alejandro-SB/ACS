#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ACS.Core.Entities.Base
{
    public abstract class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public DateTime? CreationDate { get; set; }
        public string Creator { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Updater { get; set; }
    }
}