using MongoDB.Driver;
using MyBackendAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// --- 1. MONGODB BEÁLLÍTÁSA ---
var mongoUri = Environment.GetEnvironmentVariable("MONGO_URI") 
               ?? "mongodb://dbuser:dbpassword@mongodb.my-app-space.svc.cluster.local:27017/myappdb";

builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoUri));
builder.Services.AddScoped(sp => sp.GetRequiredService<IMongoClient>().GetDatabase("myappdb"));

// --- 2. CORS BEÁLLÍTÁSA ---
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// --- 3. SERVICEEK REGISZTRÁCIÓJA ---
builder.Services.AddScoped<BookService>(); 
builder.Services.AddControllers(); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- 4. MIDDLEWARE ÉS ÚTVONALAK ---

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers(); 

app.MapGet("/db-test", async (IMongoDatabase db) => {
    try {
        await db.RunCommandAsync((Command<MongoDB.Bson.BsonDocument>)"{ping:1}");
        return Results.Ok(new { status = "success", message = "MongoDB kapcsolat OK!" });
    }
    catch (Exception ex) {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/", () => "A .NET Backend szalad és a Controller-ek aktívak!");

app.Run();