using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using ReshDiscordNetLibrary;

namespace AzurLaneBBot.Database.Models;

public partial class AzurlanedbContext : DbContext
{
    public AzurlanedbContext()
    {
    }

    public AzurlanedbContext(DbContextOptions<AzurlanedbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public virtual DbSet<BoobaBotProject> BoobaBotProjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        => optionsBuilder.UseSqlite($"Data Source={AppDomain.CurrentDomain.BaseDirectory}{ConfigurationManager.AppSettings["dbRelativeLocation"]}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BoobaBotProject>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("BOOBA_BOT_PROJECT");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
