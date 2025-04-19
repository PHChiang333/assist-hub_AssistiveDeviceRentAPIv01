using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Results;
using WebApplicationAssistiveDeviceRentAPIv01.Class;
using WebApplicationAssistiveDeviceRentAPIv01.Domain;
using WebApplicationAssistiveDeviceRentAPIv01.Models;
using WebApplicationAssistiveDeviceRentAPIv01.Models.Dto;
using WebApplicationAssistiveDeviceRentAPIv01.Security;


namespace WebApplicationAssistiveDeviceRentAPIv01.Controllers
{
    ////參考:
    ////https://medium.com/appxtech/day-20-%E8%AE%93-c-%E4%B9%9F%E8%83%BD%E5%BE%88-social-net-6-c-%E8%88%87-line-services-api-%E9%96%8B%E7%99%BC-line-login-api-%E4%B8%80-%E8%B5%B7%E6%89%8B%E5%BC%8F-31d552b9cd71#0c2e

    public class LineLoginController : ApiController
    {
        //實體化DB
        private DBModel db = new DBModel();

        //FROM GPT  ----START----
        private const string LineTokenEndpoint = "https://api.line.me/oauth2/v2.1/token";
        private const string LineProfileEndpoint = "https://api.line.me/v2/profile";

        private string ClientId = ConfigurationManager.AppSettings["LineLoginClientId"];
        private string ClientSecret = ConfigurationManager.AppSettings["LineLoginClientSecret"];
        //測試用
        private const string RedirectUri = "https://localhost:44361/api/account/linecallback";
        //實際的address
        //private string RedirectUri = ServerPath.Domain + "/api/account/linecallback";
        //private string RedirectUri = "https://assist-hub.vercel.app/auth/confirm";

        private readonly string idTokenProfileUrl = "https://api.line.me/oauth2/v2.1/verify";

