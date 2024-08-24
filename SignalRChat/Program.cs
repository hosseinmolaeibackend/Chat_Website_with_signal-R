using CoreLayer.Services.Chats;
using CoreLayer.Services.Chats.ChatGroups;
using CoreLayer.Services.Roles;
using CoreLayer.Services.Users;
using CoreLayer.Services.Users.UserGroups;
using DataLayer.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SignalRChat.Hubs;
using System.Net;

namespace SignalRChat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<EchatContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("default"));
            });

            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IChatGroupService, ChatGroupService>();
            builder.Services.AddScoped<IUserGroupService, UserGroupService>();


            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddSignalR();


            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(option =>
            {
                option.LoginPath = "/auth#Login";
                option.LogoutPath = "/auth/Logout";
                option.ExpireTimeSpan = TimeSpan.FromDays(30);
            });


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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapHub<ChatHub>("/chat");

            app.Run();
        }
    }
}
