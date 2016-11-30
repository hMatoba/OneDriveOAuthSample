using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;

namespace OneDriveOAuthSample.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        [Route("OA")]
        public string Foo()
        {
            return "https://login.live.com/oauth20_authorize.srf?client_id={client_id}&scope=onedrive.readonly&response_type=code&redirect_uri=http:%2F%2Flocalhost:61155%2FAdmin%2FOAuth";
        }

        [Route("OAuth")]
        public async Task<string> FooAsync()
        {
            var client_id = "";
            var redirect_uri = "http://localhost/Admin/OAuth";
            var client_secret = "";
            var code = Request.Query["code"];

            var httpClient = new System.Net.Http.HttpClient();

            // まず持って帰ってきた認証コードを使ってトークンを取得する
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "client_id", client_id },
                { "redirect_uri", redirect_uri },
                { "client_secret", client_secret },
                { "code", code },
                { "grant_type", "authorization_code" },
            });
            var codeResponse = await httpClient.PostAsync("https://login.live.com/oauth20_token.srf", content);
            var codeResponseBody = await codeResponse.Content.ReadAsStringAsync();
            var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(codeResponseBody);
            var token = jsonObj["access_token"];

            // 取得したトークンを使ってOneDriveにユーザ情報を要求する
            var uri = $"https://api.onedrive.com/v1.0/drive?access_token={token}";
            var tokenResponse = await httpClient.GetAsync(uri);
            var tokenResponseBody = await tokenResponse.Content.ReadAsStringAsync();

            // ユーザを確認できる情報が得られたのでごにょごにょする
            // Foo(tokenResponseBody);

            // return tokenResponseBody;

        }

    }
}
