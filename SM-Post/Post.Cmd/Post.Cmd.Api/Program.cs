using Post.Cmd.Infrastructure.Config;
using Post.Cmd.Infrastructure;
using Post.Cmd.Application.Commands;
using Post.Cmd.Infrastructure.Dispatchers;
using Post.Cmd.Application.Commands.Comment.AddCommentCommand;
using Post.Cmd.Application.Commands.Comment.EditCommentCommand;
using Post.Cmd.Application.Commands.Comment.RemoveCommentComand;
using Post.Cmd.Application.Commands.Message.EditMessageCommand;
using Post.Cmd.Application.Commands.Post.DeletePostCommand;
using Post.Cmd.Application.Commands.Post.LikePostCommand;
using Post.Cmd.Application.Commands.Post.NewPostCommand;
using CQRS.Core.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection("MongoDbConfig"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPostCmdServiceDependencies();

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
