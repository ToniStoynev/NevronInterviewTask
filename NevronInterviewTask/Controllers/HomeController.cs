using Microsoft.AspNetCore.Mvc;
using NevronInterviewTask.Extensions;

namespace NevronInterviewTask.Controllers;

public class HomeController : Controller
{
    private readonly Random _random = new();
    private const string SumKey = "IsSummed";
    private const string SessionKey = "Key";
    private readonly IHttpContextAccessor _contextAccessor;

    public HomeController(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public IActionResult Index()
    {
        List<int>? numbers = _contextAccessor?.HttpContext?.Session.GetComplexData<List<int>>(SessionKey);

        ViewBag.Numbers = numbers;
        ViewBag.IsSummed = _contextAccessor?.HttpContext?.Session.GetInt32(SumKey);
        ViewBag.Sum = numbers?.Sum() ?? 0;

        return View();
    }

    [HttpPost]
    public IActionResult ClearNumbers()
    {
        _contextAccessor?.HttpContext?.Session.Clear();
        return Ok(new { numbers = new List<int>(), sum = 0 });
    }

    [HttpPost]
    public IActionResult AddNumber()
    {
        int newNumber = _random.Next(1, 1000);

        List<int>? numbers = _contextAccessor?.HttpContext?.Session.GetComplexData<List<int>>(SessionKey);

        if (numbers is null)
        {
            numbers = new List<int>();
        }

        numbers.Add(newNumber);

        _contextAccessor?.HttpContext?.Session.SetComplexData(SessionKey, numbers);

        return Ok(new { numbers });
    }

    [HttpPost]
    public IActionResult SumNumbers()
    {
        List<int>? numbers = _contextAccessor?.HttpContext?.Session.GetComplexData<List<int>>(SessionKey);

        if (numbers is not null)
        {
            _contextAccessor?.HttpContext?.Session.SetInt32(SumKey, 1);
            int? sum = numbers.Sum();
            return Ok(sum);
        }

        return Ok();
    }
}
