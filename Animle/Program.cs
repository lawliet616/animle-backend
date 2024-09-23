using Animle.services;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

BuilderServiceConfiguration.ConfigureServices(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();
app.UseRateLimiter();



var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7020/gameHub")
                .WithAutomaticReconnect(new SignalRRetryPolicy());


app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<SignarlRHub>("/gameHub");
});

app.MapControllers();

app.Run();

