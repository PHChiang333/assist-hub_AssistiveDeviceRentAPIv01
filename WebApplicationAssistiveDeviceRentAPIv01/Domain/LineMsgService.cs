using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using WebApplicationAssistiveDeviceRentAPIv01.Class;
using WebApplicationAssistiveDeviceRentAPIv01.Models.Dto.LineBot;
using static System.Net.Mime.MediaTypeNames;

namespace WebApplicationAssistiveDeviceRentAPIv01.Domain
{
    public class LineMsgService
    {


        /// <summary>
        /// LineMsgPush正式服務
        /// </summary>
        /// <param name="selLineId">推送目標LineId</param>
        /// <param name="selMsg">推送內容</param>
        /// <returns></returns>
        public async Task<bool> LineBotPush(string selLineId, string selMsg)
        {
            //改成從Web.config
            string channelAccessToken = ConfigurationManager.AppSettings["LineMsgBot_ChannelAccessToken"];
            bool isPushSuccess = false;
            //Line Push url
            string apiEndpoint = "https://api.line.me/v2/bot/message/push";

            //test msg pushed
            string message = selMsg;
            //push 對象LineId
            string toUserId = selLineId;

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

                    isPushSuccess = true;
                    return isPushSuccess;

                }
                else
                {
                    isPushSuccess = false;
                    return isPushSuccess;
                }
            }
        }





    }
}