using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Configuration;


// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<ContentReports> ContentReports { get; set; }
        public virtual DbSet<ConversationMembers> ConversationMembers { get; set; }
        public virtual DbSet<Conversations> Conversations { get; set; }
        public virtual DbSet<Friendships> Friendships { get; set; }
        public virtual DbSet<GroupMembers> GroupMembers { get; set; }
        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<Hashtags> Hashtags { get; set; }
        public virtual DbSet<MediaFiles> MediaFiles { get; set; }
        public virtual DbSet<MessageAttachments> MessageAttachments { get; set; }
        public virtual DbSet<MessageReceipts> MessageReceipts { get; set; }
        public virtual DbSet<Messages> Messages { get; set; }
        public virtual DbSet<Notifications> Notifications { get; set; }
        public virtual DbSet<PostMedia> PostMedia { get; set; }
        public virtual DbSet<PostTags> PostTags { get; set; }
        public virtual DbSet<Posts> Posts { get; set; }
        public virtual DbSet<RateLimits> RateLimits { get; set; }
        public virtual DbSet<Reactions> Reactions { get; set; }
        public virtual DbSet<SearchIndex> SearchIndex { get; set; }
        public virtual DbSet<Stories> Stories { get; set; }
        public virtual DbSet<TblAddress> TblAddress { get; set; }
        public virtual DbSet<TblSocial> TblSocial { get; set; }
        public virtual DbSet<TblUser> TblUser { get; set; }
        public virtual DbSet<TblUserPaymentCard> TblUserPaymentCard { get; set; }
        public virtual DbSet<TbleUserPassword> TbleUserPassword { get; set; }
        public virtual DbSet<UserFeed> UserFeed { get; set; }
        public virtual DbSet<UserMentions> UserMentions { get; set; }
        public virtual DbSet<TblPostLike> TblPostLike { get; set; }
        public virtual DbSet<TblNotification> TblNotification { get; set; }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<Business> Businesses { get; set; }
        public virtual DbSet<Feature> Features { get; set; }
        public virtual DbSet<BusinessFeature> BusinessFeatures { get; set; }
        public virtual DbSet<BusinessReview> BusinessReviews { get; set; }
        public virtual DbSet<BusinessImage> BusinessImages { get; set; }
        public virtual DbSet<UserBusinessFollow> user_business_follows { get; set; }
        public virtual DbSet<TblStoryLike> TblStoryLike { get; set; }
        public virtual DbSet<TblStoryView> TblStoryView { get; set; }
        public virtual DbSet<BusinessAmenity> TblBusinessAminities { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connStr = ConfigurationManager
                    .ConnectionStrings["DefaultConnection"]
                    .ConnectionString;

                optionsBuilder.UseSqlServer(connStr);
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comments>(entity =>
            {
                entity.HasKey(e => e.CommentId)
                    .HasName("PK__Comments__C3B4DFCA7B055521");

                entity.HasIndex(e => new { e.PostId, e.CreatedOn });

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_Post");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_User");
            });

            modelBuilder.Entity<ContentReports>(entity =>
            {
                entity.HasKey(e => e.ReportId)
                    .HasName("PK__ContentR__D5BD4805ECBF10AE");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.IsResolved).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Reporter)
                    .WithMany(p => p.ContentReports)
                    .HasForeignKey(d => d.ReporterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Report_Reporter");
            });

            modelBuilder.Entity<ConversationMembers>(entity =>
            {
                entity.HasKey(e => e.ConversationMemberId)
                    .HasName("PK__Conversa__6CF98427BE7F3769");

                entity.HasIndex(e => new { e.ConversationId, e.UserId })
                    .HasName("IX_ConversationMembers_Conversation_User");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsMuted).HasDefaultValueSql("((0))");

                entity.Property(e => e.JoinedOn).HasDefaultValueSql("(sysutcdatetime())");

                entity.HasOne(d => d.Conversation)
                    .WithMany(p => p.ConversationMembers)
                    .HasForeignKey(d => d.ConversationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CM_Conversation");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ConversationMembers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CM_User");
            });

            modelBuilder.Entity<Conversations>(entity =>
            {
                entity.HasKey(e => e.ConversationId)
                    .HasName("PK__Conversa__C050D877DB208581");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(sysutcdatetime())");
            });

            modelBuilder.Entity<Friendships>(entity =>
            {
                entity.HasKey(e => e.FriendshipId)
                    .HasName("PK__Friendsh__4D531A542EFD9126");

                entity.HasIndex(e => new { e.RequesterId, e.AddresseeId })
                    .HasName("UX_Friendship_Requester_Addressee")
                    .IsUnique();

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(sysutcdatetime())");

                entity.HasOne(d => d.Addressee)
                    .WithMany(p => p.FriendshipsAddressee)
                    .HasForeignKey(d => d.AddresseeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Friend_Addressee");

                entity.HasOne(d => d.Requester)
                    .WithMany(p => p.FriendshipsRequester)
                    .HasForeignKey(d => d.RequesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Friend_Requester");
            });

            modelBuilder.Entity<GroupMembers>(entity =>
            {
                entity.HasKey(e => e.GroupMemberId)
                    .HasName("PK__GroupMem__344812925772D5FA");

                entity.HasIndex(e => new { e.GroupId, e.UserId })
                    .HasName("UX_GroupMembers_Group_User")
                    .IsUnique();

                entity.Property(e => e.JoinedOn).HasDefaultValueSql("(sysutcdatetime())");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupMembers)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GM_Group");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GroupMembers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GM_User");
            });

            modelBuilder.Entity<Groups>(entity =>
            {
                entity.HasKey(e => e.GroupId)
                    .HasName("PK__Groups__149AF36A1A68D5C9");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Group_Creator");
            });

            modelBuilder.Entity<Hashtags>(entity =>
            {
                entity.HasKey(e => e.HashtagId)
                    .HasName("PK__Hashtags__BEFA912A84475444");

                entity.HasIndex(e => e.Tag)
                    .HasName("UQ__Hashtags__C4516413AC41CE82")
                    .IsUnique();

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<MediaFiles>(entity =>
            {
                entity.HasKey(e => e.MediaId)
                    .HasName("PK__MediaFil__B2C2B5CF68DED6AF");

                entity.HasIndex(e => e.OwnerUserId)
                    .HasName("IX_MediaFiles_Owner");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(sysutcdatetime())");

                entity.HasOne(d => d.OwnerUser)
                    .WithMany(p => p.MediaFiles)
                    .HasForeignKey(d => d.OwnerUserId)
                    .HasConstraintName("FK_Media_Owner");
            });

            modelBuilder.Entity<MessageAttachments>(entity =>
            {
                entity.HasKey(e => e.AttachmentId)
                    .HasName("PK__MessageA__442C64BE77F1800F");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.MessageAttachments)
                    .HasForeignKey(d => d.MessageId)
                    .HasConstraintName("FK_MA_Message");
            });

            modelBuilder.Entity<MessageReceipts>(entity =>
            {
                entity.HasKey(e => e.ReceiptId)
                    .HasName("PK__MessageR__CC08C4206288BD81");

                entity.HasIndex(e => new { e.MessageId, e.UserId })
                    .HasName("IX_MessageReceipts_Message_User");

                entity.Property(e => e.StatusOn).HasDefaultValueSql("(sysutcdatetime())");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.MessageReceipts)
                    .HasForeignKey(d => d.MessageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Receipt_Message");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MessageReceipts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Receipt_User");
            });

            modelBuilder.Entity<Messages>(entity =>
            {
                entity.HasKey(e => e.MessageId)
                    .HasName("PK__Messages__C87C0C9CA01E0C1F");

                entity.HasIndex(e => new { e.ConversationId, e.CreatedOn });

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Conversation)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.ConversationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Message_Conversation");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Message_Sender");
            });

            modelBuilder.Entity<Notifications>(entity =>
            {
                entity.HasKey(e => e.NotificationId)
                    .HasName("PK__Notifica__20CF2E1291BE5B8F");

                entity.HasIndex(e => new { e.UserId, e.IsRead, e.CreatedOn })
                    .HasName("IX_Notifications_User_IsRead");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.IsRead).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Actor)
                    .WithMany(p => p.NotificationsActor)
                    .HasForeignKey(d => d.ActorId)
                    .HasConstraintName("FK_Notification_Actor");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.NotificationsUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notification_User");
            });

            modelBuilder.Entity<PostMedia>(entity =>
            {
                entity.Property(e => e.SortOrder).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostMedia)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_PostMedia_Post");
            });

            modelBuilder.Entity<PostTags>(entity =>
            {
                entity.HasKey(e => e.PostTagId)
                    .HasName("PK__PostTags__325724FD06B3B301");

                entity.HasOne(d => d.Hashtag)
                    .WithMany(p => p.PostTags)
                    .HasForeignKey(d => d.HashtagId)
                    .HasConstraintName("FK__PostTags__Hashta__42E1EEFE");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostTags)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK__PostTags__PostId__41EDCAC5");
            });

            modelBuilder.Entity<Posts>(entity =>
            {
                entity.HasKey(e => e.PostId)
                    .HasName("PK__Posts__AA1260181FCF6913");

                entity.HasIndex(e => new { e.UserId, e.CreatedOn });

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Post_User");
            });

            modelBuilder.Entity<RateLimits>(entity =>
            {
                entity.HasKey(e => e.RateLimitId)
                    .HasName("PK__RateLimi__0B581845C15C954D");

                entity.Property(e => e.Count).HasDefaultValueSql("((0))");

                entity.Property(e => e.WindowStart).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RateLimits)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__RateLimit__UserI__30C33EC3");
            });

            modelBuilder.Entity<Reactions>(entity =>
            {
                entity.HasKey(e => e.ReactionId)
                    .HasName("PK__Reaction__46DDF9B4933872DA");

                entity.HasIndex(e => new { e.UserId, e.PostId, e.CommentId })
                    .HasName("UX_Reaction_User_Target")
                    .IsUnique();

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(sysutcdatetime())");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.Reactions)
                    .HasForeignKey(d => d.CommentId)
                    .HasConstraintName("FK_Reaction_Comment");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Reactions)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_Reaction_Post");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reactions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reaction_User");
            });

            modelBuilder.Entity<SearchIndex>(entity =>
            {
                entity.HasKey(e => e.SearchId)
                    .HasName("PK__SearchIn__21C535F484ACCA41");

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Stories>(entity =>
            {
                entity.HasKey(e => e.StoryId)
                    .HasName("PK__Stories__3E82C048965EC5E3");

                entity.HasIndex(e => new { e.UserId, e.ExpiresOn });

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Stories)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Story_User");
            });

            modelBuilder.Entity<TblAddress>(entity =>
            {
                entity.HasKey(e => e.AddressId)
                    .HasName("PK__TblAddre__091C2AFBB2FE51AF");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblAddress)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TblAddres__UserI__3C69FB99");
            });

            modelBuilder.Entity<TblSocial>(entity =>
            {
                entity.HasKey(e => e.SocialId)
                    .HasName("PK__TblSocia__67CF711A78A170B5");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblSocial)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TblSocial__UserI__4222D4EF");
            });

            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .HasName("UQ__TblUser__A9D10534C2F22E84")
                    .IsUnique();

                entity.HasIndex(e => e.LastActiveOn);

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblUserPaymentCard>(entity =>
            {
                entity.HasKey(e => e.PaymentCardId)
                    .HasName("PK__TblUserP__833403D82358948C");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblUserPaymentCard)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TblUserPa__UserI__47DBAE45");
            });

            modelBuilder.Entity<TbleUserPassword>(entity =>
            {
                entity.Property(e => e.AddedOn).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbleUserPassword)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Tble_UserPassword_Users");
            });

            modelBuilder.Entity<UserFeed>(entity =>
            {
                entity.HasKey(e => e.FeedId)
                    .HasName("PK__UserFeed__1586DF5566872EAB");

                entity.HasIndex(e => new { e.UserId, e.CreatedOn });

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.IsRead).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<UserMentions>(entity =>
            {
                entity.HasKey(e => e.MentionId)
                    .HasName("PK__UserMent__5D9162DCFDD9DD8D");

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.UserMentions)
                    .HasForeignKey(d => d.CommentId)
                    .HasConstraintName("FK__UserMenti__Comme__3D2915A8");

                entity.HasOne(d => d.MentionedUser)
                    .WithMany(p => p.UserMentions)
                    .HasForeignKey(d => d.MentionedUserId)
                    .HasConstraintName("FK__UserMenti__Menti__3E1D39E1");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.UserMentions)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK__UserMenti__PostI__3C34F16F");
            });

            modelBuilder.Entity<BusinessFeature>()
           .HasKey(bf => new { bf.BusinessId, bf.FeatureId });

            modelBuilder.Entity<TblPostLike>()
            .HasOne(l => l.User)
            .WithMany()
            .HasForeignKey(l => l.UserId);

            modelBuilder.Entity<TblStoryLike>(entity =>
            {
                entity.ToTable("TblStoryLike");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.ReactionType)
                      .HasMaxLength(20);

                entity.HasOne(e => e.Story)
                      .WithMany()
                      .HasForeignKey(e => e.StoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.LikedByUser)
                      .WithMany()
                      .HasForeignKey(e => e.LikedBy)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.StoryCreatedByUser)
                      .WithMany()
                      .HasForeignKey(e => e.StoryCreatedBy)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<TblStoryView>(entity =>
            {
                entity.ToTable("TblStoryView");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.ViewedOn)
                      .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.IsLiked)
                      .HasDefaultValue(false);

                // Foreign Key: StoryId → Stories(Id)
                entity.HasOne(e => e.Story)
                      .WithMany()
                      .HasForeignKey(e => e.StoryId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Foreign Key: ViewedBy → TblUser(Id)
                entity.HasOne(e => e.ViewedByUser)
                      .WithMany()
                      .HasForeignKey(e => e.ViewedBy)
                      .OnDelete(DeleteBehavior.Cascade);

                // ✅ Optional: Unique constraint (1 user per story)
                entity.HasIndex(e => new { e.StoryId, e.ViewedBy })
                      .IsUnique();
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
