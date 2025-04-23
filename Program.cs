using System.Net;
using System.Net.Http.Json;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

var httpClient = new HttpClient();
var responsemono = await httpClient.GetFromJsonAsync<List<CurrencyRate>>("https://api.monobank.ua/bank/currency");
string res ="";

foreach (var rate in responsemono!)
{
    if ((rate.CurrencyCodeA == 840 || rate.CurrencyCodeA == 978) && rate.CurrencyCodeB == 980)
    {
        res=($"{GetCurrencyName(rate.CurrencyCodeA)} -> UAH: Buy = {rate.RateBuy}, Sell = {rate.RateSell}");
    }
}
Console.WriteLine(res);
//6.3
HttpListener listener = new();
listener.Prefixes.Add("http://localhost:5252/");
listener.Start();

Console.WriteLine("Сервер запущено. Перейдіть в браузер: http://localhost:5252/?login=is-33fiot-23-130");

while (true)
{
    var context = listener.GetContext();
    var login = context.Request.QueryString["login"];

    var response = context.Response;
    string result = login == "is-33fiot-23-130"
        ? "<h1>Богдан Корнієнко</h1><p>Курс: 2, Група: ІС-33</p>"
        : $"<h1>{res}</h1>";

    byte[] buffer = Encoding.UTF8.GetBytes(result);
    response.ContentType = "text/html; charset=utf-8";
    response.ContentLength64 = buffer.Length;
    response.OutputStream.Write(buffer);
    response.Close();
}

static string GetCurrencyName(int code) => code switch
{
    840 => "USD",
    978 => "EUR",
    _ => "UNKNOWN"
};

public class CurrencyRate
{
    public int CurrencyCodeA { get; set; }
    public int CurrencyCodeB { get; set; }
    public double? RateBuy { get; set; }
    public double? RateSell { get; set; }
}