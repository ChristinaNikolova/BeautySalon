﻿// ReSharper disable VirtualMemberCallInConstructor
namespace BeautySalon.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Data.Common.Models;
    using BeautySalon.Data.Models.Enums;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.IsSkinSensitive = false;

            // Client
            this.Questions = new HashSet<Question>();
            this.ArticleLikes = new HashSet<ClientArticleLike>();
            this.Comments = new HashSet<Comment>();
            this.ClientSkinProblems = new HashSet<ClientSkinProblem>();
            this.ProductReviews = new HashSet<ProductReview>();
            this.ProductLikes = new HashSet<ClientProductLike>();
            this.Orders = new HashSet<Order>();
            this.Reviews = new HashSet<Review>();
            this.ClientAppointments = new HashSet<Appointment>();

            // Stylist
            this.Answers = new HashSet<Answer>();
            this.Articles = new HashSet<Article>();
            this.StylistProcedures = new HashSet<ProcedureStylist>();
            this.StylistAppointments = new HashSet<Appointment>();

            this.ReceivedMessages = new HashSet<ChatMessage>();
            this.SendMessages = new HashSet<ChatMessage>();
            this.ClientChatGroups = new HashSet<UserChatGroup>();
            this.AdminChatGroups = new HashSet<UserChatGroup>();

            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
        }

        // Data
        [Required]
        [MaxLength(DataValidation.UserFirstNameMaxLenght)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(DataValidation.UserLastNameMaxLenght)]
        public string LastName { get; set; }

        public string Picture { get; set; }

        public string Address { get; set; }

        public Gender Gender { get; set; }

        public string SkinTypeId { get; set; }

        public virtual SkinType SkinType { get; set; }

        public bool? IsSkinSensitive { get; set; }

        public string CardId { get; set; }

        public virtual Card Card { get; set; }

        // Stylist
        public string CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public string JobTypeId { get; set; }

        public virtual JobType JobType { get; set; }

        [MaxLength(DataValidation.StylistDescriptionMaxLenght)]
        public string Description { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        // Client
        public virtual ICollection<Question> Questions { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<ClientArticleLike> ArticleLikes { get; set; }

        public virtual ICollection<ClientSkinProblem> ClientSkinProblems { get; set; }

        public virtual ICollection<ProductReview> ProductReviews { get; set; }

        public virtual ICollection<ClientProductLike> ProductLikes { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<Appointment> ClientAppointments { get; set; }

        // Stylist
        public virtual ICollection<Answer> Answers { get; set; }

        public virtual ICollection<Article> Articles { get; set; }

        public virtual ICollection<ProcedureStylist> StylistProcedures { get; set; }

        public virtual ICollection<Appointment> StylistAppointments { get; set; }

        // All
        public virtual ICollection<ChatMessage> ReceivedMessages { get; set; }

        public virtual ICollection<ChatMessage> SendMessages { get; set; }

        public virtual ICollection<UserChatGroup> ClientChatGroups { get; set; }

        public virtual ICollection<UserChatGroup> AdminChatGroups { get; set; }

        // Default
        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}
