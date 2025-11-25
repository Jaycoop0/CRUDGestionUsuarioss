using Microsoft.EntityFrameworkCore;
using CRUDGestionUsuarios.Data;
using CRUDGestionUsuarios.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CRUD de Usuarios

app.MapGet("/users", async (AppDbContext db) =>
    await db.Users.ToListAsync());

app.MapGet("/users/{id}", async (int id, AppDbContext db) =>
    await db.Users.FindAsync(id) is User u
    ? Results.Ok(u)
    : Results.NotFound());

app.MapPost("/users", async (User user, AppDbContext db) =>
{
    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Created($"/users/{user.Id}", user);
});

app.MapPut("/users/{id}", async (int id, User data, AppDbContext db) =>
{
    var u = await db.Users.FindAsync(id);
    if (u is null) return Results.NotFound();

    u.Name = data.Name;
    u.Email = data.Email;
    u.Role = data.Role;

    await db.SaveChangesAsync();
    return Results.Ok(u);
});

app.MapDelete("/users/{id}", async (int id, AppDbContext db) =>
{
    var u = await db.Users.FindAsync(id);
    if (u is null) return Results.NotFound();

    db.Users.Remove(u);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();