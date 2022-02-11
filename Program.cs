using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PersonalListlAPIJWT.Models;
using PersonalListlAPIJWT.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
}); 

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IComputerService, ComputerService>();
builder.Services.AddSingleton<IUserService, UserService>();

var app = builder.Build();

app.UseSwagger();
app.UseAuthorization();
app.UseAuthentication();



app.MapGet("/", () => "Hello World!");

app.MapPost("/login",
(UserLogin user, IUserService userservice) => Login(user, userservice))
    .Accepts<UserLogin>("application/json")
    .Produces<string>(); ;

app.MapPost("/create",
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
(Computer computer, IComputerService computerservice) => CreateComputer(computer, computerservice))
    .Accepts<Computer>("application/json")
    .Produces<Computer>(statusCode: 200, contentType: "application/json");

app.MapGet("/get",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Standard, Administrator")]
(int id, IComputerService computerservice) => GetComputer(id, computerservice))
    .Produces<Computer>();

app.MapGet("/list",
    (IComputerService computerservice) => GetComputers(computerservice))
    .Produces<List<Computer>>(statusCode: 200, contentType: "application/json");

app.MapPut("/update",
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
(Computer computer, IComputerService computerservice) => UpdateComputer(computer, computerservice))
    .Accepts<Computer>("application/json")
    .Produces<Computer>(statusCode: 200, contentType: "application/json");

app.MapDelete("/delete",
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
(int id, IComputerService computerservice) => DeleteComputer(id, computerservice));

IResult Login(UserLogin user, IUserService userservice)
{
    if (!string.IsNullOrEmpty(user.Username) &&
        !string.IsNullOrEmpty(user.Password))
    {
        var loggedInUser = userservice.Get(user);
        if (loggedInUser is null) return Results.NotFound("User not found");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, loggedInUser.Username),
            new Claim(ClaimTypes.Email, loggedInUser.Email),
            new Claim(ClaimTypes.Name, loggedInUser.Name),
            new Claim(ClaimTypes.Role, loggedInUser.Role)
        };

        var token = new JwtSecurityToken
        (
            issuer: builder.Configuration["Jwt:Issuer"],
            audience: builder.Configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(60),
            notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                SecurityAlgorithms.HmacSha256)
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Results.Ok(tokenString);
    }
    return Results.BadRequest("Invalid user credentials");
}


IResult CreateComputer(Computer computer, IComputerService computerservice)
{
    var result = computerservice.CreateComputer(computer);
    return Results.Ok(result);
}

IResult GetComputer(int id, IComputerService computerservice)
{
    var computer = computerservice.GetComputer(id);

    if (computer is null)
        return Results.NotFound("Computer not found");

    return Results.Ok(computer);
}

IResult GetComputers(IComputerService computerservice)
{
    var computers = computerservice.GetComputers();

    return Results.Ok(computers);
}

IResult UpdateComputer(Computer computer, IComputerService computerservice)
{
    var updatedComputer = computerservice.UpdateComputer(computer);

    if (updatedComputer is null)
        Results.NotFound("Computer not found");

    return Results.Ok(updatedComputer);
}

IResult DeleteComputer(int id, IComputerService computerservice)
{
    var result = computerservice.DeleteComputer(id);

    if (!result) 
        Results.BadRequest("Something went wrong");

    return Results.Ok(result);
}

app.UseSwaggerUI();

app.Run();
