using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplicationAssistiveDeviceRentAPIv01.Class;
using WebApplicationAssistiveDeviceRentAPIv01.Models;
using WebApplicationAssistiveDeviceRentAPIv01.Models.Dto.LineBot;

namespace WebApplicationAssistiveDeviceRentAPIv01.Controllers
{
    public class LineBotController : ApiController
    {
        //實體化DB
        private DBModel db = new DBModel();


        [HttpPost]
        [Route("api/v1/linebot/reply")]
        public async Task<IHttpActionResult> ReplyBot()
        {
            string ChannelAccessToken = LineBotMsg.ChannelAccessToken;

            try
            {
                //取得 http Post RawData(should be JSON)
                string postData = Request.Content.ReadAsStringAsync().Result;
                //剖析JSON
                var ReceivedMessage = isRock.LineBot.Utility.Parsing(postData);
                //回覆訊息
                string Message;
                Message = "你說了:" + ReceivedMessage.events[0].message.text;
                //回覆用戶
                isRock.LineBot.Utility.ReplyMessage(ReceivedMessage.events[0].replyToken, Message, ChannelAccessToken);
                //回覆API OK
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok();
            }
        }

        [HttpPost]
        [Route("api/v1/linebot/push")]
        public async Task<IHttpActionResult> PushBot([FromBody] LinePushDto request)
        { 
            string channelAccessToken = LineBotMsg.ChannelAccessToken;
            
            //Line Push url
            string apiEndpoint = "https://api.line.me/v2/bot/message/push";

            //test msg pushed
            //string message = "Hello, LINE!  push測試";
            string message = request.message;
            //push 對象LineId
            //我的LineId用來測試  LineId = ""
            string toUserId = "";

            var jsonContent = new
            {
                to = toUserId,
                messages = new[]
                {
                    new
                    {
                        type="text",
                        text= message
                    }
                }
            };

            string jsonString = JsonConvert.SerializeObject(jsonContent);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            // 使用 HttpClient 發送 POST 請求
            using (var httpClient = new HttpClient())
            {
                // 設定 Authorization 標頭
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {channelAccessToken}");
                // 發送請求
                var response = await httpClient.PostAsync(apiEndpoint, content);
                // 處理回應
                if (response.IsSuccessStatusCode)
                {
                    //Console.WriteLine("消息發送成功！");

                    var result = new
                    {
                        StatusCode = response.StatusCode,
                        msg = await response.Content.ReadAsStringAsync()
                    };

                    return Ok(result);

                }
                else
                {
                    //Console.WriteLine($"發生錯誤：{response.StatusCode} - {await response.Content.ReadAsStringAsync()}");

                    var result = new
                    {
                        StatusCode = response.StatusCode,
                        msg = await response.Content.ReadAsStringAsync()
                    };

                    return Ok(result);
                }
            }


        }


        [HttpPost]
        [Route("api/v1/linebot/push_button")]
        public async Task<IHttpActionResult> PushBotButton()
        {
            //string channelAccessToken = LineBotMsg.ChannelAccessToken;
            string channelAccessToken = ConfigurationManager.AppSettings["LineMsgBot_ChannelAccessToken"];

            bool isPushSuccess = false;
            //Line Push url
            string apiEndpoint = "https://api.line.me/v2/bot/message/push";

            //test msg pushed
            //string message = selMsg;
            //push 對象LineId
            //我的LineId用來測試  LineId = ""
            string selLineId = "";
            string toUserId = selLineId;

            //List<LinePushTemplateButtonActionUriDto> actionUris = deser



            var jsonContent = new
            {
                to = toUserId,
                messages = new[]
                {
                    new
                    {
                        type="template",
                        altText = "btn template",
                        template = new
                        {
                            type = "buttons",
                            thumbnailImageUrl=ServerPath.Domain+"/picture/logo/LOGO-default.jpg",
                            imageAspectRatio="square",
                            imageSize = "cover",
                            imageBackgroundColor = "#FFFFFF",
                            //title = "123",
                            text = "請點擊:",
                            defaultAction = new
                            {
                                type = "uri",
                                label = "assist-hub",
                                uri = "https://assist-hub.vercel.app"
                            },
                            actions = new LinePushTemplateButtonActionUriDto[] {
                                new LinePushTemplateButtonActionUriDto
                                {
                                    type="uri",
                                    label="AA063",
                                    uri="https://assist-hub.vercel.app/inquiry/AA063"
                                },
                                new LinePushTemplateButtonActionUriDto
                                {
                                    type="uri",
                                    label="AA063S",
                                    uri="https://assist-hub.vercel.app/suggest/AA063S"
                                },
                            }
                        }
                    }
                }
            };

            string jsonString = JsonConvert.SerializeObject(jsonContent);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            // 使用 HttpClient 發送 POST 請求
            using (var httpClient = new HttpClient())
            {
                // 設定 Authorization 標頭
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {channelAccessToken}");
                // 發送請求
                var response = await httpClient.PostAsync(apiEndpoint, content);
                // 處理回應
                if (response.IsSuccessStatusCode)
                {
                    //Console.WriteLine("消息發送成功！");

                    var result = new
                    {
                        StatusCode = response.StatusCode,
                        msg = await response.Content.ReadAsStringAsync()
                    };

                    return Ok(result);

                }
                else
                {
                    //Console.WriteLine($"發生錯誤：{response.StatusCode} - {await response.Content.ReadAsStringAsync()}");

                    var result = new
                    {
                        StatusCode = response.StatusCode,
                        msg = await response.Content.ReadAsStringAsync()
                    };

                    return Ok(result);
                }
            }


        }





    }
}