        /// <summary>
        /// LineLogin callback api
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/account/linecallback")]
        public async Task<IHttpActionResult> LineCallback(string code, string state)
        {
            if (code == null || string.IsNullOrEmpty(code) || state == null || string.IsNullOrEmpty(state))
                return BadRequest("Invalid Line login request.");

            try
            {
                // 1. 交換 Access Token
                var tokenResponse = await GetAccessToken(code);
                if (tokenResponse == null)
                    return BadRequest("Failed to get access token.");

                // 2.1. 使用 Access Token 取得用戶資料
                var profile = await GetUserProfile(tokenResponse.AccessToken);
                if (profile == null)
                    return BadRequest("Failed to get user profile.");

                // 2.2 使用IdToke取得 用戶資料
                var profileByIdToken = await GetUserProfileByIdToken(tokenResponse.IdToken, profile.UserId);
                if (profileByIdToken == null)
                    return BadRequest("Failed to get user profile by IdToken.");

                //3.驗證帳號是否存在，並回傳結果

                string selLineId = profile.UserId;
                string selLineUserName = profile.DisplayName;
                string selLineEmail = profileByIdToken.email;

                var existingUser = db.User.Where(u => u.IsDeleted == false).Any(u => u.UserEmail == selLineEmail);

                
                if (existingUser)
                {
                    //有驗證資料

                    var selExistingUser = db.User.Where(u => u.IsDeleted == false).Where(u => u.UserEmail == selLineEmail).FirstOrDefault();
                    var selExistingUserInfo = db.UserInfo.Where(ui => ui.IsDeleted == false).SingleOrDefault(ui => ui.UserId == selExistingUser.UserId);

                    //更新資料
                    selExistingUser.LineId = selLineId;

                    selExistingUser.UserName = selLineUserName;
                    selExistingUser.UserEmail = selLineEmail;
                    selExistingUser.IsAdmin = false;

                    selExistingUser.UpdateAt = DateTime.Now;

                    db.SaveChanges();


                    //生成JWTtoken
                    JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
                    string jwtToken = jwtAuthUtil.GenerateToken(selExistingUser.UserId);

                    var selData = new
                    {
                        jwtToken = jwtToken,
                        name = selExistingUser.UserName,
                        email = selExistingUser.UserEmail,
                        phone = selExistingUserInfo.UserPhone,
                        addressZip = selExistingUserInfo.AddressZIP,
                        addressCity = selExistingUserInfo.AddressCity,
                        addressDistinct = selExistingUserInfo.AddressDistinct,
                        addressDetail = selExistingUserInfo.AddressDetail,
                        IsAdmin = selExistingUser.IsAdmin,
                    };

                    var responseStr = new
                    {
                        statusCode = 200,
                        status = true,
                        message = "登入成功！(用戶已存在)",
                        //jwtToken = jwtToken,
                        data = selData

                    };

                    return Ok(responseStr);
                }
                else
                {
                    //無驗證資料

                    //新增用戶
                    User userAdd = new User();

                    userAdd.LineId = selLineId;
                    userAdd.UserName = selLineUserName;
                    userAdd.UserEmail = selLineEmail;
                    userAdd.UserPassword = Function.GenerateRandomCode()+ Function.GenerateRandomCode()+ Function.GenerateRandomCode()+"!Aa1"; //給一個亂數避免被直接登入
                    userAdd.IsAdmin = false;

                    userAdd.CreateAt = DateTime.Now;
                    userAdd.UpdateAt = DateTime.Now;
                    userAdd.IsDeleted = false;
                    userAdd.DeleteAt = null;

                    db.User.Add(userAdd);
                    db.SaveChanges();

                    //新增UserInfo


                    var selNewUser = db.User.Where(u => u.UserEmail == selLineEmail).FirstOrDefault();

                    int selNewUserId = selNewUser.UserId;

                    UserInfo userInfoAdd = new UserInfo();

                    userInfoAdd.UserId = selNewUserId;

                    userInfoAdd.Gender = "";
                    userInfoAdd.Birth = DateTime.Now;
                    userInfoAdd.UserPhone = "";
                    userInfoAdd.AllowedContactPeriod = "全天 09：00 - 21：00";
                    userInfoAdd.AddressZIP = "";
                    userInfoAdd.AddressCity = "";
                    userInfoAdd.AddressDistinct = "";
                    userInfoAdd.AddressDetail = "";

                    userInfoAdd.CreateAt = DateTime.Now;
                    userInfoAdd.UpdateAt = DateTime.Now;
                    userInfoAdd.IsDeleted = false;
                    userInfoAdd.DeleteAt = null;


                    db.UserInfo.Add(userInfoAdd);
                    db.SaveChanges();

                    //TODO 要改DB 的email unique 不然軟刪除會有問題

                    //找尋用戶
                    var selExistingUser = db.User.Where(u => u.IsDeleted == false).Where(u => u.UserEmail == selLineEmail).FirstOrDefault();
                    var selExistingUserInfo = db.UserInfo.Where(ui => ui.IsDeleted == false).SingleOrDefault(ui => ui.UserId == selExistingUser.UserId);

                    //生成JWTtoken
                    JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
                    string jwtToken = jwtAuthUtil.GenerateToken(selExistingUser.UserId);

                    var selData = new
                    {
                        jwtToken = jwtToken,
                        name = selExistingUser.UserName,
                        email = selExistingUser.UserEmail,
                        phone = selExistingUserInfo.UserPhone,
                        addressZip = selExistingUserInfo.AddressZIP,
                        addressCity = selExistingUserInfo.AddressCity,
                        addressDistinct = selExistingUserInfo.AddressDistinct,
                        addressDetail = selExistingUserInfo.AddressDetail,
                        IsAdmin = selExistingUser.IsAdmin,
                    };

                    var responseStr = new
                    {
                        statusCode = 200,
                        status = true,
                        message = "登入成功！(新用戶註冊)",
                        //jwtToken = jwtToken,
                        data = selData

                    };

                    return Ok(responseStr);

                }

                }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private async Task<LineTokenResponse> GetAccessToken(string code)
        {
            using (var client = new HttpClient())
            {
                var formData = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"grant_type", "authorization_code"},
                {"code", code},
                {"redirect_uri", RedirectUri},
                {"client_id", ClientId},
                {"client_secret", ClientSecret}
            });

                var response = await client.PostAsync(LineTokenEndpoint, formData);
                if (!response.IsSuccessStatusCode) return null;

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<LineTokenResponse>(json);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        private async Task<LineProfileResponse> GetUserProfile(string accessToken)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                var response = await client.GetAsync(LineProfileEndpoint);
                if (!response.IsSuccessStatusCode) return null;

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<LineProfileResponse>(json);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idToken"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        private async Task<UserIdTokenProfileDto> GetUserProfileByIdToken(string idToken, string user_id)
        {
            using (var client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {idToken}");

                var formData = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"id_token", idToken},
                {"client_id", ClientId},
                {"nonce", "09876xyz"},
                {"user_id", user_id}
            });

                //client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
                //var response = await client.PostAsync(LineTokenEndpoint, formData);
                var response = await client.PostAsync(idTokenProfileUrl, formData);
                if (!response.IsSuccessStatusCode) return null;

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserIdTokenProfileDto>(json);
            }
        }


        // FROM GPT  ----End----
    }
}


