using IdentitySample.Helpers;
using IdentitySample.Models;
using IdentitySample.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataBaseContext>(p => p.UseSqlServer("Server=.;Database=IdentitySample;User Id=sa;Password=.;TrustServerCertificate=True"));
builder.Services.AddControllersWithViews();

builder.Services.AddIdentity<User, Role>().
                    AddEntityFrameworkStores<DataBaseContext>().
                        AddDefaultTokenProviders().
                        AddRoles<Role>().
                        AddErrorDescriber<CustomIdentityError>()
                        .AddPasswordValidator<MyPasswordValidator>();

builder.Services.AddAuthorization(options =>
                    {
                        options.AddPolicy("AdminUsers", policy =>
                        {
                            policy.RequireRole("Admin");
                        });
                        options.AddPolicy("BuyerPolicy", policy =>
                        {
                            policy.RequireClaim("Buyer");
                        });
                        options.AddPolicy("BloodType", policy =>
                        {
                            policy.RequireClaim("Blood", "Ap", "Op");
                        });
                        options.AddPolicy("IsBlogForUser", policy =>
                        {
                            policy.AddRequirements(new BlogRequirement());
                        });
                    }
);

//builder.Services.Configure<IdentityOptions>(
//                                        option =>
//                                        {
//                                            //User setting
//                                            option.User.AllowedUserNameCharacters = "abcs";
//                                            option.User.RequireUniqueEmail = true;

//                                            //Password setting
//                                            option.Password.RequireDigit = true; //حتما کاراکتر عددی در پسورد استفاده شود
//                                            option.Password.RequireLowercase = true; //حتما کاراکتر حرف کوچک در پسورد استفاده شود
//                                            option.Password.RequireUppercase = true; //حتما کاراکتر بزرگ در پسورد استفاده شود
//                                            option.Password.RequireNonAlphanumeric = true; // !@#$%^&*()-+ //حتما کاراکتر خاص در پسورد استفاده شود
//                                            option.Password.RequiredLength = 8; //حداقل طول پسورد
//                                            option.Password.RequiredUniqueChars = 1; // چه تعداد کاراکتر غیرتکراری در پسورد لحاظ شود

//                                            //Lockout setting
//                                            option.Lockout.MaxFailedAccessAttempts = 3; //کاربر تا چند مرتبه پسورد را اشتباه بزند لاک می شود؟
//                                            option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10); // مدت زمان لاک شدن کاربر چقدر باشد

//                                            //SignIn setting
//                                            option.SignIn.RequireConfirmedAccount = true;
//                                            option.SignIn.RequireConfirmedEmail = true;
//                                            option.SignIn.RequireConfirmedPhoneNumber = true;
//                                        });

//builder.Services.ConfigureApplicationCookie(
//                                        options =>
//                                        {
//                                            //Cookie setting
//                                            options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
//                                            options.LoginPath = "/Account/Login";
//                                            options.AccessDeniedPath = "/Account/AccessDenied";
//                                            options.SlidingExpiration = true;
//                                        });

//builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>, AddMyClaims>();
builder.Services.AddScoped<IClaimsTransformation, AddClaims>();
//builder.Services.AddSingleton<IAuthorizationHandler, UserCreditHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, IsBlogForUserAuthorizationHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
