using Post.Query.Infrastructure;
using Confluent.Kafka;
using Post.Query.Infrastructure.Consumers;
using Post.Query.Application;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));

// Add DbContext 
builder.Services.AddDatabaseContext(builder.Configuration.GetConnectionString("SqlServer"));

builder.Services.AddInfrastructureDependencies();
builder.Services.AddServiceDependencies();

builder.Services.AddControllers();

builder.Services.AddHostedService<ConsumerHostedService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
