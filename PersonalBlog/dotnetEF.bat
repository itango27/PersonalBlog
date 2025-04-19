dotnet ef migrations add InitialCreate --context PersonalBlogIdentityContext --project PersonalBlog.csproj
dotnet ef database update --context PersonalBlogIdentityContext --project PersonalBlog.csproj
dotnet ef migrations add InitialCreate --context PersonalBlogContext --project PersonalBlog.csproj
dotnet ef database update --context PersonalBlogContext --project PersonalBlog.csproj
dotnet ef migrations add AddedScaffolding --context PersonalBlogContext --project PersonalBlog.csproj
dotnet ef database update --context PersonalBlogContext --project PersonalBlog.csproj