﻿namespace BeautySalon.Data
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Models;
    using BeautySalon.Data.Models;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
            typeof(ApplicationDbContext).GetMethod(
                nameof(SetIsDeletedQueryFilter),
                BindingFlags.NonPublic | BindingFlags.Static);

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<ChatGroup> ChatGroups { get; set; }

        public DbSet<ChatMessage> ChatMessages { get; set; }

        public DbSet<ClientArticleLike> ClientArticleLikes { get; set; }

        public DbSet<ClientProductLike> ClientProductLikes { get; set; }

        public DbSet<ClientSkinProblem> ClientSkinProblems { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<JobType> JobTypes { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Procedure> Procedures { get; set; }

        public DbSet<ProcedureProduct> ProcedureProducts { get; set; }

        public DbSet<ProcedureStylist> ProcedureStylists { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductOrder> ProductOrders { get; set; }

        public DbSet<ProductReview> ProductReviews { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<QuizAnswer> QuizAnswers { get; set; }

        public DbSet<QuizQuestion> QuizQuestions { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<SkinProblem> SkinProblems { get; set; }

        public DbSet<SkinProblemProcedure> SkinProblemProcedures { get; set; }

        public DbSet<SkinType> SkinTypes { get; set; }

        public DbSet<TypeCard> TypeCards { get; set; }

        public DbSet<UserChatGroup> UserChatGroups { get; set; }

        public override int SaveChanges() => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            this.SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Needed for Identity models configuration
            base.OnModelCreating(builder);

            // builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            this.ConfigureUserIdentityRelations(builder);

            EntityIndexesConfiguration.Configure(builder);

            var entityTypes = builder.Model.GetEntityTypes().ToList();

            // Set global query filter for not deleted entities only
            var deletableEntityTypes = entityTypes
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
                method.Invoke(null, new object[] { builder });
            }

            // Disable cascade delete
            var foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
            where T : class, IDeletableEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        // Applies configurations
        private void ConfigureUserIdentityRelations(ModelBuilder builder)
             => builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
