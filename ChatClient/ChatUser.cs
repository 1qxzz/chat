using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
    [Table("ChatLoginUser")]
    public class ChatUser
    {
        [Key]
        public string Uname {  get; set; }
        public string Name { get; set; }
        public string Pwd { get; set; }
    }
}
