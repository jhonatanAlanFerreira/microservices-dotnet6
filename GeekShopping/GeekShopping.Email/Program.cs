using GeekShopping.Email.MessageConsumer;
using GeekShopping.Email.Model.Context;
using GeekShopping.Email.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];
var mysqlVersion = new MySqlServerVersion(new Version(8, 0, 5));

builder.Services.AddDbContext<MySQLContext>(options => options.UseMySql(connection, mysqlVersion));

var mqBuilder = new DbContextOptionsBuilder<MySQLContext>();
mqBuilder.UseMySql(connection, mysqlVersion);
builder.Services.AddSingleton(new EmailRepository(mqBuilder.Options));
builder.Services.AddScoped<IEmailRepository, EmailRepository>();
builder.Services.AddHostedService<RabbitMQPaymentConsumer>();

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
