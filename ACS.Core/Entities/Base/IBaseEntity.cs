using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ACS.Core.Entities.Base
{
    public interface IBaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        int Id { get; set; }
        DateTime CreationDate { get; set; }
        string? Creator { get; set; }
        DateTime UpdateDate { get; set; }
        string? Updater { get; set; }
    }
}