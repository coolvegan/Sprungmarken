
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var sprungmarken = new Dictionary<String, String>
{
};

void dateiEinlesen()
{
    string path = @"sprungmarken.txt";
    try
    {
        //Open the stream and read it back.
        using (FileStream fs = File.OpenRead(path))
        {
            byte[] b = new byte[1024];
            UTF8Encoding temp = new UTF8Encoding(true);
            int readLen;
            while ((readLen = fs.Read(b, 0, b.Length)) > 0)
            {
                Console.WriteLine(temp.GetString(b, 0, readLen));
            }
        }
        using (var reader = new StreamReader(path))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                sprungmarken.Add(values[0], values[1]);
            }
        }
    }
    catch (FileNotFoundException e)
    {
        Console.WriteLine(e);
    }
    catch (IOException)
    {
        Console.WriteLine("Fehler beim Lesen der CSV-Datei.");
    }
}
dateiEinlesen();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/{sprungpunkt}", (string sprungpunkt) =>
{
    sprungmarken.TryGetValue(sprungpunkt, out var result);
    if(result is null)
    {
        return Results.NotFound("Ziel nicht gefunden.");
    }
    return Results.Redirect(result);
});

app.Run();

