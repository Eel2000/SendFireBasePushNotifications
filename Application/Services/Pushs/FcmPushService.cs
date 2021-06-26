using Application.DTOs.Pushs;
using CorePush.Google;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Pushs
{
    public class FcmPushService : IFcmPushService
    {
        private readonly string fcmUrl = "https://fcm.googleapis.com/fcm/send";
        public FcmPushService()
        {
            //FirebaseApp.Create(new AppOptions()
            //{
            //    Credential = GoogleCredential.FromFile("C:/Users/CTRL TECH/Documents/Certif/Keys/aspfcm-8444d4a6ebd4.json")
            //});
        }

        //public async Task<object> SendAsync(MessageDto message)
        //{
        //    try
        //    {
        //        var settings = new FcmSettings
        //        {
        //            SenderId = "537043514648",
        //            ServerKey = "AAAAfQpJtRg:APA91bE250X3qL2CGbSziF_2Ro8hqHUDEmt_W6rI_rHEhlBM6wf_BQcLW8rt7jVnicYt2JIandfl9LvtEpI95xoPkUUPwD1PZRGjc9jxgPpC8HTU9NiATZkCnk2cSHYvXi6pL80_-8rm"
        //        };

        //        var fcm = new FcmSender(settings, new HttpClient());
        //        var playload = new
        //        {
        //            notification = new { title = "Message", body = "Hello wolrd" },
        //            playload = new { title = "message" }
        //        };

        //        var msg = new
        //        {
        //            Data = new Dictionary<string, string>()
        //            {
        //                { "title", "Message" },
        //                { "body", "Message" },
        //            },
        //            Topic = "Parent",
        //            To = "ehaIRodPMD8:APA91bGd92_D7rKy5pBsBUhkS6ktYwiZ3-EssV4ZWZMw82eojbQgG2m77xemFf3PmLkYHMtvkar6aRlhrQ69ZO0TRmyiYHdxV9BnWe-_lwGG8a_nmfrjIAb6FJ0oYt7orzsB0fdedrKb"
        //        };

        //        var msg = new Message
        //        {
        //            Notification = new Notification()
        //            {
        //                Title = "Message",
        //                Body = "$GOOG gained 11.80 points to close at 835.67, up 1.43% on the day.",
        //            },
        //            Data = new Dictionary<string, string>()
        //            {
        //                { "title", "Message" },
        //                { "body", "Message" },
        //            },
        //            Android = new AndroidConfig()
        //            {
        //                TimeToLive = TimeSpan.FromHours(1),
        //                Notification = new AndroidNotification()
        //                {
        //                    Icon = "stock_ticker_update",
        //                    Color = "#f45342",
        //                },
        //            },
        //            Apns = new ApnsConfig()
        //            {
        //                Aps = new Aps()
        //                {
        //                    Badge = 42,
        //                },
        //            },
        //            Topic = "Parent"
        //        };

        //        var result = await SendAsync(msg, settings);

        //        return new
        //        {
        //            status = "SUCCESS",
        //            message = result
        //        };
        //    }
        //    catch (Exception e)
        //    {
        //        return new
        //        {
        //            status = "ERROR",
        //            message = e.Message,
        //            data = e
        //        };
        //    }
        //}

        public async Task<object> SendAsync(MessageDto message)
        {
            //string response = string.Empty;
            try
            {
                //var app = FirebaseApp.GetInstance("baseApp");
                //if (ReferenceEquals(app, null))
                //{
                   

                //}

                var msg = new Message()
                {
                    Data = new Dictionary<string, string>()
                    {
                        { "title", message.Title },
                        { "body", message.Body },
                    },
                    //Token = message.Token
                    Topic = message.Topic
                };

                var response = await FirebaseMessaging.DefaultInstance.SendAsync(msg);
                //var response = await HttpSend(msg);

                return new
                {
                    status = "SUCCESS",
                    message = "Notification sent",
                    data = response
                };
            }
            catch (Exception e)
            {
                return new
                {
                    status = "ERROR",
                    message = "ERRR_00: failed to send push notification please check logs",
                    data = e
                };
            }
        }

        private async Task<string> HttpSend(object msg)
        {
            HttpRequestMessage httpRequest = null;
            HttpClient httpClient = null;

            var authorizationKey = string.Format("Key={0}", "AAAAoyPFG0g:APA91bG3yLGiDsCQMdvL4cBHYIikwilHnqofkYmhzPnM3awfbWEaOhUPiJcDq6tbSiL94-f0xg5dxttwQMgIs8TCSSMvK3dPmUo5dB7gojs8rgOu5S8DhIXwAkAT8jtb9hyFYx8UD_Zz");
            var jsonBody = JsonConvert.SerializeObject(msg);

            httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send");
            httpRequest.Headers.TryAddWithoutValidation("Authorization", authorizationKey);
            httpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            httpClient = new HttpClient();
            var result = await httpClient.SendAsync(httpRequest);

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadAsStringAsync();
            }

            return result.StatusCode.ToString();
        }

        public async Task<FcmResponse> SendAsync(object payload, FcmSettings settings, CancellationToken cancellationToken = default)
        {
            var serialized = JsonConvert.SerializeObject(payload);
            var http = new HttpClient();

            using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, fcmUrl))
            {
                httpRequest.Headers.Add("Authorization", $"key = {settings.ServerKey}");

                if (!string.IsNullOrEmpty(settings.SenderId))
                {
                    httpRequest.Headers.Add("Sender", $"id = {settings.SenderId}");
                }

                httpRequest.Content = new StringContent(serialized, Encoding.UTF8, "application/json");

                using (var response = await http.SendAsync(httpRequest, cancellationToken))
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException("Firebase notification error: " + responseString);
                    }

                    return JsonConvert.DeserializeObject<FcmResponse>(responseString);
                }
            }
        }
    }
}
