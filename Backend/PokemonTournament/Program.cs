

using PokemonTournament.Infrastructure;
using PokemonTournament.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//Since this is a small project, I have added them here for simplicity.
// In a larger project, I use ServiceCollectionExtensions to register services in a separate class.
builder.Services.AddHttpClient<IPokeClient, PokeClient>();
builder.Services.AddScoped<IBattleService, BattleService>();
builder.Services.AddScoped<ITournamentService, TournamentService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularApp", policy =>
        policy.WithOrigins(
            "http://localhost:4200",
            "http://localhost:5129",
            "https://localhost:7025")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AngularApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
