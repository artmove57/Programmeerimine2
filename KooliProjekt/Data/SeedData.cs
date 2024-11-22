using KooliProjekt.Data;
using Microsoft.AspNetCore.Components.Forms;
using System.Data;

public static class SeedData
{
    public static void Generate(ApplicationDbContext context)
    {
        if (context.Matches.Any())
        {
            return;
        }

        var list = new Matches();
        list.Title = "List 1";
        list.Items.Add(new Matches
        {
            Title = "Item 1.1"
        });

        context.Matches.Add(list);

        // Veel andmeid

        context.SaveChanges();
    }
}
public static void Generate(ApplicationDbContext context, UserManager<IdentityUser> userManager)
{
    if (context.Matches.Any())
    {
        return;
    }

    var user = new IdentityUser
    {
        UserName = "newuser@example.com",
        Email = "newuser@example.com",
        NormalizedUserName = "NEWUSER@EXAMPLE.COM",
        NormalizedEmail = "NEWUSER@EXAMPLE.COM"
    };


    userManager.CreateAsync(user, "Password123!").Wait();
 
    var tournament1 = new Tournament();
    tournament1.Name = "Bundesliga";
    tournament1.StartData = DataSetDateTime;
    tournament1.EndData = DataSetDateTime;
    tournament1.Description = "Football Match in German, 22 players are playing";

    var team1 = new Team();
    team1.Name = "Francfurt Eintraht";
    team1.Match = "Match1";

    var ranking1 = Ranking();
    ranking1.TotalPoints = InputText;