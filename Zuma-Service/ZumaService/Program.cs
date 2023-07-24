using Microsoft.AspNetCore.Hosting.Server.Features;
using Zuma.Web.Services.Implementation;
using ZumaService.Data;
using ZumaService.Repositories.Implementation;
using ZumaService.Repositories.Interface;
using ZumaService.Services.Implementation;
using ZumaService.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ZumaDB>();

builder.Services.AddScoped(typeof(IVoteBlockRepository), typeof(VoteBlockRepository));

builder.Services.AddScoped(typeof(IVoteBlockValidationService), typeof(VoteBlockValidationService));

builder.Services.AddScoped(typeof(IVoteBlockService), typeof(VoteBlockService));

builder.Services.AddScoped(typeof(IElectionReportService), typeof(ElectionReportService));

builder.Services.AddMemoryCache();

builder.Services.AddHostedService<VoteBlockMineService>();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/ElectionReports/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=ElectionReports}/{action=Index}/{id?}");

app.Run();
