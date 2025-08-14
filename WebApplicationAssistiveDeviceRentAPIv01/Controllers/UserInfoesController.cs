using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplicationAssistiveDeviceRentAPIv01.Class;
using WebApplicationAssistiveDeviceRentAPIv01.Models;
using WebApplicationAssistiveDeviceRentAPIv01.Models.Dto;
using WebApplicationAssistiveDeviceRentAPIv01.Security;

namespace WebApplicationAssistiveDeviceRentAPIv01.Controllers
{
    public class UserInfoesController : ApiController
    {
        private DBModel db = new DBModel();


        /// <summary>
        /// api/v1/member/profile
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthFilter]
        [Route("api/v1/member/profile")]
        public IHttpActionResult UserProfile()
        {
            var userToken = JwtAuthUtil.GetPayload(Request.Headers.Authorization.Parameter);

            try
            {
                int userId = Convert.ToInt32(userToken["UserId"].ToString());

                //先確認User, UserInfo存在
                if (db.User.Where(u => u.IsDeleted == false).Any(u => u.UserId == userId) && db.UserInfo.Where(ui => ui.IsDeleted ==false).Any(ui => ui.UserId == userId))
                {
                    var selUser = db.User.Where(u => u.IsDeleted == false).Where(u => u.UserId == userId).FirstOrDefault();
                    var selUserInfo = db.UserInfo.Where(ui => ui.IsDeleted == false).Where(u => u.UserId == userId).FirstOrDefault();

                    var selData = new UserProfileDto
                    {
                        name = selUser.UserName,
                        gender = selUserInfo.Gender,
                        dobDate = selUserInfo.Birth ?? DateTime.MinValue,
                        dobStamp =(selUserInfo.Birth ?? DateTime.MinValue).ToString("yyyy-MM-dd"),
                        email = selUser.UserEmail,
                        phone = selUserInfo.UserPhone,
                        contactTime = selUserInfo.AllowedContactPeriod,
                        addressZip =selUserInfo.AddressZIP,
                        addressCity = selUserInfo.AddressCity,
                        addressDistrict=selUserInfo.AddressDistinct,
                        addressDetail = selUserInfo.AddressDetail
                    };

                    var result = new
                    {
                        statusCode = 200,
                        status = true,
                        message = "取得會員資料成功",
                        data = selData
                    };

                    return Ok(result);
                }
                else
                {
                    //沒找到user

                    var result = new
                    {
                        statusCode = 404,
                        status = false,
                        message = "no user exist."
                    };
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                var result = new
                {
                    statusCode = 500,
                    status = false,
                    message = "Server error"
                };
                return Ok(result);
            }

        }


        /// <summary>
        /// api/v1/member/profile
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [JwtAuthFilter]
        [Route("api/v1/member/profile")]
        public IHttpActionResult UserProfileUpdate([FromBody] UserProfileUpdateRequestDto request)
        {
            var userToken = JwtAuthUtil.GetPayload(Request.Headers.Authorization.Parameter);

            try
            {
                int userId = Convert.ToInt32(userToken["UserId"].ToString());

                //先確認User, UserInfo存在
                if (db.User.Where(u => u.IsDeleted == false).Any(u => u.UserId == userId) && db.UserInfo.Where(ui => ui.IsDeleted == false).Any(ui => ui.UserId == userId))
                {
                    var selUser = db.User.Where(u => u.IsDeleted == false).Where(u => u.UserId == userId).FirstOrDefault();
                    var selUserInfo = db.UserInfo.Where(ui => ui.IsDeleted == false).Where(u => u.UserId == userId).FirstOrDefault();

                    selUser.UserName=request.name;
                    selUserInfo.Gender=request.gender;
                    selUserInfo.Birth=request.dobStamp;
                    selUserInfo.UserPhone=request.phone;
                    selUserInfo.AllowedContactPeriod=request.contactTime;
                    selUserInfo.AddressZIP=request.addressZip;
                    selUserInfo.AddressCity=request.addressCity;
                    selUserInfo.AddressDistinct=request.addressDistrict;
                    selUserInfo.AddressDetail=request.addressDetail;

                    selUser.UpdateAt=DateTime.Now;
                    selUserInfo.UpdateAt= DateTime.Now;

                    db.SaveChanges();

                    var result = new
                    {
                        statusCode = 200,
                        status = true,
                        message = "編輯會員資料成功",
                    };

                    return Ok(result);
                }
                else
                {
                    //沒找到user

                    var result = new
                    {
                        statusCode = 404,
                        status = false,
                        message = "no user exist."
                    };
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                var result = new
                {
                    statusCode = 500,
                    status = false,
                    message = "Server error"
                };
                return Ok(result);
            }

        }






    }
}