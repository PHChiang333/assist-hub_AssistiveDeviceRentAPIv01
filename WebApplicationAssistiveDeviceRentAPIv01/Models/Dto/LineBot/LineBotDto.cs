using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationAssistiveDeviceRentAPIv01.Models.Dto.LineBot
{
    public class LineBotDto
    {
    }

    public class LinePushDto
    {
        public string message { get; set; }
    }

    public class LinePushTemplateButtonActionUriDto
    {
        public string type { get; set; } 
        public string label { get; set; }
        public string uri { get; set; }
    }


}