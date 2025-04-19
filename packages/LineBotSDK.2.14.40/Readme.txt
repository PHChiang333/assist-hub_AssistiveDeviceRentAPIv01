=========================================================================
LineBotSDK for .NET core & .NET Framework 
 (LINE Messaging API, LINE Login, LINE Notify, Liff App, RichMenu supported)

Recommended .net version is .NET 4.7.2
Recommended .net core version  is .NET Core 3.1
=========================================================================

Example of Usage:
https://github.com/isdaviddong/LineBotSdkExample
https://github.com/isdaviddong/Line_Login_Example
https://github.com/isdaviddong/Line_Notify_Example
https://github.com/isdaviddong/Linebot-Demo-CopyCat
https://github.com/isdaviddong/Linebot-Demo-AccountBook

 ===================== Easy to use ==================
Push Message : 
isRock.LineBot.Utility.PushMessage (user id, text message, AccessToken);

Parsing Receieved Message in WebHook (Parsing received JSON):
var ReceivedMessage = isRock.LineBot.Utility.Parsing (postData);

Reply Message  
isRock.LineBot.Utility.ReplyMessage (ReplyToken, text message, AccessToken);

=========================================================================
.net core examples:
=========================================================================
LINE Bot Examples
https://github.com/isdaviddong/Example-LinePushAPI-dotNetCore31
https://github.com/isdaviddong/Example-PushImageMap-dotNetCore3
LINE Bot Example - basic WebHook & push message
https://github.com/isdaviddong/LineBotSdkDotNetCoreWebExample
LINE Login
https://github.com/isdaviddong/LineLogin-dotnetcoreExample


=========================================================================
LINE Bot Examples - .net framework version
=========================================================================
LINE Bot Example - Personal accounting
https://github.com/isdaviddong/Linebot-Demo-AccountBook
LINE Bot Example - Face Recognition
https://github.com/isdaviddong/Linebot-Demo-FaceRecognition
LINE Bot Example - TranslatorKing
https://github.com/isdaviddong/Linebot-Demo-TranslatorKing
LINE Bot Example - Dynamic Rich Menu
https://github.com/isdaviddong/Linebot-Demo-DynamicRichMenu

=========================================================================
How to use (in Chinese):
http://studyhost.blogspot.tw/search/label/LineBot
=================================================== 
History:
2018/5/7			0.7.1	support Get Content/User Info  in WebHook BaseController
2018/5/16		0.7.2	Add MS QnA Macker Helper
2018/5/27		0.7.3	update MS QnA Macker  Helper For GA
2018/8/24		0.7.6	Add Issue short-live Channel Access Token  API support
									Add Liff Server API Support
2018/9/13		0.7.8	Fix QnA Maker call for GA version
2018/9/15		0.8.0	QuickReply supported
2018/10/4		0.8.3	fix QnA Services bug
2018/10/31		0.8.4    support Tempalte Message in MessageBase
2018/11/16		0.8.5	udpate models for Member Join/Leave Events
										ref: https://developers.line.me/en/news/2018/11/15/
2018/12/16		0.8.52  Bug Fix
2019/01/21		0.8.6	Bug Fix
2019/03/25					support Rich Menu And Bug Fix
2019/07/22		2.0.0	.net core supported
2019/07/28		2.0.1	add 9 APIs about messages quota, delivery, Insight
2020/01/25		2.0.10 update LINE Notify API to support long words
2022/05/13	    2.5.33 udpate for Added features such as automatic opening and closing of the rich menu when tapping a rich menu 
									ref: https://developers.line.biz/en/news/2022/05/13/richmenu-keyboard/

=======================================
中文說明 (Chinese)
=======================================
使用範例:
https://github.com/isdaviddong/LineBotSdkExample
如何使用請參考套件公開說明書:
http://studyhost.blogspot.tw/search/label/LineBot
簡易使用說明:
Push Message(主動發訊息給用戶):
isRock.LineBot.Utility.PushMessage(用戶id, 文字訊息, AccessToken);

Parsing Receieved Message(Parsing 收到的JSON): 
var ReceivedMessage = isRock.LineBot.Utility.Parsing(postData);

Reply Message(回覆用戶的訊息):
isRock.LineBot.Utility.ReplyMessage(ReplyToken, 文字訊息, AccessToken);

==================================================
C# Examples:
LINE Bot 範例 - 小P記帳
https://github.com/isdaviddong/Linebot-Demo-AccountBook
LINE Bot 範例 - 人臉辨識
https://github.com/isdaviddong/Linebot-Demo-FaceRecognition
LineBotSdk範例，包含純文字訊息、貼圖、圖片發送
https://github.com/isdaviddong/LineBotSdkExample
Line_Notify_Example
https://github.com/isdaviddong/Line_Notify_Example
Line_Login_Example
https://github.com/isdaviddong/Line_Login_Example
展示如何建立一個可以接收檔案的Line WebHook 
https://github.com/isdaviddong/LineExample_WebHookGetPicture
Carousel Template Example
https://github.com/isdaviddong/LinePushCarouselTemplateExample
Line bot 連續對談機制:
https://github.com/isdaviddong/LinebotConversationExample
Line bot 群組對談範例程式碼:
https://github.com/isdaviddong/LinebotInTheGroup
