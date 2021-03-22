using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Votacao.Extensions
{
    public class MessageViewModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string RedirectTo { get; set; }
        public bool Success { get; set; }
    }
}
