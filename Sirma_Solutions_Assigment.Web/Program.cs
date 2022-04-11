using Sirma_Solutions_Assigment.BussinessLogic.Contracts;
using Sirma_Solutions_Assigment.BussinessLogic.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddPageRoute("/csv/index", "");
}).AddRazorRuntimeCompilation();

builder.Services.AddControllers();
builder.Services.AddScoped<ICSVService, CSVService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapControllers();
app.MapRazorPages();


app.Run();
