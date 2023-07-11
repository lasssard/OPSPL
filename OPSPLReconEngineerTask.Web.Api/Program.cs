using OPSPLReconEngineerTask.Data.DbContext;
using OPSPLReconEngineerTask.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped<OPSPLTaskContext>(_ =>
    new OPSPLTaskContext(builder.Configuration.GetConnectionString("OPSPL")));
builder.Services.AddScoped<IWordInverter, WordInverter>();
builder.Services.AddScoped<IInvertWordsService, InvertWordsService>();
builder.Services.AddScoped<IBookSearchService, BookSearchService>();
builder.Services.AddScoped<IReportBuildService, ReportBuildService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();