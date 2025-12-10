using HTLVBFingerflitzer.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

IConfigurationSection dailyChallengeSection =
    builder.Configuration.GetSection("DailyChallenge");
string? dailyChallengeTextGeneratorType =
    dailyChallengeSection.GetValue<string>("Type");
if (dailyChallengeTextGeneratorType == "static-text")
{
    builder.Services.AddSingleton<IDailyChallengeTextGenerator>(
        new DailyChallengeStaticTextGenerator(
            dailyChallengeSection.GetValue<string>("StaticText") ?? "Das ist der 5AHWII FingerFlitzer der viel Spaß macht!" // -> Test
        )
    );
}
else if (dailyChallengeTextGeneratorType == "rotating-text")
{
    builder.Services.AddSingleton<IDailyChallengeTextGenerator>(
        new DailyChallengeRotatingTextGenerator(
            dailyChallengeSection.GetSection("TextsToRotate").Get<string[]>() ?? ["Hallo das ist der 5AHWII Finger Flitzer123!"]
        )
    );
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
