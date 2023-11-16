using WebAPI_ASP_Net.Repositories;
using WebAPI_ASP_Net.Repositories.Containers.Dictionary;
using WebAPI_ASP_Net.Repositories.Containers.List;
using WebAPI_ASP_Net.Repositories.Containers.Queue;
using WebAPI_ASP_Net.Repositories.Containers.Stack;
using WebAPI_ASP_Net.Repositories.List;
using WebAPI_ASP_Net.Repositories.Queue;
using WebAPI_ASP_Net.Repositories.Stack;
using WebAPI_ASP_Net.Utils.Timer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDictionaryContainer<int, int>, DictionaryContainer<int, int>>();
builder.Services.AddSingleton<IDictionaryRepository<int, int>, DictionaryRepository<int, int>>();

builder.Services.AddSingleton<IQueueContainer<int>, QueueContainer<int>>();
builder.Services.AddSingleton<IQueueRepository<int>, QueueRepository<int>>();

builder.Services.AddSingleton<IListContainer<int>, ListContainer<int>>();
builder.Services.AddSingleton<IListRepository<int>, ListRepository<int>>();

builder.Services.AddSingleton<IStackContainer<int>, StackContainer<int>>();
builder.Services.AddSingleton<IStackRepository<int>, StackRepository<int>>();

builder.Services.AddScoped<ITimer, PerformanceTimer>();

var app = builder.Build();

app.UseCors(builder =>
      builder
          .AllowAnyHeader()
          .AllowAnyMethod()
          .AllowCredentials()
   );

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
    });
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
