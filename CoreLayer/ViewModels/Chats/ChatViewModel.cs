using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.ViewModels.Chats
{
    public class ChatViewModel
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public string Text {  get; set; }
        public DateTime DateTime { get; set; }
    }
}
