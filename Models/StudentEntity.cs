using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchlManSysAPI.Models
{
    public class StudentEntity
    {
        [Key]
        public int StdId { get; set; }
        public string StdName { get; set; }
        public long StdMob { get; set; }
        public int FamId { get; set; }
        public int TutId { get; set; }
        public string StdDob { get; set; }
        public int StdAge { get; set; }

    }
}